using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management;
using System.Security.Principal;
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

        private List<Account> LoggedInAccounts { get; set; }

        public string Domain;
        public DateTime StartTime;
        public TimeSpan ElapsedTime;
        public TimeSpan LockTime;
        public string UserName;
        public Timer Timer;
        public ITerminalServer Server;

        private Timer UpdateTimer;
        private Timer LockoutTimer;

        public AccountManager(DatabaseManager dbmanager, double interval = 1, double updateInterval = 3600)
        {
            DbManager = dbmanager;
            SetupTimer(interval);
            SetupUpdateTimer(updateInterval);
            SetupLockoutTimer(interval);
            ITerminalServicesManager manager = new TerminalServicesManager();
            Server = manager.GetLocalServer();
            Load();
        }

        private void SetupLockoutTimer(double interval)
        {
            Console.WriteLine("Setting up Account Manager Lockout Timer");
            LockoutTimer = new Timer(interval*1000){AutoReset = true};
            LockoutTimer.Elapsed += Update;
            LockoutTimer.Start();
        }

        // checks to see what user is logged in
        // Checks to see if the logged in user is being tracked
        // checks to see if the tracked user has run out of time.
        private void Update(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Checking for Lockout");
            Console.WriteLine("{0} accounts logged in", LoggedInAccounts.Count);
            foreach (var account in LoggedInAccounts)
            {
                if (account.Tracking)
                {
                    Console.WriteLine("{0} is being tracked", account.Username);
                    if (account.AllottedTime.TotalSeconds <= 0)
                    {
                        Console.WriteLine("Locking...");
                        LockAccount(account);
                    }
                    if (account.AllottedTime.TotalSeconds > 0)
                    {
                        UnlockAccount(account.Username);
                    }
                }
            }
        }

        // Loads the account information from the computer
        // Checks to see if each account has been saved to the db.
        // If the account is in the db, updates the allotted time for the listed account
        private void Load()
        {
            LoggedInAccounts = new List<Account>();
            var accounts = GetLoggedInAccounts();
            var dbAccounts = GetDbAccounts();
            foreach (var account in accounts)
            {
                var foundAccount = dbAccounts.FirstOrDefault(a => a.Username == account.Username);
                if (foundAccount == null)
                    foundAccount = DbManager.SaveAccount(account);
                LoggedInAccounts.Add(foundAccount);
            }
        }

        private void SetupUpdateTimer(double interval)
        {
            Console.WriteLine("Setting up Account Manager Update Timer");
            UpdateTimer = new Timer(interval * 1000){AutoReset = true};
            UpdateTimer.Elapsed += ForceUpdate;
            UpdateTimer.Start();
        }

        private void ForceUpdate(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Tick");
            var computer = DbManager.GetComputer();
            DbManager.SaveAccounts(computer.Id, (List<Account>) GetLoggedInAccounts());
        }

        public List<Account> GetLoggedOnSessions()
        {
            var sessions = Server.GetSessions();
            List<Account> accounts = new List<Account>();
            foreach (var session in sessions)
            {
                var account = DbManager.GetAccounts().FirstOrDefault(a => a.Username == session.UserName);
                if(account != null)
                    accounts.Add(account);
            }
            return accounts;
        }

        public int GetSessionId(string username)
        {
            var session = Server.GetSessions().First(a => a.UserName == username);
            if (session != null)
                return session.SessionId;
            return -1;
        }

        private void SetupTimer(double interval)
        {
            Timer = new Timer(interval * 1000) { AutoReset = true };
            Timer.Disposed += TimerDisposed;
            Timer.Elapsed += Tick;
            ElapsedTime = TimeSpan.Zero;
        }

        public void LockAccount(Account account)
        {
            LockAccount(account.Domain, account.Username);
        }

        public void LockAccount(string domain, string username)
        {
            Console.WriteLine("Locking the account: {0}/{1}", domain, username);
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

        #region Timer
        private void TimerDisposed(object sender, EventArgs e)
        {
            DisconnectUser(UserName, true);
            LockAccount(UserName);
        }

        public void StartTimer(double interval = 1)
        {
            double milli = interval * 1000;
            Timer.Enabled = true;
            Timer.Interval = milli;
            StartTime = DateTime.Now;
            Timer.Start();
        }

        public void StopTimer()
        {
            Timer.Stop();
            Timer.Dispose();
        }

        public void ResetTimer()
        {
            ElapsedTime = TimeSpan.Zero;
        }

        public void SetTimer(DateTime time)
        {
            LockTime = new TimeSpan(time.Hour, time.Minute, time.Second);
        }
        public void SetTimer(int hours, int minutes, int seconds)
        {
            LockTime = new TimeSpan(hours, minutes, seconds);
        }
        private void Tick(object sender, ElapsedEventArgs e)
        {
            var inbetweentime = DateTime.Now - StartTime;
            inbetweentime = new TimeSpan(inbetweentime.Hours, inbetweentime.Minutes, inbetweentime.Seconds);
            ElapsedTime = inbetweentime;

            if (ElapsedTime.CompareTo(LockTime) >= 0)
            {
                Console.WriteLine(ElapsedTime.CompareTo(LockTime));
                Timer.Dispose();
            }
            Console.WriteLine("Elapsed: {0}", ElapsedTime);
        }
        #endregion
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

        #region Production Code
        public int GetUserSessionId(string username=null)
        {
            if(username == null)
            {
                username = UserName;
            }
                Server.Open();
                foreach (var session in Server.GetSessions())
                {
                    if (session.UserName.Equals(username))
                    {
                        return session.SessionId;
                    }
                }
                Server.Close();
            return -1;
        }

        public ITerminalServicesSession GetUserSession(string username=null)
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

        public void DisconnectUser(string username=null, bool synchronous = false)
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

        public void UnlockAccount(string username=null)
        {            
            if (username == null)
            {
                username = UserName;
            }
            try
            {
                Console.Write("UnLocking the account: {0}", username);
                DirectoryEntry user = new DirectoryEntry("WinNT://" + Environment.MachineName + "/" + username);
                Console.Write(".");
                //Console.WriteLine(user.Properties["UserFlags"].Value);
                Console.Write(".");
                //Console.WriteLine(old);
                user.Properties["UserFlags"].Value = ADS_USER_FLAG.ADS_UF_NORMAL_ACCOUNT;
                Console.Write(".");
                //Console.WriteLine(user.Properties["UserFlags"].Value);
                user.CommitChanges();
                Console.WriteLine(".");
                user.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error with unlocking the account");
            }
            Console.WriteLine("Account has been unlocked.");
        }
        #endregion

        public IEnumerable<Account> GetLoggedInAccounts()
        {
            List<Account> list = new List<Account>();

            var sessions = Server.GetSessions();
            foreach (var session in sessions)
            {
                var account = new Account
                    {
                        CreatedAt = DateTime.Now,
                        Domain = session.DomainName,
                        Username = session.UserName,
                        UpdatedAt = DateTime.Now
                    };

                list.Add(account);
            }
            return list;
        }

        public IEnumerable<Account> GetDbAccounts()
        {
            return DbManager.GetAccounts();
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
    }
}