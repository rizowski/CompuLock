using System.Collections.Generic;
using Newtonsoft.Json;

namespace REST.Models
{
    [JsonObject]
    public class Computer : IParser
    {
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public int Id { get; private set; }
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "enviroment")]
        public string Enviroment { get; set; }
        [JsonProperty(PropertyName = "ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty(PropertyName = "accounts", NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<Account> Accounts { get; set; } 

        public Computer(int id, int userId, string name, string enviroment, string ipAddress, List<Account> accounts )
        {
            Id = id;
            UserId = userId;
            Name = name;
            Enviroment = enviroment;
            IpAddress = ipAddress;
            Accounts = accounts;
        }
        public Computer()
        {

        }

        public Computer(int id, int userId, string name, string enviroment, string ipAddress)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Enviroment = enviroment;
            IpAddress = ipAddress;
        }

        public Computer( int userId, string name, string enviroment, string ipAddress)
        {
            UserId = userId;
            Name = name;
            Enviroment = enviroment;
            IpAddress = ipAddress;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
