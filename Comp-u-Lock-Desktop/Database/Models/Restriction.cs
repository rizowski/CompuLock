using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database.Models
{
    class Restriction
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }

        [JsonProperty(PropertyName = "day_attributes", NullValueHandling = NullValueHandling.Ignore)]
        IEnumerable<Day> Days { get; set; }

        public Restriction()
        {
        }
    }
}
