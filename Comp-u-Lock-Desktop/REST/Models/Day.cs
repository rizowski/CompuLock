using System.Collections.Generic;
using Newtonsoft.Json;

namespace REST.Models
{
    public class Day
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "restriction_id")]
        public int RestrictionId { get; set; }

        [JsonProperty(PropertyName = "hour_attributes", NullValueHandling = NullValueHandling.Ignore)]
        IEnumerable<Hour> Hours { get; set; }

        public Day(IEnumerable<Hour>  hours)
        {
            Hours = hours;
        }

        public Day()
        {
            
        }
    }

    public class Hour
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "day_id")]
        public int DayId { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public int End { get; set; }

        public Hour(int start, int end)
        {
            Start = start;
            End = end;
        }

        public Hour()
        {
            
        }
    }
}
