using System;
using System.DirectoryServices.AccountManagement;
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
        public DatabaseManager DbClient { get; private set; }
        public AccountManager AccountManager { get; private set; }
        public ComputerManager ComputerManager { get; set; }
        public ProcessManager ProcessManager { get; set; }
        public InternetExplorerHistoryReader BrowserManager { get; set; }
        public RestService RestS { get; set; }

        private const string RestServer = "http://localhost:3000";
        private const string Api = "api/v1/";

        public MainService()
        {
            DbClient = new DatabaseManager("settings", "myPass");
            RestS = new RestService(RestServer, Api);
            ComputerManager = new ComputerManager();
            var computer = DbClient.SaveComputer(ComputerManager.GetComputer());
            DbClient.SaveAccounts(computer.Id, ComputerManager.GetAccounts());
            AccountManager = new AccountManager();
            ProcessManager = new ProcessManager();
            BrowserManager = new InternetExplorerHistoryReader();
            SystemEvents.SessionSwitch += Switch;

        }


        public void Switch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
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
            var restUser = RestS.GetUser(token);
            if (restUser == null)
                throw new NullReferenceException("User not found");
            return DbClient.SaveUser(restUser);
        }
        public IEnumerable<Account> GetRestAccounts(string token)
        {
            return RestS.GetAllAccounts(token);
        }

        public void SendUser(User user)
        {
            RestS.UpdateUser(user.AuthToken, user);
        }
        #endregion

        #region Database
        public User GetDbUser()
        {
            var user = DbClient.GetUser();
            if (user != null)
                return user;
            throw new NullReferenceException("No Users Stored");
        }
        public void SaveDbUser(string token)
        {
            DbClient.SaveUser(new User
                {
                    AuthToken = token
                });
        }

        public IEnumerable<Account> GetDbAccounts()
        {
            return DbClient.GetAccounts();
        }
        public Computer GetDbComputer()
        {
            return DbClient.GetComputer();
        }

        public void SaveDbAccounts()
        {
            var accounts = ComputerManager.GetAccounts();
            foreach (var account in accounts)
            {
                DbClient.SaveAccount(account);
            }
        }
        #endregion

        #region Computer
        public void SaveHistory()
        {
            var histories = BrowserManager.GetHistory();
            var account = DbClient.GetTrackingAccounts();
            if(account != null)
                foreach (var history in histories)
                {
                    var id = account.Id;
                    history.AccountId = id;
                    DbClient.SaveHistory(history);
                }
        }
        #endregion

        #region private
        
        #endregion


        //Methods that use the db and the rest service to save objects at the same time.
    }
}
