using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database.Models
{
    [JsonObject]
    public class Account
    {
        public Account()
        {
        }

        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int WebId { get; set; }

        [JsonProperty(PropertyName = "computer_id")]
        public int ComputerId { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "admin", NullValueHandling = NullValueHandling.Ignore)]
        public bool Admin { get; set; }

        [JsonIgnore]
        public bool Locked { get; set; }

        [JsonProperty(PropertyName = "tracking")]
        public bool Tracking { get; set; }
        [JsonProperty(PropertyName = "allotted_time")]
        public TimeSpan AllottedTime { get; set; }
        [JsonProperty(PropertyName = "used_time")]
        public TimeSpan UsedTime { get; set; }

        [JsonProperty(PropertyName = "account_process_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Process> Processes { get; set; }

        [JsonProperty(PropertyName = "restriction_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Restriction> Restrictions { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
