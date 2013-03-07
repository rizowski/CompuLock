using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Timers;
using ActiveDs;
using Cassia;
using Database;
using Database.Models;

namespace Service.Profile
{
    public class AccountManager
    {
        private const string WinNT = "WinNT://";
        private const string UserFlags = "UserFlags";

        private DatabaseManager DbManager { get; set; }

        public string UserName;
        public ITerminalServer Server;

        private Timer UpdateTimer;
        private Timer LockoutTimer;

        //                                                                  60 min
        public AccountManager( double interval = 5, double updateInterval = 3600)
        {
            DbManager = new DatabaseManager("settings","");
            SetupUpdateTimer(updateInterval);
            SetupLockoutTimer(interval);
            ITerminalServicesManager manager = new TerminalServicesManager();
            Server = manager.GetLocalServer();
            //ForceUpdate();
            Update(null, null);
        }

        // checks to see what user is logged in
        // Checks to see if the logged in user is being tracked
        // checks to see if the tracked user has run out of time.
        private void Update(object sender, ElapsedEventArgs e)
        {
            var accounts = GetLoggedInAccounts();
            Console.WriteLine("{0} accounts logged in", accounts.Count());
            foreach (var account in accounts)
            {// find out if the count is different and update the list
                var dbaccount = GetDbAccounts().FirstOrDefault(a => a.Username == account.Username);

                if (dbaccount == null)
                {
                    var computer = DbManager.GetComputer();
                    account.ComputerId = computer.Id;
                    DbManager.SaveAccount(account);
                }
                else
                {
                    account.Id = dbaccount.Id;
                    account.ComputerId = dbaccount.ComputerId;
                    DbManager.UpdateAccount(account);
                }
            }
        }

        private void LockUpdate(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Lock Tick");
            var accounts = GetLoggedInAccounts();
            foreach (var account in accounts)
            {
                var dbaccount = GetDbAccounts().FirstOrDefault(a => a.Username == account.Username);

                if (dbaccount == null) continue;
                if (!dbaccount.Tracking) continue;

                if (dbaccount.AllottedTime <= TimeSpan.FromSeconds(0))
                {
                    if(!dbaccount.Locked)
                        LockAccount(dbaccount);
                }
                else
                {
                    if(dbaccount.Locked)
                        UnlockAccount(dbaccount);
                }
            }
        }

        public IEnumerable<Account> ForceUpdate()
        {
            var accounts = GetDbAccounts();
            if (accounts.Any())
            {
                var loggedins = GetLoggedInAccounts();
                foreach (var loggedin in loggedins)
                {
                    var found = accounts.FirstOrDefault(a => a.Username == loggedin.Username);
                    if (found == null)
                        DbManager.SaveAccount(loggedin);
                }
            }
            return GetDbAccounts();
        } 

        #region Timer
        private void TimerDisposed(object sender, EventArgs e)
        {
            DisconnectUser(UserName, true);
            LockAccount(UserName);
        }

        public void StartUpdateTimer()
        {
            UpdateTimer.Start();
        }

        public void StopUpdateTimer()
        {
            UpdateTimer.Stop();
            UpdateTimer.Dispose();
        }

        public void StartLockoutTimer()
        {
            LockoutTimer.Start();
        }

        public void StopLockoutTimer()
        {
            LockoutTimer.Stop();
            LockoutTimer.Dispose();
        }

        private void SetupLockoutTimer(double interval)
        {
            Console.WriteLine("Setting up Account Manager Lockout Timer");
            LockoutTimer = new Timer(interval * 1000) { AutoReset = true };
            LockoutTimer.Elapsed += LockUpdate;
            LockoutTimer.Start();
        }
        private void SetupUpdateTimer(double interval)
        {
            Console.WriteLine("Setting up Account Manager Update Timer");
            UpdateTimer = new Timer(interval * 1000) { AutoReset = true };
            UpdateTimer.Elapsed += Update;
            UpdateTimer.Start();
        }
        #endregion

