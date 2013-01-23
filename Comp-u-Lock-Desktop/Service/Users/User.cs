using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Management;
using System.Runtime.InteropServices;
using System.Timers;
using ActiveDs;
using Service.Enviroment;

namespace Service.Users
{
    internal class UserAccount
    {
        public string Domain;
        public DateTime StartTime;
        public TimeSpan ElapsedTime;
        public TimeSpan LockTime;
        public string UserName;
        public Timer Timer;

        const int WTS_CURRENT_SESSION = -1;
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        public UserAccount(string domain, string username, double interval = 1)
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
            try
            {
                //var context = new PrincipalContext(ContextType.Machine);
                //UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.Name, username);
                
                //var computer = ComputerPrincipal.FindByIdentity(context, IdentityType.UserPrincipalName, user.Name);
                //Console.WriteLine(computer.DisplayName);
                //Console.WriteLine(computer.Enabled);
                //computer.Enabled = false;

                //DirectoryEntry user = new DirectoryEntry("WinNT://"+Environment.MachineName+"/"+username);
                //Console.WriteLine(user.Properties["UserFlags"].Value);
                //user.Properties["UserFlags"].Value = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;//ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;//ADS_USER_FLAG.ADS_UF_LOCKOUT;
                //Console.WriteLine(user.Properties["UserFlags"].Value);
                //user.CommitChanges();

                //ExitWindowsEx(4, 0);

                //http://msdn.microsoft.com/en-us/library/system.management.managementobject.aspx

                //http://stackoverflow.com/questions/484278/log-off-user-from-win-xp-programmatically-in-c-sharp/484303#484303
                //http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/59dd345d-df85-4128-8641-f01f01583194
                //http://msdn.microsoft.com/en-us/library/windows/desktop/aa393964(v=vs.85).aspx#obtaining_data_from_WMI
                //http://msdn.microsoft.com/en-us/library/windows/desktop/aa394572(v=vs.85).aspx
                //ConnectionOptions connOptions = new ConnectionOptions();
                //connOptions.Impersonation = ImpersonationLevel.Impersonate;
                //connOptions.EnablePrivileges = true;
                //ManagementScope manScope = new ManagementScope(String.Format(@"\\{0}\ROOT\CIMV2", Environment.MachineName), connOptions);
                //manScope.Connect();
                //ObjectQuery oQuery = new ObjectQuery("select * from Win32_OperatingSystem");
                //ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(manScope, oQuery);
                //ManagementObjectCollection oReturn = oSearcher.Get();
                //foreach (ManagementObject mo in oReturn)
                //{
                //    ManagementBaseObject inParams = mo.GetMethodParameters("Win32Shutdown");
                //    //inParams["Flags"] = 4;
                //    //mo.InvokeMethod("Name",);
                //    ManagementBaseObject outParams = mo.InvokeMethod("Win32Shutdown", inParams, null);
                //}

                if (!WTSDisconnectSession(WTS_CURRENT_SERVER_HANDLE,
                 WTS_CURRENT_SESSION, false))
                    throw new Win32Exception();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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