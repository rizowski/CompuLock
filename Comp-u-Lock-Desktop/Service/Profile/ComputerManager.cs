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
using ActiveDs;
using Database.Enviroment;
using Database.Models;

namespace Service.Profile
{
    public class ComputerManager
    {
        private const string WinNT = "WinNT://";
        private const string UserFlags = "UserFlags";
        public ComputerManager()
        {
            GetComputer();
            GetAccounts();
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

        public Computer GetComputer()
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
    }
}
