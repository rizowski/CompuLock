using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Models;


namespace Service.Profile
{
    public class TimeManager
    {
        public IEnumerable<Account> accounts { get; set; }
        public double Interval { get; set; }

        public TimeManager(IEnumerable<Account> accounts, double interval = 1)
        {
            Interval = interval;
           
        }
    }
}
