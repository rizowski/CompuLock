using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using REST.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Service.REST
{
    class RestComputer : RestService, IRest<Computer>
    {
        public RestComputer(string server, string apipath) : base(server, apipath)
        {
        }

        public Computer Create(string token, Computer item)
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

        public Computer Update(string token, Computer item)
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

        public IEnumerable<Computer> GetAll(string token)
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

        public Computer GetOneById(string token, int id)
        {
            var computers = GetAll(token);
            return computers.FirstOrDefault(c => c.Id == id);
        }

        public void Destroy(string token, int id)
        {
            var request = new RestRequest(ApiPath+"computers/" + id, Method.DELETE);
            request.AddParameter(AUTH, token);
            var response = Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Status Code Error: {0}", response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
