using System;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class AccountHistory : IParser
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public int AccountId { get; set; }
        [JsonProperty]
        public string Domain { get; set; }
        [JsonProperty]
        public string Url { get; set; }
        [JsonProperty]
        public DateTime LastVisited { get; set; }
        [JsonProperty]
        public int VisitCount { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
