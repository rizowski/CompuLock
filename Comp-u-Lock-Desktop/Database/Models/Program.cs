using System;
using Newtonsoft.Json;

namespace Database.Models
{
    [JsonObject(Title = "AccountProgram")]
    public class Program
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonIgnore]
        public DateTime LastRun { get; set; }
        [JsonProperty(PropertyName = "open_count")]
        public int OpenCount { get; set; }

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
