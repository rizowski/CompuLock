using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class Computer : IParser
    {
        [JsonProperty]
        public int Id { get; private set; }
        [JsonProperty]
        public int UserId { get; set; }
        [JsonProperty]
        public string Domain { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Enivroment { get; set; }
        [JsonProperty]
        public string IpAddress { get; set; }

        [JsonProperty]
        public List<Account> Accounts { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
