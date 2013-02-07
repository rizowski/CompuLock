using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
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

        [JsonProperty(PropertyName = "history", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<AccountHistory> Histories { get; set; }
        [JsonProperty(PropertyName = "process", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<AccountProcess> Processes { get; set; }
        [JsonProperty(PropertyName = "programs", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<AccountProgram> Programs { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
