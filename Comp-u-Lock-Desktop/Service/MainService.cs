using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using CompuLockDesktop;
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
            DbServer = new DatabaseServer("Data", true);
            DbServer.StartServer();
            DbClient = new DatabaseClient("localhost:8080");
        }
        public void testing()
        {
            var users = ComputerManager.GetUsers();
            var computer = ComputerManager.GetComputer();
            computer = DbClient.Save(computer);
            foreach (var principal in users)
            {
                var account = new Account
                    {
                        AllottedTime = 0, 
                        ComputerId = computer.Id, 
                        Tracking = false, 
                        Username = principal.Name, 
                        CreatedAt = DateTime.Now, 
                        UpdatedAt = DateTime.Now
                    };
                DbClient.Save(account);
            }
        }
    }
}
