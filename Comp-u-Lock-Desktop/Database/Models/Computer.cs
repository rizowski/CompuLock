using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Enviroment { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
