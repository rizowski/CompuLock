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
            UpdateTimer.Elapsed += UpdateAccounts;
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
            var dbAccounts = DbManager.GetAccounts();

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
                foreach (var dbAccount in dbAccounts.Where(dbAccount => dbAccount.ComputerId == 0))
                {
                    dbAccount.ComputerId = dbcomp.WebId;
                    DbManager.UpdateAccount(dbAccount);
                }
                
                foreach (var history in dbHistories.Where(history => history.ComputerId == 0))
                {
                    history.ComputerId = dbcomp.WebId;
                    DbManager.UpdateHistory(history);
                }

                dbcomp.Histories = (List<History>) dbHistories;
                var restcomp = UpdateComputer(dbUser.AuthToken, dbcomp);
                if (restcomp == null) return;
                foreach (var history in restcomp.Histories)
                {
                    var foundhistory = dbHistories.FirstOrDefault(h => h.Title == history.Title && h.Url == history.Url);
                    if (foundhistory == null)
                    {
                        if (history.Title.Length!=0)
                        {
                            DbManager.SaveHistory(history);
                        }
                    }
                    else
                    {
                        history.Id = foundhistory.Id;
                        DbManager.UpdateHistory(history);
                    } 
                }
                
                
                
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
            var dbUser = DbManager.GetUser();
            var dbcomp = DbManager.GetComputer();
            var dbAccounts = DbManager.GetAccounts();

            if (dbUser == null) return;
            if (dbUser.AuthToken.Length == 0) return;
            if (dbUser.WebId == 0) return;

            if (dbcomp == null) return;
            if (dbcomp.WebId == 0) return;

            foreach (var account in dbAccounts)
            {
                var stuff = GetAllAccounts(dbUser.AuthToken);
                var foundAccount = stuff.FirstOrDefault(a => a.ComputerId == account.ComputerId && a.Domain == account.Domain && a.Username == account.Username);
                // account model needs to be updated with a ComputerId
                if (foundAccount == null)
                {
                    account.ComputerId = dbcomp.WebId;
                    SaveAccount(dbUser.AuthToken, account);
                }
                else
                {

                    //NEED to change time on rails db to int so I can save as seconds
                    // change form on rails application
                    // Check the date for updated at to see why it isn't being sent from the server.
                    // Add processes to this list after all this.
                    if (foundAccount.UpdatedAt > account.UpdatedAt || foundAccount.CreatedAt > account.CreatedAt)
                    {
                        foundAccount.Id = account.Id;
                        DbManager.UpdateAccount(foundAccount);
                    }
                    else
                    {
                        //sends to server
                        account.WebId = foundAccount.WebId;
                        UpdateAccount(dbUser.AuthToken, account);
                    }
                    //Proceses
                }
            }
        }
    }
}