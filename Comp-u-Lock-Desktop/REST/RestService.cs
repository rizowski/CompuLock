using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Database.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;

namespace REST
{
    public class RestService//: IApi
    {
        public string ApiPath = "";
        public RestClient Client;
        protected const string AUTH = "auth_token";

        public RestService(string server, string apipath)
        {
            Client = new RestClient(server);
            ApiPath = apipath;
        }


        [Obsolete("This method should not be used. User should specify token for method use.", true)]
        public string GetToken(string username, string password)
        {
            var request = new RestRequest("api/v1/tokens", Method.POST);
            request.AddParameter("email", username);
            request.AddParameter("password", password);
            var response = Client.Execute(request);
            return response.Content;
        }

        public User GetUser(string token)
        {
            var request = new RestRequest("api/v1/users", Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var userjson = JObject.Parse(response.Content);
            var user = JsonConvert.DeserializeObject<User>(userjson["user"].ToString());
            return user;
        }

        public IEnumerable<Computer> GetComputers(string token)
        {
            var request = new RestRequest("api/v1/computers", Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var comp = JObject.Parse(response.Content);
            var comps = JsonConvert.DeserializeObject<List<Computer>>(comp["computers"].ToString());
            // http://james.newtonking.com/projects/json/help/
            return comps;
        }

        public IEnumerable<Account> GetAccounts(string token)
        {
            var request = new RestRequest("api/v1/accounts", Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var account = JObject.Parse(response.Content);
            var accounts = JsonConvert.DeserializeObject<List<Account>>(account["accounts"].ToString());
            return accounts;
        }

        public Account GetAccountById(string token, int accountId)
        {
            var accounts = GetAccounts(token);
            return accounts.FirstOrDefault(a => a.Id == accountId);
        }

        public Computer GetComputerById(string token, int computerId)
        {
            var computers = GetComputers(token);
            return computers.FirstOrDefault(c => c.Id == computerId);
        }

        public IEnumerable<History> GetHistory(string token, int accountId)
        {
            var account = GetAccountById(token, accountId);
            return account.Histories;
        }

        public IEnumerable<Process> GetProcesses(string token, int accountId)
        {
            var account = GetAccountById(token, accountId);
            return account.Processes;
        }

        public IEnumerable<Program> GetPrograms(string token, int accountId)
        {
            var account = GetAccountById(token, accountId);
            return account.Programs;
        }

        public void UpdateUser(string token, User user)
        {
            var request = new RestRequest("api/v1/users/" + user.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(user);
            Console.WriteLine(json);
            request.AddParameter("user", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }

        public void UpdateComputer(string token, Computer computer)
        {
            var request = new RestRequest("api/v1/computers/" + computer.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(computer);
            Console.WriteLine(json);
            request.AddParameter("computer", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }

        public void UpdateAcount(string token, Account account)
        {
            var request = new RestRequest("api/v1/accounts/" + account.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(account);
            Console.WriteLine(json);
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }

        public void CreateComputer(string token, Computer computer)
        {
            if (computer.UserId == 0)
                throw new ArgumentException("Computer must have a user Id");
            var request = new RestRequest("api/v1/computers/", Method.POST);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(computer);
            request.AddParameter("computer", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }

        public void CreateAccount(string token, Account account)
        {
            if (account.ComputerId == 0)
                throw new ArgumentException("Account must have a computer Id");
            var request = new RestRequest("api/v1/accounts", Method.POST);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(account);
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }

        public void CreateHistory(string token, int accountId, History history)
        {
            throw new NotImplementedException();
        }

        public void CreateProgram(string token, int accountId, Program program)
        {
            throw new NotImplementedException();
        }

        public void CreateProcess(string token, int accountId, Process process)
        {
            throw new NotImplementedException();
        }

        public void DeleteComputer(string token, int id)
        {
            var request = new RestRequest("api/v1/computers/" + id, Method.DELETE);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }

        public void DeleteAccount(string token, int id)
        {
            var request = new RestRequest("api/v1/accounts/" + id, Method.DELETE);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
