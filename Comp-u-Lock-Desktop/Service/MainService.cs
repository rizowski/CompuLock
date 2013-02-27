using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
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

        private const string RestServer = "http://192.168.144.1:3000";
        private const string Api = "api/v1/";

        public MainService()
        {
            DbManager = new DatabaseManager("settings", "myPass");
            RestService = new RestService(RestServer, Api);
            ComputerManager = new ComputerManager();
            AccountManager = new AccountManager(DbManager);
            ProcessManager = new ProcessManager(DbManager);
            BrowserManager = new InternetExplorerHistoryReader();
        }

        #region Rest
        #region Get
        public User GetRestUser(string token)
        {
            var restUser = RestService.GetUser(token);
            if (restUser == null)
            {
                Logger.Write("User not found", Status.Warning);
                throw new NullReferenceException("User not found");
            }
            return DbManager.SaveUser(restUser);
        }
        public IEnumerable<Account> GetRestAccounts(string token)
        {
            return RestService.GetAllAccounts(token);
        }
        #endregion
        #region Save
        public void SendUser(User user)
        {
            RestService.UpdateUser(user.AuthToken, user);
        }
        #endregion
        #endregion

        #region Database
        #region Get
        public User GetDbUser()
        {
            var user = DbManager.GetUser();
            if (user != null)
                return user;
            Logger.Write("No user Stored", Status.Warning);
            throw new NullReferenceException("No User Stored");
        }
        public IEnumerable<Account> GetDbAccounts()
        {
            return DbManager.GetAccounts();
        }
        public Computer GetDbComputer()
        {
            return DbManager.GetComputer();
        }
        #endregion
        #region Save
        public void SaveAccountsToDb(IEnumerable<Account> accounts)
        {
            foreach (var account in accounts)
            {
                DbManager.SaveAccount(account);
            }
        }
        public void SaveUserToDb(string token)
        {
            DbManager.SaveUser(new User
            {
                AuthToken = token
            });
        }
        public void SaveHistoryToDb()
        {
            var histories = BrowserManager.GetHistory();
            var computer = DbManager.GetComputer();
            if (computer != null)
                foreach (var history in histories)
                {
                    history.ComputerId = computer.Id;
                    DbManager.SaveHistory(history);
                }
        }
        #endregion
        #endregion

        #region Computer
        public Computer GetComputer()
        {
            return ComputerManager.GetComputer();
        }
        public IEnumerable<Account> GetAccounts()
        {
            return ComputerManager.GetAccounts();
        }
        #endregion

        #region private
        
        #endregion

        public IEnumerable<History> GetHistory()
        {
            return DbManager.GetHistories();
        }
    }
}
