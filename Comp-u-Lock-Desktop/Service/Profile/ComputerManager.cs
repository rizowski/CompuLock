using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Timers;
using Database;
using Database.Enviroment;
using Database.Models;

namespace Service.Profile
{
    public class ComputerManager
    {
        public bool Watching { get; set; }
        private DatabaseManager DbManager { get; set; }
        private Timer UpdateTimer;

        public ComputerManager()//DatabaseManager dbManager)
        {
            DbManager = new DatabaseManager("settings","");
            SetupUpdateTimer(10800);
        }

        private void SetupUpdateTimer(double interval)
        {
            Console.WriteLine("Setting up Computer Manager Update Timer");
            UpdateTimer = new Timer(interval * 1000){AutoReset = true};
            UpdateTimer.Elapsed += ForceUpdate;
            UpdateTimer.Start();
            Watching = true;
            ForceUpdate();
        }

        private void ForceUpdate(object sender, EventArgs eventArgs)
        {
            var dbcomputer = DbManager.GetComputer();
            if (dbcomputer == null)
            {
                ForceUpdate();
            }
            else
            {
                var start = dbcomputer.UpdatedAt;
                var count = DateTime.Now.Subtract(start).TotalDays;
                if(count >= 7)
                    ForceUpdate();
            }
        }
        public void StopUpdating()
        {
            UpdateTimer.Dispose();
            Watching = false;
        }

        public Computer ForceUpdate()
        {
            var dbcomputer = DbManager.GetComputer();
            
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            String MyIp = localIPs[0].ToString();
            if (dbcomputer == null)
            {
                var computer = new Computer
                    {
                        CreatedAt = DateTime.Now,
                        Name = Environment.MachineName,
                        Enviroment = OS.StringName,
                        IpAddress = MyIp,
                        UpdatedAt = DateTime.Now
                    };
                return DbManager.SaveComputer(computer);
            }
            return null;
        }

        #region View
        public Computer GetComputer()
        {
            var computer = DbManager.GetComputer();
            if (computer == null)
            {
                return ForceUpdate();
            }
            return computer;
        }

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
        #endregion

    }
}
