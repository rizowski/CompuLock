using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Database.Models;
using Raven.Imports.Newtonsoft.Json;
using Raven.Imports.Newtonsoft.Json.Linq;
using RestSharp;

namespace REST
{
    public class RestService//: IApi
    {
        public string ApiPath { get; set; }
        public RestClient Client { get; set; }

        private const string AccountPath = "accounts/";
        private const string UserPath = "users/";
        protected const string AUTH = "auth_token";

        public RestService(string server, string apipath)
        {
            ApiPath = apipath;
        }

#region Accounts
        public Account CreateAccount(string token, Account item)
        {
            if (item.ComputerId == 0)
                throw new ArgumentException("Account must have a computer Id");
            var request = new RestRequest(ApiPath + AccountPath, Method.POST);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(item);
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            throw new NotImplementedException();
        }

        public Account UpdateAccount(string token, Account item)
        {
            var request = new RestRequest(ApiPath + AccountPath + item.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(item);
            Console.WriteLine(json);
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAllAccounts(string token)
        {
            var request = new RestRequest(ApiPath + AccountPath, Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var account = JObject.Parse(response.Content);
            var accounts = JsonConvert.DeserializeObject<List<Account>>(account["accounts"].ToString());
            return accounts;
        }

        public Account GetAccountById(string token, int id)
        {
            var accounts = GetAllAccounts(token);
            return accounts.FirstOrDefault(a => a.Id == id);
        }

        public void DestroyAccount(string token, int id)
        {
            var request = new RestRequest(ApiPath + AccountPath + id, Method.DELETE);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
#endregion
#region User
        public User UpdateUser(string token, User item)
        {
            var request = new RestRequest(ApiPath + UserPath + item.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = item.ToJSON();
            request.AddParameter("user", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var userjson = JObject.Parse(response.Content);
            User user = null;
            try
            {
                user = JsonConvert.DeserializeObject<User>(userjson["user"].ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return user;
        }

        public User GetUser(string token)
        {
            var request = new RestRequest(ApiPath + UserPath, Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var userjson = JObject.Parse(response.Content);
            User user = null;
            try
            {
                user = JsonConvert.DeserializeObject<User>(userjson["user"].ToString());
                user.AuthToken = token;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return user;
        }
#endregion

#region Computers
        public Computer CreateComputer(string token, Computer item)
        {
            if (item.UserId == 0)
                throw new ArgumentException("Computer must have a user Id");
            var request = new RestRequest(ApiPath+"computers/", Method.POST);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(item);
            request.AddParameter("computer", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var compjson = JObject.Parse(response.Content);
            var comp = JsonConvert.DeserializeObject<Computer>(compjson["computer"].ToString());
            return comp;
        }

        public Computer UpdateComputer(string token, Computer item)
        {
            var request = new RestRequest(ApiPath+"computers/" + item.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(item);
            request.AddParameter("computer", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var compjson = JObject.Parse(response.Content);
            var comp = JsonConvert.DeserializeObject<Computer>(compjson["computer"].ToString());
            return comp;
        }

        public IEnumerable<Computer> GetAllComputers(string token)
        {
            var request = new RestRequest(ApiPath+"computers", Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var comp = JObject.Parse(response.Content);
            var comps = JsonConvert.DeserializeObject<List<Computer>>(comp["computers"].ToString());
            return comps;
        }

        public Computer GetComputerById(string token, int id)
        {
            var computers = GetAllComputers(token);
            return computers.FirstOrDefault(c => c.Id == id);
        }

        public void DestroyComputer(string token, int id)
        {
            var request = new RestRequest(ApiPath+"computers/" + id, Method.DELETE);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
#endregion

#region Processes

#endregion
    }
}
