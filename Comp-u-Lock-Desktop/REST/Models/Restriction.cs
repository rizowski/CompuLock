using System.Collections.Generic;
using Newtonsoft.Json;

namespace REST.Models
{
    class Restriction
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }

        [JsonProperty(PropertyName = "day_attributes", NullValueHandling = NullValueHandling.Ignore)]
        IEnumerable<REST.Models.Day> Days { get; set; }

        public Restriction(IEnumerable<REST.Models.Day> days)
        {
            Days = days;
        }

        public Restriction()
        {
        }
    }
}
