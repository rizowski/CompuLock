using System;
using System.Collections.Generic;

namespace Database.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Domain { get; set; }
        public bool Tracking { get; set; }

        public TimeSpan AllottedTime { get; set; }
        public TimeSpan UsedTime { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual IEnumerable<History> Histories { get; set; }
        public virtual IEnumerable<Process> Processes { get; set; }
        public virtual IEnumerable<Program> Programs { get; set; }
    }
}
