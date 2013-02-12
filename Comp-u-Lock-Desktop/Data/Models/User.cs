using System;
using System.Collections.Generic;
using Data.Models;
using Newtonsoft.Json;

namespace Data.JSON.Models
{
    [JsonObject]
    public class User : IParser
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "computers", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Computer> Computers { get; set; }


        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public User(int id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }

        public User()
        {
            
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
