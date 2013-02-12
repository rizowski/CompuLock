using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Enviroment { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
