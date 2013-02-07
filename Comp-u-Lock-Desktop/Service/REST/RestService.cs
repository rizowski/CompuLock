using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Data.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Service.REST
{
    public class RestService: IApi
    {
        public RestClient Client;
        private const string AUTH = "auth_token";
        public RestService(string server)
        {
            Client = new RestClient(server);
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

        public List<Computer> GetComputers(string token)
        {
            var request = new RestRequest("api/v1/computers", Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var comp = JObject.Parse(response.Content);
            var comps = JsonConvert.DeserializeObject < List<Computer>>(comp["computers"].ToString());
            // http://james.newtonking.com/projects/json/help/
            return comps;
        }

        public List<Account> GetAccounts(string token, int computerId)
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

        public List<AccountHistory> Histories(string token, int accountId)
        {
            throw new NotImplementedException();
        }

        public List<AccountProcess> GetProcesses(string token, int accountId)
        {
            throw new NotImplementedException();
        }

        public List<AccountProgram> GetPrograms(string token, int accountId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string token, User user)
        {
            var request = new RestRequest("api/v1/users/"+user.Id, Method.PUT);
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
            throw new NotImplementedException();
        }

        public void UpdateAcount(string token, Account account)
        {
            throw new NotImplementedException();
        }

        public void WriteComputer(string token, Computer computer)
        {
            var request = new RestRequest("api/v1/users/", Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(computer);
        }

        public void WriteAccount(string token, int computerId, Account account)
        {
            throw new NotImplementedException();
        }

        public void WriteHistory(string token, int accountId, AccountHistory history)
        {
            throw new NotImplementedException();
        }

        public void WriteProgram(string token, int accountId, AccountProgram program)
        {
            throw new NotImplementedException();
        }

        public void WriteProcess(string token, int accountId, AccountProcess process)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IRestResponse restResponse)
        {
            Console.Write("Deserializing");
            Console.WriteLine(restResponse.Content);
        }
    }
}
