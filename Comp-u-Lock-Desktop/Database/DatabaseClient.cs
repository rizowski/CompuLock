using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Models;
using Raven.Client.Document;
using Raven.Client.Linq;

namespace Database
{
    public class DatabaseClient
    {
        public DocumentStore Store { get; set; }

        public DatabaseClient(string server)
        {
            Store = new DocumentStore{Url = server};
            Store.Initialize();
        }

        public T Save<T>(T t)
        {
            using (var session = Store.OpenSession())
            {
                session.Store(t);
                session.SaveChanges();
            }
            return t;
        }

        public Computer FindComputerById(int id)
        {
            Computer comp = null;
            using (var session = Store.OpenSession())
            {
                comp = session.Load<Computer>(id);
            }
            return comp;
        }
    }
}
