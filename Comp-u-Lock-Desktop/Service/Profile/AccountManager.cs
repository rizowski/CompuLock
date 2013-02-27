using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
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

        public string Domain;
        public DateTime StartTime;
        public TimeSpan ElapsedTime;
        public TimeSpan LockTime;
        public string UserName;
        public Timer Timer;
        public ITerminalServer Server;

        public AccountManager(DatabaseManager dbmanager, double interval = 1)
        {
            DbManager = dbmanager;
            SetupTimer(interval);
            ITerminalServicesManager manager = new TerminalServicesManager();
            Server = manager.GetLocalServer();
            SystemEvents.SessionSwitch += Switch;
        }

        private void SetupTimer(double interval)
        {
            Timer = new Timer(interval * 1000) { AutoReset = true };
            Timer.Disposed += TimerDisposed;
            Timer.Elapsed += Tick;
            ElapsedTime = TimeSpan.Zero;
        }

        private void Switch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
        {
            //var send = (SystemEvents) sender;
            switch (sessionSwitchEventArgs.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                    break;
                case SessionSwitchReason.ConsoleDisconnect:
                    break;
                case SessionSwitchReason.RemoteConnect:
                    Check();
                    break;
                case SessionSwitchReason.RemoteDisconnect:
                    break;
                case SessionSwitchReason.SessionLogon:
                    Check();
                    break;
                case SessionSwitchReason.SessionLogoff:
                    break;
                case SessionSwitchReason.SessionLock:
                    break;
                case SessionSwitchReason.SessionUnlock:
                    Check();
                    break;
                case SessionSwitchReason.SessionRemoteControl:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        //TODO Check and subscribe to tracking event
        private void Check()
        {
            var username = Environment.UserName;
            Console.WriteLine("Looking for: {0}", username);
            var account = DbManager.GetAccountByName(username);
            if (account != null)
            {
                if (account.Tracking)
                {
                    if(account.AllottedTime == TimeSpan.FromSeconds(0))
                        LockAccount(account.Username);
                }
                else
                {
                    Console.WriteLine("Not Watching " + username);
                }
            }
            else
            {
                Console.WriteLine("No account found");
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

        public IEnumerable<Account> GetAccounts()
        {
            var accounts = DbManager.GetAccounts();
            if (accounts.Count() == 0)
            {
                SecurityIdentifier builtinAdminSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid,
                                                                            null);
                PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, builtinAdminSid.Value);
                List<Account> list = new List<Account>();
                foreach (var member in group.Members)
                {
                    list.Add(new Account
                        {
                            CreatedAt = DateTime.Now,
                            Domain = Environment.UserDomainName,
                            Username = member.Name,
                            Tracking = false,
                            UpdatedAt = DateTime.Now
                        });
                    return list;
                }
            }
            return accounts;
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