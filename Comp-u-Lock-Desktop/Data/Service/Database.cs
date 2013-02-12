using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Security.AccessControl;
using Data.Database;

namespace Data.Service
{
    public class Database
    {
        public ModelsContainer context;
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
            context = new ModelsContainer();
            context.Database.CreateIfNotExists();

        }

        public void StoreComputer(Data.Database.Computer comp)
        {
            var con = new ModelsContainer();
            con.Computers.Add(comp);
            con.SaveChanges();

        }

    }
}
