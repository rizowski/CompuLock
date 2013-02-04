using System;
using Newtonsoft.Json;

namespace Data.Models
{
    class History : IParser
    {
        private int Id { get; set; }
        public int AccountId { get; set; }
        public string Domain { get; set; }
        public string Url { get; set; }
        public DateTime LastVisited { get; set; }
        public int VisitCount { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
