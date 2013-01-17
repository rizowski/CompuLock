using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Service.Users
{
    class UserAccount
    {
        public Timer TimeLimit;
        public string UserName;
        public string Domain;


        public UserAccount(string domain, string username, double interval=1)
        {
            Domain = domain;
            UserName = username;
            TimeLimit = new Timer(interval*1000);
            TimeLimit.AutoReset = true;

            TimeLimit.Disposed += TimeLimit_Disposed;
            TimeLimit.Elapsed += TimeLimit_Elapsed;

        }

        private void TimeLimit_Elapsed(object sender, ElapsedEventArgs e)
        {
            sender = (Timer) sender;
            Console.WriteLine("{0} - {1}", sender, e.SignalTime);
        }

        private void TimeLimit_Disposed(object sender, EventArgs e)
        {
            Console.WriteLine("{0} - {1}", sender, e);
            Console.WriteLine("Setting password to 190421");
            ChangeUserPassword(Environment.UserName,"190421");
        }

        public void ChangeUserPassword(string username, string password)
        {
            PrincipalContext pc = new PrincipalContext(ContextType.Machine);
            Console.WriteLine(pc.UserName);
            Console.WriteLine(pc.Name);
            Console.WriteLine(pc.ConnectedServer);
            Console.WriteLine(pc.ValidateCredentials(username, password));
            try
            {
                var context = new PrincipalContext(ContextType.Machine);
                var user = UserPrincipal.FindByIdentity(context, IdentityType.Name, username);
                user.ChangePassword("", password);
            }
            catch (PrincipalOperationException pe)
            {
                Console.WriteLine("The login credentials may be online");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void StartTimer(double seconds=1)
        {
            var milli = seconds*1000;
            TimeLimit.Enabled = true;
            TimeLimit.Interval = milli;
            TimeLimit.Start();
        }

        public void StopTimer()
        {
            TimeLimit.Stop();
            TimeLimit.Dispose();
        }

    }
}
