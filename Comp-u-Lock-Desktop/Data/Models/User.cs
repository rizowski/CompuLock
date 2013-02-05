using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data.Models
{
    [JsonObject]
    public class User : IParser
    {
        [JsonIgnore]
        public int Id { get; private set; }
        [JsonIgnore]
        private bool Admin { get; set; }
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

        public User(User user)
        {
            this.Id = user.Id;
            this.Admin = user.Admin;
            this.CreatedAt = user.CreatedAt;
            this.UpdatedAt = user.UpdatedAt;

            this.Username = user.Username;
            this.Email = user.Email;
            this.AuthToken = user.AuthToken;
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
