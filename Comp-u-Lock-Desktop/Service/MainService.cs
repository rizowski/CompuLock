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
            
        }
    }
}
