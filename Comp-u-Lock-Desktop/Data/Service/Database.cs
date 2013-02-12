using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Security.AccessControl;
using Data;
using Data.JSON.Models;
using Data.Models;

namespace Data.Service
{
    public class Database
    {
        public ModelsContext context;
        public Database(string file)
        {
            var connection = new SQLiteConnection();
            connection.ConnectionString = new DbConnectionStringBuilder()
                {
                    {"Data Source", file},
                    {"Version", "3"},
                    {"FailIfMissing", "false"}
                }.ConnectionString;
            Console.WriteLine(connection.ConnectionString);
            context = new ModelsContext();
            context.Database.CreateIfNotExists();

        }

        public void StoreComputer(Computer comp)
        {
            context.Computers.Add(comp);
            context.SaveChanges();

        }

    }
}
