using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Data
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Domain { get; set; }
    }

    public class AccountContext : DbContext
    {
        public DbSet<Account> ComputerAccounts { get; set; }
    }
}
