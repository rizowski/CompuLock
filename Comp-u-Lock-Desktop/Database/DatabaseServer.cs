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
            /*dm.SaveComputer(new Computer
                {
                    UserId = 1,
                    Enviroment = "Windows 8",
                    Name = "Rizowski-Lappy",
                    IpAddress = "192.168.1.1"
                });*/
            /*dm.SaveAccount(new Account
                {
                    ComputerId = 1,
                    Domain = "WORKGROUP",
                    Username = "Rizowski",
                    Tracking = true
                });*/
            var accounts=  dm.GetAccounts();
            foreach (var account in accounts)
            {
                Console.WriteLine(account.Username);
            }
        }

        public static void Main(string[] args)
        {
            var db = new DatabaseServer();
            Console.Read();
        }

    }
}
