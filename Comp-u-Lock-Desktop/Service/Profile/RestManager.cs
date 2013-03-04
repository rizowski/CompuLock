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
            SetupUpdateTimer(300);
            Update();
        }

        private void SetupUpdateTimer(double interval)
        {
            UpdateTimer = new Timer(interval * 1000){AutoReset = true};
            UpdateTimer.Elapsed += Update;
            UpdateTimer.Start();
        }

        // either push to the server and ignore or something like that idk
        private void Update(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var user = DbManager.GetFullUser();
            if (user.AuthToken != null)
            {
                UpdateUser(user.AuthToken, user);
            }
        }
        public void Update()
        {
            Update(null, null);
        }
    }
}
