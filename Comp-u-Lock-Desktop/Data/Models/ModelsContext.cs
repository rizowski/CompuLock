using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Data.JSON.Models;

namespace Data.Models
{
    public class ModelsContext : DbContext 
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Computer> Computers { get; set; }

        public DbSet<History> Histories { get; set; }

        public DbSet<Process> Processes { get; set; }

        public DbSet<Program> Programs { get; set; }
    }
}
