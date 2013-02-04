using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Models;
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
            RestResponse<User> response = (RestResponse<User>) Client.Execute<User>(request);
            response.Data.Email = username;
            response.Data.Password = password;
            
            request.Method = Method.POST;
            var sucess = Client.Execute(request);

            Console.WriteLine(sucess.Content);
        }

        public string GetToken(string username, string password)
        {
            var request = new RestRequest("api/v1/tokens", Method.POST);
            request.AddParameter("email", "crouska@gmail.com");
            request.AddParameter("password", "190421");
            var response = Client.Execute(request);
            return response.Content;
        }
    }
}
