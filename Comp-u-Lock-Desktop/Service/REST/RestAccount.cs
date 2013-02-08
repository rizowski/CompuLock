using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Service.REST
{
    class RestAccount : RestService, IRest<Data.Models.Account>
    {
        private const string ACCOUNT = "accounts/";

        public RestAccount(string server, string apipath) : base(server, apipath)
        {

        }

        public Data.Models.Account Create(string token, Data.Models.Account item)
        {
            if (item.ComputerId == 0)
                throw new ArgumentException("Account must have a computer Id");
            var request = new RestRequest(ApiPath+ACCOUNT, Method.POST);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(item);
            request.AddParameter("account", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            throw new NotImplementedException();
        }

        public Data.Models.Account Update(string token, Data.Models.Account item)
        {
            var request = new RestRequest(ApiPath + ACCOUNT + item.Id, Method.PUT);
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

        public IEnumerable<Data.Models.Account> GetAll(string token)
        {
            var request = new RestRequest(ApiPath + ACCOUNT, Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var account = JObject.Parse(response.Content);
            var accounts = JsonConvert.DeserializeObject<List<Data.Models.Account>>(account["accounts"].ToString());
            return accounts;
        }

        public Data.Models.Account GetOneById(string token, int id)
        {
            var accounts = GetAll(token);
            return accounts.FirstOrDefault(a => a.Id == id);
        }

        public void Destroy(string token, int id)
        {
            var request = new RestRequest(ApiPath + ACCOUNT + id, Method.DELETE);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
