using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Database.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace REST
{
    public class RestService
    {
        public string Server { get; set; }
        public string ApiPath { get; set; }
        public RestClient Client { get; set; }

        private const string AccountPath = "accounts/";
        private const string UserPath = "users/";
        private const string ComputerPath = "computers/";
        private const string ProgramPath = "programs/";
        private const string ProcessPath = "processes";
        private const string HistoriesPath = "histories/";

        private const string ComputerKey = "computer";
        private const string ProgramKey = "program";
        private const string ProcessKey = "process";

        protected const string Auth = "auth_token";

        public RestService(string server, string apipath)
        {
            Client = new RestClient(server);
            Server = server;
            ApiPath = apipath;
        }

#region Accounts
        public Account SaveAccount(string token, Account item)
        {
            if (item.ComputerId == 0)
                throw new ArgumentException("Account must have a computer Id");
            var request = new RestRequest(ApiPath + AccountPath, Method.POST);
            request.AddParameter(Auth, token);
            var json = item.ToJSON();
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var accountJson = JObject.Parse(response.Content);
            //TRY Parse into ERROR
            var account = JsonConvert.DeserializeObject<Account>(accountJson["account"].ToString());
            return account;
        }

        public Account UpdateAccount(string token, Account item)
        {
            var request = new RestRequest(ApiPath + AccountPath + item.WebId, Method.PUT);
            request.AddParameter(Auth, token);
            var json = item.ToJSON();
            Console.WriteLine(json);
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var accountJson = JObject.Parse(response.Content);
            //TRY Parse into ERROR
            var account = JsonConvert.DeserializeObject<Account>(accountJson["account"].ToString());
            return account;
        }

        public IEnumerable<Account> GetAllAccounts(string token)
        {
            var request = new RestRequest(ApiPath + AccountPath, Method.GET);
            request.AddParameter(Auth, token);
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
            return accounts.FirstOrDefault(a => a.WebId == id);
        }

        public IEnumerable<Program> GetAllProgramsByAccount(string token, int id)
        {
            var account = GetAccountById(token, id);
            return account.Programs;
        }

        public void DestroyAccount(string token, int id)
        {
            var request = new RestRequest(AccountPath + id, Method.DELETE);
            request.AddParameter(Auth, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
#endregion
#region User
        public User UpdateUser(string token, User item)
        {
            var request = new RestRequest(ApiPath + UserPath + item.WebId, Method.PUT);
            request.AddParameter(Auth, token);
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
            request.AddParameter(Auth, token);
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
            var request = new RestRequest(ApiPath + ComputerPath, Method.POST);
            request.AddParameter(Auth, token);
            var json = item.ToJSON();
            request.AddParameter("computer", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var compjson = JObject.Parse(response.Content);
            var comp = JsonConvert.DeserializeObject<Computer>(compjson[ComputerKey].ToString());
            return comp;
        }

        public Computer UpdateComputer(string token, Computer item)
        {
            var request = new RestRequest(ApiPath + ComputerPath + item.WebId, Method.PUT);
            request.AddParameter(Auth, token);
            var json = item.ToJSON();
            request.AddParameter("computer", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var compjson = JObject.Parse(response.Content);
            var comp = JsonConvert.DeserializeObject<Computer>(compjson[ComputerKey].ToString());
            return comp;
        }

        public IEnumerable<Computer> GetAllComputers(string token)
        {
            var request = new RestRequest(ApiPath + ComputerPath, Method.GET);
            request.AddParameter(Auth, token);
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
            return computers.FirstOrDefault(c => c.WebId == id);
        }

        public void DestroyComputer(string token, int id)
        {
            var request = new RestRequest(ApiPath + ComputerPath + id, Method.DELETE);
            request.AddParameter(Auth, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
#endregion

    #region Processes
        //TODO Processes
        public Process CreateProcess(string token, Process proc)
        {
            if (proc.AccountId <= 0)
                throw new AggregateException("Account Id needs to be present");
            var request = new RestRequest(ApiPath + ProcessPath, Method.POST);
            request.AddParameter(Auth, token);
            var json = proc.ToJSON();
            request.AddParameter("process", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var processjson = JObject.Parse(response.Content);
            var process = JsonConvert.DeserializeObject<Process>(processjson[ProcessKey].ToString());
            return process;
        }

        public Process UpdateProcess(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Process> GetAllProcessesByAccount(string token)
        {
            //TODO MAY NOT BE AVAILABLE
            throw new NotImplementedException();
        } 

        public Process GetProcessById()
        {
            throw new NotImplementedException();
        }
    #endregion

        #region History
        //TODO Histroy
        public History CreateHistory(string token)
        {
            throw new NotImplementedException();
        }

        public History UpdateHistory(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<History> GetAllHistoriesByAccount(string token)
        {
            //TODO MAY NOT BE AVAILABLE
            throw new NotImplementedException();
        }

        public History GetHistoryById(string token)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Program
        //TODO Programs
        public Program CreateProgram(string token, Program prog)
        {
            if (prog.AccountId == 0)
                throw new ArgumentException("Program must have an account Id");
            var request = new RestRequest(ApiPath + ProgramPath, Method.POST);
            request.AddParameter(Auth, token);
            var json = prog.ToJSON();
            request.AddParameter("program", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var programjson = JObject.Parse(response.Content);
            var program = JsonConvert.DeserializeObject<Program>(programjson[ProgramKey].ToString());
            return program;
        }

        public Program UpdateProgram(string token, Program prog)
        {
            var request = new RestRequest(ApiPath + ProgramPath + prog.WebId, Method.PUT);
            request.AddParameter(Auth, token);
            var json = prog.ToJSON();
            request.AddParameter("program", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var compjson = JObject.Parse(response.Content);
            var comp = JsonConvert.DeserializeObject<Program>(compjson[ProgramKey].ToString());
            return comp;
        }

        public Program GetProgramById(string token)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
