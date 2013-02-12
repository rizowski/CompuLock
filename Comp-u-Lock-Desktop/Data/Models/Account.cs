using System.Collections.Generic;
using Data.Models;
using Newtonsoft.Json;

namespace Data.JSON.Models
{
    [JsonObject]
    public class Account :IParser
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "computer_id")]
        public int ComputerId { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "tracking")]
        public bool Tracking { get; set; }
        [JsonProperty(PropertyName = "allotted_time")]
        public int AllottedTime { get; set; }
        [JsonProperty(PropertyName = "used_time")]
        public int UsedTime { get; set; }

        [JsonProperty(PropertyName = "account_history", NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<History> Histories { get; set; }
        [JsonProperty(PropertyName = "account_process", NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<Process> Processes { get; set; }
        [JsonProperty(PropertyName = "account_program", NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<Program> Programs { get; set; } 

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
