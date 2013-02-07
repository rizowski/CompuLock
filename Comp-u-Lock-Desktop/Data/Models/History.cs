using System;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class AccountHistory : IParser
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "last_visited")]
        public DateTime LastVisited { get; set; }
        [JsonProperty(PropertyName = "visit_count")]
        public int VisitCount { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
