using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Models
{
    class Restriction
    {
        List<Day> Days { get; set; }
        public int AccountId { get; set; }

        public Restriction(List<Day> days)
        {
            Days = days;
        }

        public Restriction()
        {
        }
    }
}
