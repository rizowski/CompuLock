using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Timers;
using ActiveDs;
using Cassia;

namespace Service.Profile
{
    public class AccountManager
    {
        public string Domain;
        public DateTime StartTime;
        public TimeSpan ElapsedTime;
        public TimeSpan LockTime;
        public string UserName;
        public Timer Timer;
        public ITerminalServer Server;

        public AccountManager(double interval = 1)
        {
            Timer = new Timer(interval*1000) {AutoReset = true};
            Timer.Disposed += TimerDisposed;
            Timer.Elapsed += Tick;
            ElapsedTime = TimeSpan.Zero;
            ITerminalServicesManager manager = new TerminalServicesManager();
            Server = manager.GetLocalServer();
        }

        private void TimerDisposed(object sender, EventArgs e)
        {
            DisconnectUser(UserName, true);
            LockAccount(UserName);
        }

        public void StartTimer(double interval = 1)
        {
            double milli = interval*1000;
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

    }
}