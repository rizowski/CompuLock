using System;
using System.Net;
using Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Service.REST
{
    class RestUser : RestService
    {
        private const string USER = "users/";

        public RestUser(string server, string apipath) : base(server, apipath)
        {
        }

        public User Update(string token, User item)
        {
            if(item.Computers != null)
                throw new ArgumentException("Computers needs to be null");
            var request = new RestRequest(ApiPath+USER + item.Id, Method.PUT);
            request.AddParameter(AUTH, token);
            var json = JsonConvert.SerializeObject(item);
            request.AddParameter("user", json);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
            var userjson = JObject.Parse(response.Content);
            var user = JsonConvert.DeserializeObject<User>(userjson["user"].ToString());
            return user;
        }

        public User Get(string token)
        {
            var request = new RestRequest(ApiPath+USER, Method.GET);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            var userjson = JObject.Parse(response.Content);
            var user = JsonConvert.DeserializeObject<User>(userjson["user"].ToString());
            return user;
        }
    }
}
