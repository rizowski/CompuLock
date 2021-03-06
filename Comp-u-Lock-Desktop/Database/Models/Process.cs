﻿using System;
using Newtonsoft.Json;

namespace Database.Models
{
    [JsonObject]
    public class Process
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int WebId { get; set; }

        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
