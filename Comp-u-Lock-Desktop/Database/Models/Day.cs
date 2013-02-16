using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Models
{
    public class Day
    {
        List<Hour> Hours { get; set; }

        public Day(List<Hour>  hours)
        {
            Hours = hours;
        }

        public Day()
        {
            
        }
    }

    public class Hour
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Hour(int start, int end)
        {
            Start = start;
            End = end;
        }

        public Hour()
        {
            
        }
    }
}
