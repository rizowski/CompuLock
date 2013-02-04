using System;
using Newtonsoft.Json;

namespace Data.Models
{
    public class Program : IParser
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public DateTime LastRun { get; set; }
        public int OpenCount { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
