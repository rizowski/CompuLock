using System;
using Newtonsoft.Json;

namespace REST.Models
{
    [JsonObject(Title = "AccountHistory")]
    public class History
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "visit_count")]
        public int VisitCount { get; set; }

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
