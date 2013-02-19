using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace REST.Models
{
    [JsonObject]
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "computer_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<REST.Models.Computer> Computers { get; set; }
        [JsonIgnore]
        public string AuthToken { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public User()
        {
            
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
