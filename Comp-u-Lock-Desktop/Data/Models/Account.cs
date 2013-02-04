using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    public class Account :IParser
    {
        private int id { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public bool Admin { get; set; }
        public bool Tracking { get; set; }

        public IEnumerable<History> Histories { get; set; }
        public IEnumerable<Process> Processes { get; set; }
        public IEnumerable<Program> Programs { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
