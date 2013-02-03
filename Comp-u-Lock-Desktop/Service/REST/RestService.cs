using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Service.REST
{
    class RestService
    {
        public RestClient Client;
        public RestService(string server)
        {
            Client = new RestClient(server);
        }

        public void Post()
        {
            
        }

        public void Get(string controller, int? id=null)
        {
            var request = new RestRequest(controller + "/{id}", Method.GET);
            if (id.HasValue)
            {
                request.AddUrlSegment("id", "" + id);
            }
            request.RequestFormat = DataFormat.Json;
            var response = Client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void Login(string username, string password)
        {
            var request = new RestRequest("users/sign_in",Method.GET);
            
            request.AddParameter("email", username);
            request.AddParameter("password", password);

            var response = Client.Execute(request);

            Console.WriteLine(response.Content);
        }
    }
}
