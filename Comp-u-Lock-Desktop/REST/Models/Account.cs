﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace REST.Models
{
    [JsonObject]
    public class Account
    {
        public Account()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "computer_id")]
        public int ComputerId { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "tracking")]
        public bool Tracking { get; set; }
        [JsonProperty(PropertyName = "allotted_time")]
        public int AllottedTime { get; set; }
        [JsonProperty(PropertyName = "used_time")]
        public int UsedTime { get; set; }

        [JsonProperty(PropertyName = "account_history_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<History> Histories { get; set; }
        [JsonProperty(PropertyName = "account_process_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Process> Processes { get; set; }
        [JsonProperty(PropertyName = "program_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Program> Programs { get; set; }

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
