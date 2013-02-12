
using System;
using Database;
using Database.Models;

namespace Service.Db
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
            Console.WriteLine("Saved");

        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
