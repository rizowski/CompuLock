using System;
using Database;
using Database.Models;
using Service.Db;
using Service.Profile;

namespace Service
{
   
    public class MainService
    {
        public DatabaseServer DbServer { get; private set; }
        public DatabaseClient DbClient { get; private set; }
        public UserManager UserManager { get; private set; }

        public MainService()
        {
            //DbServer = new DatabaseServer("Data", true);
            //DbServer.StartServer();
            DbClient = new DatabaseClient("http://localhost:8080");
        }
        public void testing()
        {
            var users = ComputerManager.GetUsers();
            var computer = ComputerManager.GetComputer();
            computer = DbClient.Save(computer);
            foreach (var principal in users)
            {
                Console.WriteLine(principal.DisplayName);
                Console.WriteLine(principal.Name);
                Console.WriteLine(principal.DistinguishedName);
                Console.WriteLine(principal.UserPrincipalName);
                var account = new Account
                    {
                        AllottedTime = 0, 
                        Domain = principal.Context.Name,
                        ComputerId = computer.Id, 
                        Tracking = false, 
                        Username = principal.Context.UserName, 
                        CreatedAt = DateTime.Now, 
                        UpdatedAt = DateTime.Now
                    };
                DbClient.Save(account);
            }
        }
    }
}
