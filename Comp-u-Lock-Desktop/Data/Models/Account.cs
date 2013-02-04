using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    class Account :IParser
    {
        private int id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
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
