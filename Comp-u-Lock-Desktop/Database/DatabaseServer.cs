using System;
using System.Threading;
using Database.Models;

namespace Database
{
    public class DatabaseServer
    {
        
        public DatabaseServer()
        {
            DatabaseManager dm = new DatabaseManager("settings", "myPass");
           
            dm.SaveUser(new User
                {
                    AuthToken = "MyAuthToken",
                    Email = "crouska@gmail.com",
                    Username = "Rizowski"
                });
        }

        public static void Main(string[] args)
        {
            var db = new DatabaseServer();
            Console.Read();
        }

    }
}
