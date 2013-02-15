using System;
using Database;
using Database.Models;
using Service.Db;
using Service.Profile;

namespace Service
{
   
    public class MainService
    {
        public DatabaseClient DbClient { get; private set; }
        public UserManager UserManager { get; private set; }

        public MainService()
        {
        }
        
        public void testing()
        {
            
        }
    }
}
