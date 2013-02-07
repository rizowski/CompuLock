using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class Computer : IParser
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "enviroment")]
        public string Enivroment { get; set; }
        [JsonProperty(PropertyName = "ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty(PropertyName = "accounts", NullValueHandling = NullValueHandling.Ignore)]
        public List<Account> Accounts { get; set; }

        public Computer(int id, int userId, string domain, string name, string enviroment, string ipAddress, List<Account> accounts )
        {
            Id = id;
            UserId = userId;
            Domain = domain;
            Name = name;
            Enivroment = enviroment;
            IpAddress = ipAddress;
            Accounts = accounts;
        }
        public Computer()
        {

        }

        public Computer(int id, int userId, string domain, string name, string enviroment, string ipAddress)
        {
            Id = id;
            UserId = userId;
            Domain = domain;
            Name = name;
            Enivroment = enviroment;
            IpAddress = ipAddress;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
