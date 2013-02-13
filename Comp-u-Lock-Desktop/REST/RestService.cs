using RestSharp;

namespace REST
{
    public class RestService//: IApi
    {
        public string ApiPath { get; set; }
        public RestClient Client { get; set; }
        protected const string AUTH = "auth_token";

        public RestService(string server, string apipath)
        {
            Client = new RestClient(server);
            ApiPath = apipath;
        }
    }
}
