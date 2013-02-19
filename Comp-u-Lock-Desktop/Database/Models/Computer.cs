using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database.Models
{
    [JsonObject]
    public class Computer
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int WebId { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "enviroment")]
        public string Enviroment { get; set; }
        [JsonProperty(PropertyName = "ip_address", NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        [JsonProperty(PropertyName = "account_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<Account> Accounts { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public Computer()
        {

        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
