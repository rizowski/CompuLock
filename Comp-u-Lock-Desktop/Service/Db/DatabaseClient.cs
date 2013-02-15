using Database;
using Database.Models;

namespace Service.Db
{
    public class DatabaseClient : DatabaseManager
    {

        public DatabaseClient(string path, string pass):base(path, pass)
        {

            /*dm.SaveUser(new User
                {
                    AuthToken = "MyAuthToken",
                    Email = "crouska@gmail.com",
                    Username = "Rizowski"
                });
            dm.SaveComputer(new Computer
                {
                    UserId = 1,
                    Enviroment = "Windows 8",
                    Name = "Rizowski-Lappy",
                    IpAddress = "192.168.1.1"
                });
            dm.SaveAccount(new Account
                {
                    ComputerId = 1,
                    Domain = "WORKGROUP",
                    Username = "Rizowski",
                    Tracking = true
                });
             var accounts=  dm.GetAccounts();
            foreach (var account in accounts)
            {
                Console.WriteLine(account.Username);
            }
            dm.SaveHistory(new History
                {
                    AccountId = 1,
                    Title ="Facebook",
                    Domain = "Facebook.com",
                    Url = "/Rizowski",
                    VisitCount = 1
                });*/
            /*var histories = dm.GetHistoryForAccount(1);
            foreach (var history in histories)
            {
                Console.WriteLine("{0} - {1}", history.Title, history.Domain);
            }*/
        }

        

        
    }
}
