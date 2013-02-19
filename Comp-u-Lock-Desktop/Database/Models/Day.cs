using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Database.Models
{
    public class Day
    {
        public int Id { get; set; }
        public int WebId { get; set; }

        public int RestrictionId { get; set; }

        IEnumerable<Hour> Hours { get; set; }


        public Day()
        {
            
        }
    }

    public class Hour
    {
        public int Id { get; set; }
        public int WebId { get; set; }

        public int DayId { get; set; }

        public int Start { get; set; }
        public int End { get; set; }

        public Hour()
        {
            
        }
    }
}
