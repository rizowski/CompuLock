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

namespace Service
{
   
    public class MainService
    {
        public DatabaseServer DbServer { get; private set; }
        public DatabaseClient DbClient { get; private set; }

        public MainService()
        {
            DbServer = new DatabaseServer("Data", true);
            DbServer.StartServer();
            DbClient = new DatabaseClient("localhost:8080");

            MainWindow mw = new MainWindow();
            mw.Activate();

        }
    }
}
