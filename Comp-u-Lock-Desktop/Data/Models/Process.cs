using System;
using Newtonsoft.Json;

namespace Data.Models
{
    class Process : IParser
    {
        private int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public DateTime LastRun { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
