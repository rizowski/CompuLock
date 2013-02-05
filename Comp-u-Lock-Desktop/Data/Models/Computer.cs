using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    public class Computer : IParser
    {
        public int id { get; private set; }
        [JsonProperty]
        public string Domain { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public OperatingSystem OS { get; set; }

        public List<Account> Accounts { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
