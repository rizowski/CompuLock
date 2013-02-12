using System.Data.Entity;
using Database.Models;

namespace Database
{
    // http://weblogs.asp.net/scottgu/archive/2010/07/16/code-first-development-with-entity-framework-4.aspx
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("ModelEntities")
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Process> Processes { get; set; }
    }
}
