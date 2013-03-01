using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Database;
using REST;

namespace Service.Profile
{
    class RestManager : RestService
    {
        private Timer UpdateTimer { get; set; }
        private DatabaseManager DbManager { get; set; }

        public RestManager(string server, string apipath, DatabaseManager dbmanager) : base(server, apipath)
        {
            DbManager = dbmanager;
        }

        private void SetupUpdateTimer(double interval)
        {
            UpdateTimer = new Timer(interval){AutoReset = true};
            UpdateTimer.Elapsed += Update;
            UpdateTimer.Start();
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            
        }
    }
}
