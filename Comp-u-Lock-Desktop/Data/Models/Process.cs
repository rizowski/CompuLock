using System;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class AccountProcess : IParser
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public int AccountId { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public DateTime LastRun { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
