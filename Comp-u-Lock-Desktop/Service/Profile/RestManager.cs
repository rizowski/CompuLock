using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Database;
using Database.Models;
using REST;

namespace Service.Profile
{
    public class RestManager : RestService
    {
        private Timer UpdateTimer { get; set; }
        private DatabaseManager DbManager { get; set; }

        public RestManager(string server, string apipath)
            : base(server, apipath)
        {
            DbManager = new DatabaseManager("settings", "");
            SetupUpdateTimer(30);
            //Update(null, null);
        }

        private void SetupUpdateTimer(double interval)
        {
            Console.WriteLine("Setting up Rest Manager Update Timer");
            UpdateTimer = new Timer(interval*1000) {AutoReset = true};
            UpdateTimer.Elapsed += UpdateUser;
            UpdateTimer.Elapsed += UpdateComputer;
            UpdateTimer.Start();
        }

        // either push to the server and ignore or something like that idk
        private void Update(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var dbuser = DbManager.GetUser();
            Console.WriteLine("Updating Rest User");
            if (dbuser == null) return;
            if (dbuser.AuthToken.Length == 0) return;
            User restUser = null;
            try
            {
                restUser = GetUser(dbuser.AuthToken);
                var user = DbManager.UpdateUser(restUser);
                var computer = DbManager.GetComputer();
                var accounts = DbManager.GetAccounts();

                if (restUser.Computers.FirstOrDefault(c => c.Enviroment == computer.Enviroment && c.Name == computer.Name) == null)
                {
                    computer.UserId = restUser.WebId;
                    var restcomputer = CreateComputer(user.AuthToken, computer);
                    
                    foreach (var account in accounts)
                    {
                        account.ComputerId = computer.WebId;
                        DbManager.UpdateAccount(account);
                    }
                    DbManager.UpdateComputer(restcomputer);
                }
                else
                {
                    computer.Accounts = (List<Account>) accounts;
                    UpdateComputer(user.AuthToken, computer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void UpdateUser(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Updating Rest User");
            var dbuser = DbManager.GetUser();

            if (dbuser == null) return;
            if (dbuser.AuthToken.Length == 0) return;
            if (dbuser.WebId == 0)
            {
                var restUser = GetUser(dbuser.AuthToken);
                DbManager.UpdateUser(restUser);
            }
        }

        private void UpdateComputer(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Updating Rest Computer");
            var dbUser = DbManager.GetUser();
            var dbcomp = DbManager.GetComputer();
            var dbHistories = DbManager.GetHistories();

            if (dbUser == null) return;
            if (dbUser.AuthToken.Length == 0) return;
            if (dbUser.WebId == 0) return;

            if (dbcomp == null) return;
            if (dbcomp.UserId == 0)
            {
                dbcomp.UserId = dbUser.WebId;
                dbcomp = DbManager.UpdateComputer(dbcomp);
            }
            var restcomps = GetAllComputers(dbUser.AuthToken);
            if (restcomps.FirstOrDefault(c => c.Enviroment == dbcomp.Enviroment && c.Name == dbcomp.Name) == null)
            {
                var newcomp = CreateComputer(dbUser.AuthToken, dbcomp);
                DbManager.UpdateComputer(newcomp);
            }
            else
            {
                foreach (var history in dbHistories)
                {
                    if (history.ComputerId == 0)
                    {
                        history.ComputerId = dbcomp.WebId;
                        var dbhistory = DbManager.UpdateHistory(history);
                    }
                }
                dbcomp.Histories = (List<History>) dbHistories;
                UpdateComputer(dbUser.AuthToken, dbcomp);
            }
        }

        //Notes:
        // You may want to see if you can update accounts when you send a computer, if not write it up so that you can send accounts with processes.
        // You also need to remember about histories
        // Parsing seems to be the problem. Also SQLite connection errors don't seem to be spawning. it amy be due to the fact of time.
        // Stay up again, we need to get this done.
        // keep it up.
        private void UpdateAccounts(object sender, ElapsedEventArgs e)
        {
            
        }
    }
}