using System;
using Database;
using Database.Models;
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
        public REST.RestService RestS { get; set; }

        private const string RestServer = "http://localhost:3000";
        private const string Api = "api/v1/";

        public MainService()
        {
            DbClient = new DatabaseManager("settings", "myPass");
            RestS = new RestService(RestServer, Api);
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


        //Methods that use the db and the rest service to save objects at the same time.
    }
}