        #region Sessions
        public int GetSessionId(string username)
        {
            var session = Server.GetSessions().First(a => a.UserName == username);
            if (session != null)
                return session.SessionId;
            return -1;
        }
        public List<Account> GetLoggedOnSessions()
        {
            var sessions = Server.GetSessions();
            List<Account> accounts = new List<Account>();
            foreach (var session in sessions)
            {
                var account = DbManager.GetAccounts().FirstOrDefault(a => a.Username == session.UserName);
                if (account != null)
                    accounts.Add(account);
            }
            return accounts;
        }
        public ITerminalServicesSession GetUserSession(string username = null)
        {
            if (username == null)
            {
                username = UserName;
            }
            ITerminalServicesSession temp = null;
            Server.Open();

            foreach (var session in Server.GetSessions())
            {
                if (session.UserName.Equals(username))
                {
                    temp = session;
                }
            }
            Server.Close();
            return temp;
        }
        public ITerminalServicesSession GetUserSession(int sessionId)
        {
            if (sessionId < 0)
            {
                throw new ArgumentException("Id must be 0 or above");
            }
            ITerminalServicesSession temp = null;
            Server.Open();

            foreach (var session in Server.GetSessions())
            {
                if (session.SessionId == sessionId)
                {
                    temp = session;
                }
            }
            Server.Close();
            return temp;
        }
        #endregion

        #region Management
        public void LockAccount(Account account)
        {
            
            LockAccount(account.Domain, account.Username);
            account.Locked = true;
            DbManager.UpdateAccount(account);
        }
        public void LockAccount(string domain, string username)
        {
            DisconnectUser(username);
            Console.WriteLine("Locking the account: {0}\\{1}", domain, username);
            try
            {
                using (DirectoryEntry user = new DirectoryEntry(WinNT + Environment.MachineName + "/" + username))
                {
                    user.Properties[UserFlags].Value = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;
                    user.CommitChanges();
                }
                Console.WriteLine("Lock Completed.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void LockAccount(string username)
        {
            try
            {
                Console.Write("Locking the account: {0}", username);
                DirectoryEntry user = new DirectoryEntry("WinNT://" + Environment.MachineName + "/" + username);
                Console.Write(".");
                user.Properties["UserFlags"].Value = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;
                Console.Write(".");
                user.CommitChanges();
                Console.WriteLine(".");
                user.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The account has been locked.");
        }
        public bool IsLocked(Account account)
        {
            return IsLocked(account.Username);
        }
        public bool IsLocked(string username)
        {
            bool value = false;
            try
            {
                DirectoryEntry user;
                using (user = new DirectoryEntry(WinNT + Environment.MachineName + "/" + username))
                {
                    var flag = (ADS_USER_FLAG)user.Properties[UserFlags].Value;
                    if (flag.Equals(515))
                        value = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return value;
        }
        public void StartTracking(Account account)
        {
            account.Tracking = true;
            DbManager.SaveAccount(account);
        }
        public void StopTracking(Account account)
        {
            account.Tracking = false;
            DbManager.SaveAccount(account);
        }
        public void DisconnectUser(string username = null, bool synchronous = false)
        {
            if (username == null)
            {
                username = UserName;
            }
            var session = GetUserSession(username);
            if (session != null)
                session.Disconnect(synchronous);
            else
                throw new Exception("Session could not be found for user: " + username);
        }
        public void DisconnectUser(int sessionId, bool synchronous = false)
        {
            if (sessionId < 0)
            {
                throw new ArgumentException("Id must be 0 or above");
            }
            var session = GetUserSession(sessionId);
            if (session != null)
                session.Disconnect(synchronous);
            else
                throw new Exception("Session could not be found for SessionId: " + sessionId);
        }
        public void UnlockAccount(Account account)
        {
            UnlockAccount(account.Username);
            account.Locked = false;
            DbManager.UpdateAccount(account);
        }
        public void UnlockAccount(string username = null)
        {
            if (username == null)
            {
                username = UserName;
            }
            try
            {
                Console.Write("UnLocking the account: {0}", username);
                DirectoryEntry user = new DirectoryEntry("WinNT://" + Environment.MachineName + "/" + username);
                user.Properties["UserFlags"].Value = ADS_USER_FLAG.ADS_UF_NORMAL_ACCOUNT;
                user.CommitChanges();
                user.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error with unlocking the account");
            }
            Console.WriteLine("Account has been unlocked.");
        }

        // gets all the logged in accounts with their current state
        public IEnumerable<Account> GetLoggedInAccounts()
        {
            List<Account> list = new List<Account>();
            
            var sessions = Server.GetSessions();
            foreach (var session in sessions)
            {
                var account = new Account
                {
                    Domain = session.DomainName,
                    Username = session.UserName,
                    Locked = IsLocked(session.UserName),
                    UpdatedAt = DateTime.Now
                };

                list.Add(account);
            }
            return list;
        }
        // gets all the logged db users with their current state
        public IEnumerable<Account> GetDbAccounts()
        {
            return DbManager.GetAccounts();
        }
        
        #endregion
    }
}