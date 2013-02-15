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
           
            /*dm.SaveUser(new User
                {
                    AuthToken = "MyAuthToken",
                    Email = "crouska@gmail.com",
                    Username = "Rizowski"
                });*/
            dm.SaveComputer(new Computer
                {
                    UserId = 1,
                    Enviroment = "Windows 8",
                    Name = "Rizowski-Lappy",
                    IpAddress = "192.168.1.1"
                });
        }

        public static void Main(string[] args)
        {
            var db = new DatabaseServer();
            Console.Read();
        }

    }
}
