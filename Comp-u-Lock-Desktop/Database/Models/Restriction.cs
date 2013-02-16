using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Models
{
    class Restriction
    {
        IEnumerable<Day> Days { get; set; }
        public int AccountId { get; set; }

        public Restriction(IEnumerable<Day> days)
        {
            Days = days;
        }

        public Restriction()
        {
        }
    }
}
