using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data.Models
{
    [JsonObject]
    public class User : IParser
    {
        [JsonProperty]
        public int Id { get; private set; }
        [JsonIgnore]
        public bool Admin { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty]
        public string Username { get; set; }
        [JsonProperty]
        public string Email { get; set; }
        [JsonProperty]
        public string AuthToken { get; set; }

        [JsonProperty]
        public List<Computer> Computers { get; set; }

        public User()
        {
            
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
