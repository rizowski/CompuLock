using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Timers;
using ActiveDs;
using Cassia;
using Service.Enviroment;
using Timer = System.Timers.Timer;

namespace Service.Users
{
    internal class UserManager
    {
        public string Domain;
        public DateTime StartTime;
        public TimeSpan ElapsedTime;
        public TimeSpan LockTime;
        public string UserName;
        public Timer Timer;

        public UserManager(string domain, string username, double interval = 1)
        {
            Domain = domain;
            UserName = username;
            Timer = new Timer(interval*1000) {AutoReset = true};
            //#if(debug)
            Timer.Disposed += TimerDisposed;
            Timer.Elapsed += TimerElapsed;
            //#endif
            Timer.Elapsed += Tick;
            ElapsedTime = TimeSpan.Zero;
            
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TimerDisposed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        public void ChangeUserPassword(string username, string oldpassword, string newpassword)
        {
            var pc = new PrincipalContext(ContextType.Machine);
            #if (debug)
            Console.WriteLine(pc.UserName);
            Console.WriteLine(pc.Name);
            Console.WriteLine(pc.ConnectedServer);
            Console.WriteLine(pc.ValidateCredentials(username, newpassword));
            #endif
            try
            {
                var context = new PrincipalContext(ContextType.Machine);
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.Name, username);
                //user.ChangePassword(oldpassword, newpassword);
                user.SetPassword(newpassword);

            }
            catch (PrincipalOperationException pe)
            {
                Console.WriteLine("The login credentials may be online");
            }
            catch (MultipleMatchesException me)
            {
                Console.WriteLine("More than one account has been found with the same username {0}", username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
            //lock an account
        }

        public List<Principal> GetUsers()
        {
            SecurityIdentifier builtinAdminSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);

            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);

            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, builtinAdminSid.Value);
            
            if (group != null)
                return new List<Principal>(group.Members);
            return null;
        }

        public int GetUserSessionId(string username=null)
        {
            if(username == null)
            {
                username = UserName;
            }
            ITerminalServicesManager manager = new TerminalServicesManager();
            using (ITerminalServer server = manager.GetLocalServer())
            {
                server.Open();
                foreach (var session in server.GetSessions())
                {
                    if (session.UserName.Equals(username))
                    {
                        return session.SessionId;
                    }
                }
                
            }
            return -1;
        }

        public void DisconnectUser(string username, int sessionId = -1)
        {
            if (username == null)
            {
                throw new ArgumentException("Username Cannot be null");
            }

        }

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);

        [DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);

        [DllImport("Kernal32.dll", EntryPoint = "WTSGetActiveConsoleSessionId")]
        public static extern int WTSGetActiveConsoleSessionId();

        public void UnlockAccount(string domain, string username)
        {
            var context = new PrincipalContext(ContextType.Machine);
            var computer = ComputerPrincipal.FindByIdentity(context, IdentityType.Name, username);
            computer.Enabled = true;
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            var inbetweentime = DateTime.Now - StartTime;
            inbetweentime = new TimeSpan(inbetweentime.Hours, inbetweentime.Minutes, inbetweentime.Seconds);
            ElapsedTime = inbetweentime;

            if (ElapsedTime.CompareTo(LockTime) >= 0)
            {
                Console.WriteLine("Lock the account and change the password.");
            }
            Console.WriteLine("Elapsed: {0}", ElapsedTime);
        }
    }
}