using System;
using System.DirectoryServices.AccountManagement;
using System.Timers;

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

        public UserAccount(string domain, string username, double interval = 1)
        {
            Domain = domain;
            UserName = username;
            Timer = new Timer(interval*1000) {AutoReset = true};
            #if(debug)
            Timer.Disposed += TimerDisposed;
            Timer.Elapsed += TimerElapsed;
            #endif
            Timer.Elapsed += Tick;
            ElapsedTime = TimeSpan.Zero;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            #if(debug)
            Console.WriteLine("{0} - {1}", sender, e.SignalTime);
            #endif
        }

        private void TimerDisposed(object sender, EventArgs e)
        {
            Console.WriteLine("Setting password to 190421");
            ChangeUserPassword(UserName, "", "190421");
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
                var context = new PrincipalContext(ContextType.Machine);
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.Name, username);
                
                var computer = ComputerPrincipal.FindByIdentity(context, IdentityType.UserPrincipalName, user.Name);
                Console.WriteLine(computer.DisplayName);
                Console.WriteLine(computer.Enabled);
                //computer.Enabled = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
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