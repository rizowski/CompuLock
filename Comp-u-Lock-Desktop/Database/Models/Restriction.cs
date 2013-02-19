using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Database.Models
{
    public class Restriction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        IEnumerable<Day> Days { get; set; }

        public Restriction()
        {
        }
    }
}
