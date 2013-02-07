using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class Account :IParser
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public int ComputerId { get; set; }
        [JsonProperty]
        public string Domain { get; set; }
        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public bool Tracking { get; set; }
        [JsonProperty]
        public int AllottedTime { get; set; }
        [JsonProperty]
        public int UsedTime { get; set; }

        public IEnumerable<AccountHistory> Histories { get; set; }
        public IEnumerable<AccountProcess> Processes { get; set; }
        public IEnumerable<AccountProgram> Programs { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
