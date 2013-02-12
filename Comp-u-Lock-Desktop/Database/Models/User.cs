using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AuthToken { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }
    }
}
