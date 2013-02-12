using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<Process> Processes { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
    }
}
