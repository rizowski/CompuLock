using System;
using System.Net;
using Database.Models;
using Raven.Imports.Newtonsoft.Json;
using Raven.Imports.Newtonsoft.Json.Linq;
using RestSharp;

namespace REST.Service
{
    public class RestUser : RestService
    {
        private const string USER = "users/";

        public RestUser(string server, string apipath) : base(server, apipath)
        {
        }

        public User Update(string token, User item)
        {
            var request = new RestRequest(ApiPath+USER + item.Id, Method.PUT);
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

        public User Get(string token)
        {
            var request = new RestRequest(ApiPath+USER, Method.GET);
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
            catch (Exception e )
            {
                Console.WriteLine(e);
            }
            return user;
        }
    }
}
