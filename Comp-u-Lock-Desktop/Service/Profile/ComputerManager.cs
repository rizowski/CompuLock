using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Timers;
using ActiveDs;
using Database;
using Database.Enviroment;
using Database.Models;

namespace Service.Profile
{
    public class ComputerManager
    {
        
        private DatabaseManager DbManager { get; set; }

        private Timer UpdateTimer;

        public ComputerManager(DatabaseManager dbManager)
        {
            DbManager = dbManager;
            SetupUpdateTimer(10800);
        }

        private void SetupUpdateTimer(double interval)
        {
            UpdateTimer = new Timer(interval * 1000){AutoReset = true};
            UpdateTimer.Elapsed += ForceUpdate;
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

        public void ForceUpdate()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            String MyIp = localIPs[0].ToString();
            var computer = new Computer
            {
                Name = Environment.MachineName,
                Enviroment = OS.StringName,
                IpAddress = MyIp,
                UpdatedAt = DateTime.Now
            };
            DbManager.SaveComputer(computer);
        }

        #region View
        public Computer GetComputer()
        {
            var computer = DbManager.GetComputer();
            if (computer == null)
            {
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                String MyIp = localIPs[0].ToString();
                return new Computer
                    {
                        CreatedAt = DateTime.Now,
                        Name = Environment.MachineName,
                        Enviroment = OS.StringName,
                        IpAddress = MyIp,
                        UpdatedAt = DateTime.Now
                    };
            }
            return computer;
        }

        public List<Account> GetAccounts()
        {
            SecurityIdentifier builtinAdminSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
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
            }
            return list;
        }
        #endregion

    }
}
