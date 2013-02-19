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
        public AccountManager AccountManager { get; private set; }
        

        public MainService()
        {
            DbClient = new DatabaseClient("settings", "myPass");

        }
        
        public void testing()
        {
            
        }
        //Methods that use the db and the rest service to save objects at the same time.
    }
}
