using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using Database;
using Database.Models;
using Microsoft.Win32;
using REST;
using Service.Profile;
using System.Collections.Generic;

namespace Service
{
   
    public class MainService
    {
        public DatabaseManager DbManager { get; private set; }
        public AccountManager AccountManager { get; private set; }
        public ComputerManager ComputerManager { get; set; }
        public ProcessManager ProcessManager { get; set; }
        public InternetExplorerHistoryReader BrowserManager { get; set; }
        public RestService RestService { get; set; }

        private const string RestServer = "http://localhost:3000";
        private const string Api = "api/v1/";

        public MainService()
        {
            if(File.Exists("settings.sqlite"))
                File.Delete("settings.sqlite");
            DbManager = new DatabaseManager("settings", "myPass");
            RestService = new RestService(RestServer, Api);
            ComputerManager = new ComputerManager();
            var computer = DbManager.SaveComputer(ComputerManager.GetComputer());
            DbManager.SaveAccounts(computer.Id, ComputerManager.GetAccounts());
            AccountManager = new AccountManager();
            ProcessManager = new ProcessManager();
            BrowserManager = new InternetExplorerHistoryReader();
            DbManager.SaveHistories(computer.Id, BrowserManager.GetHistory());
            SystemEvents.SessionSwitch += Switch;
        }


        private void Switch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
        {
            switch (sessionSwitchEventArgs.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                    break;
                case SessionSwitchReason.ConsoleDisconnect:
                    break;
                case SessionSwitchReason.RemoteConnect:
                    Check();
                    break;
                case SessionSwitchReason.RemoteDisconnect:
                    break;
                case SessionSwitchReason.SessionLogon:
                    Check();
                    break;
                case SessionSwitchReason.SessionLogoff:
                    break;
                case SessionSwitchReason.SessionLock:
                    break;
                case SessionSwitchReason.SessionUnlock:
                    Check();
                    break;
                case SessionSwitchReason.SessionRemoteControl:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //TODO Check and subscribe to tracking event
        private void Check()
        {
            var username = Environment.UserName;
            Console.WriteLine("Looking for: {0}", username);
            var accounts = ComputerManager.GetAccounts();
            var account = accounts.FirstOrDefault(a => a.Username == username);
            if (account != null)
            {
                if (account.Tracking)
                {
                    Logger.Write("I am watching you " + username);
                }
                else
                {
                    Logger.Write("I am not watching you " + username);
                }
            }
            else
            {
                Console.WriteLine("No account found");
            }
        }
        #region Rest
        public User GetRestUser(string token)
        {
            var restUser = RestService.GetUser(token);
            if (restUser == null)
                throw new NullReferenceException("User not found");
            return DbManager.SaveUser(restUser);
        }
        public IEnumerable<Account> GetRestAccounts(string token)
        {
            return RestService.GetAllAccounts(token);
        }

        public void SendUser(User user)
        {
            RestService.UpdateUser(user.AuthToken, user);
        }
        #endregion

        #region Database
        public User GetDbUser()
        {
            var user = DbManager.GetUser();
            if (user != null)
                return user;
            throw new NullReferenceException("No Users Stored");
        }
        public void SaveDbUser(string token)
        {
            DbManager.SaveUser(new User
                {
                    AuthToken = token
                });
        }

        public IEnumerable<Account> GetDbAccounts()
        {
            return DbManager.GetAccounts();
        }
        public Computer GetDbComputer()
        {
            return DbManager.GetComputer();
        }

        public void SaveDbAccounts()
        {
            var accounts = ComputerManager.GetAccounts();
            foreach (var account in accounts)
            {
                DbManager.SaveAccount(account);
            }
        }
        #endregion

        #region Computer
        public void SaveHistory()
        {
            var histories = BrowserManager.GetHistory();
            var computer = DbManager.GetComputer();
            if(computer != null)
                foreach (var history in histories)
                {
                    history.ComputerId = computer.Id;
                    DbManager.SaveHistory(history);
                }
        }
        #endregion

        #region private
        
        #endregion


        //Methods that use the db and the rest service to save objects at the same time.
    }
}
