using System.Linq;
using Database.Models;
using Raven.Client.Document;

namespace Service.Db
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

        public Account FindAccountById(int id)
        {
            Account comp = null;
            using (var session = Store.OpenSession())
            {
                comp = session.Load<Account>(id);
            }
            return comp;
        }

        public User FindUserByAuthToken(string token)
        {
            User user = null;
            using (var session = Store.OpenSession())
            {
                return user = session.Query<User>().First(c => c.AuthToken == token);
            }
        }
    }
}
