using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Management;
using System.Timers;
using ActiveDs;
using Cassia;
using Database;
using Database.Models;
using Microsoft.Win32;

namespace Service.Profile
{
    public class AccountManager
    {
        private const string WinNT = "WinNT://";
        private const string UserFlags = "UserFlags";

        private DatabaseManager DbManager { get; set; }

        public string UserName;
        public ITerminalServicesManager manager;

        public double Interval { get; set; }

        private Timer UpdateTimer;
        private Timer LockoutTimer;
        private Timer LoggingTimer;

        //                                                                  60 min
        public AccountManager( double interval = 5, double updateInterval = 3600)
        {
            Interval = interval;
            DbManager = new DatabaseManager("settings","");
            SetupUpdateTimer(Interval);
            SetupLockoutTimer(Interval);
            SetupLoggingTimer(Interval);
            manager = new TerminalServicesManager();
            //ForceUpdate();
            Update(null, null);
        }

        // checks to see what user is logged in
        // Checks to see if the logged in user is being tracked
        // checks to see if the tracked user has run out of time.
        private void Update(object sender, ElapsedEventArgs e)
        {
            var accounts = GetLoggedInAccounts();
            var dbaccounts = GetDbAccounts();
            var computer = DbManager.GetComputer();
            Console.WriteLine("{0} accounts logged in", accounts.Count());
            if (dbaccounts.Count() < accounts.Count())
            {
                foreach (var account in accounts)
                {
// find out if the count is different and update the list
                    var dbaccount = dbaccounts.FirstOrDefault(a => a.Username == account.Username);

                    if (dbaccount == null)
                    {
                        if (account.Username.Length > 0)
                        {
                            account.ComputerId = computer.WebId;
                            DbManager.SaveAccount(account);
                        }
                    }
                }
            }
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            var dbaccounts = GetDbAccounts();
            var inAccounts = GetLoggedInAccounts();
            foreach (var inAccount in inAccounts)
            {
                var loggedin = dbaccounts.FirstOrDefault(a => a.Username == inAccount.Username);
                if (loggedin == null) return;

                if (loggedin.Tracking)
                {
                    if (loggedin.AllottedTime > 0)
                    {
                        loggedin.AllottedTime -= (int)Interval;
                        loggedin.UsedTime += (int)Interval;
                        DbManager.UpdateAccount(loggedin);
                    }
                }
            }
        }

        private void LockUpdate(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Lock Tick");
            var accounts = GetLoggedInAccounts();
            var dbAccounts = GetDbAccounts();
            foreach (var account in accounts)
            {
                var dbaccount = dbAccounts.FirstOrDefault(a => a.Username == account.Username);

                if (dbaccount == null) continue;
                if (!dbaccount.Tracking)
                {
                    if (IsLocked(dbaccount.Username))
                    {
                        UnlockAccount(account);
                    }
                }
                else
                {
                    if (dbaccount.AllottedTime <= 0)
                    {
                        if (!IsLocked(dbaccount.Username))
                            LockAccount(dbaccount);
                    }
                    else
                    {
                        if (IsLocked(dbaccount.Username))
                            UnlockAccount(dbaccount);
                    }
                }
                
            }
        }

        private void Logg(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Logg");
            var accounts = GetLoggedInAccounts();
            var dbAccounts = GetDbAccounts();
            var dbprocesses = DbManager.GetProcesses();

            foreach (var account in accounts)
            {
                var dbaccount = dbAccounts.FirstOrDefault(a => a.Username == account.Username);
                if (dbaccount == null) return;
                if (dbaccount.Tracking)
                {
                    if (!dbaccount.Locked)
                    {
                        foreach (var process in account.Processes)
                        {
                            var dbprocess = dbprocesses.FirstOrDefault(p => p.Name == process.Name);
                            if (dbprocess == null)
                            {
                                if (dbaccount.Id != 0)
                                {
                                    process.AccountId = dbaccount.Id;
                                    DbManager.SaveProcess(process);
                                }
                            }
                            else
                            {
                                if (dbaccount.WebId != 0 && dbprocess.AccountId != dbaccount.WebId)
                                {
                                    dbprocess.AccountId = dbaccount.WebId;
                                    DbManager.UpdateProcess(dbprocess);
                                }
                            }
                        }
                    }
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
            UpdateTimer.Elapsed += Tick;
            UpdateTimer.Start();
        }

        private void SetupLoggingTimer(double interval)
        {
            LoggingTimer = new Timer(interval * 1000){AutoReset = true};
            LoggingTimer.Elapsed += Logg;
            LoggingTimer.Start();
        }
        #endregion

        #region Sessions
        public int GetSessionId(string username)
        {
            using (ITerminalServer Server = manager.GetLocalServer())
            {
                Server.Open();
                var session = Server.GetSessions().First(a => a.UserName == username);
                if (session != null)
                    return session.SessionId;
            }
            return -1;
        }
        public List<Account> GetLoggedOnSessions()
        {
            List<Account> accounts = new List<Account>();
            using (ITerminalServer Server = manager.GetLocalServer())
            {
                Server.Open();
                var sessions = Server.GetSessions();
                foreach (var session in sessions)
                {
                    var account = DbManager.GetAccounts().FirstOrDefault(a => a.Username == session.UserName);
                    if (account != null)
                        accounts.Add(account);
                }
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
            using (ITerminalServer Server = manager.GetLocalServer())
            {
                Server.Open();
                foreach (var session in Server.GetSessions())
                {
                    if (session.UserName.Equals(username))
                    {
                        temp = session;
                    }
                }
            }
            return temp;
        }
        public ITerminalServicesSession GetUserSession(int sessionId)
        {
            if (sessionId < 0)
            {
                throw new ArgumentException("Id must be 0 or above");
            }
            ITerminalServicesSession temp = null;
            using (ITerminalServer Server = manager.GetLocalServer())
            {
                Server.Open();
                foreach (var session in Server.GetSessions())
                {
                    if (session.SessionId == sessionId)
                    {
                        temp = session;
                    }
                }
            }
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
                    if (flag == (ADS_USER_FLAG) 515)
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
            using (ITerminalServer Server = manager.GetLocalServer())
            {
                Server.Open();
                try
                {
                    var sessions = Server.GetSessions();
                    
                    foreach (var session in sessions)
                    {
                        var processes = session.GetProcesses();
                        List<Process> processess = new List<Process>();
                        foreach (var process in processes)
                        {
                            processess.Add(new Process
                            {
                                Name = process.ProcessName
                            });
                        }
                        var account = new Account
                        {
                            Domain = session.DomainName,
                            Username = session.UserName,
                            Locked = IsLocked(session.UserName),
                            Processes = processess,
                            UpdatedAt = DateTime.Now
                        };

                        list.Add(account);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
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