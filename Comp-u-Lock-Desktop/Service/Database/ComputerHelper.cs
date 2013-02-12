using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Database;
using Database.Models;

namespace Service.Database
{
    class ComputerHelper
    {
        public DatabaseContext Context { get; set; }
        public ComputerHelper()
        {
            Context = new DatabaseContext();
        }

        public void Save(Computer computer)
        {
            Context.Computers.Add(computer);
            Context.SaveChanges();
        }
    }
}
