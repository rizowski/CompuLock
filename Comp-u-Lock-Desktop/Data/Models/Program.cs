using System;
using Newtonsoft.Json;

namespace Data.Models
{
    [JsonObject]
    public class AccountProgram : IParser
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public int AccountId { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonIgnore]
        public DateTime LastRun { get; set; }
        [JsonProperty]
        public int OpenCount { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
