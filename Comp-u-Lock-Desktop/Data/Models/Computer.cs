using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Models
{
    public class Computer : IParser
    {
        private int id { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
        public OperatingSystem OS { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
