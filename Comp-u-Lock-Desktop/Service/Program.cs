using System;
using Data.Service;
using Data.Enviroment;
using Data.Models;
using Service.REST;
using Service.Users;
using Processes = Service.Users.Processes;
using Programs = Service.Users.Programs;

namespace Service
{
    public class Program
    {
        static  void Main(string[] args)
        {
            /*Console.WriteLine("History:");
            RunHistory();*/
            /*Console.WriteLine("\nSQLiteHistory");
            GetSQLiteHistory();*/
            /*Console.WriteLine("\nProcesses:");
            RunProcesses();*/
           /* Console.WriteLine("\nPrograms:");
            RunPrograms();*/
            /*Console.WriteLine("\nRun Os");
            RunOS();*/
            /*Console.WriteLine("\nRun User");
            RunUser();*/
            /*Console.WriteLine("\nRun DbAccounts");
            RunDbAccounts();*/
            Console.WriteLine("\nRun REST");
            REST();
            Console.Read();

        }

        private static void RunDbAccounts()
        {
            User u = new User();

            u.CreatedAt = DateTime.Now;
            u.UpdatedAt = DateTime.Now;
            u.Email = "crouska@gmail.com";
            u.Username = "Rizowski";
            Database db = new Database("settings.db");

            db.Write(u.ToJSON());
            
            
            Console.WriteLine(u.ToJSON());
        }

        public static void RunProcesses()
        {
            var record = new Processes();
            var processes = record.GetProcesses();
            foreach (var process in processes)
            {
                Console.WriteLine(process.ProcessName);
            }
            Console.WriteLine();
            var userprocesses = record.GetProcessesByUser("Rizowski");
            Console.WriteLine();
            Console.WriteLine("Named Processes");
            foreach (var process in userprocesses)
            {
                object[] outParameters = new object[2];
                process.InvokeMethod("GetOwner", outParameters);
                Console.WriteLine("{0} - {1}",outParameters[0], process["name"]);
            }
        }

        public static void RunPrograms()
        {
            Programs pro = new Programs();
            pro.GetRunningPrograms();
        }

        public static void RunHistory()
        {
            Console.WriteLine("Internet Explorer");
            var ie = new InternetExplorerHistoryReader();
            Console.WriteLine(ie.IsRunning());
            Console.WriteLine(ie.Version);
            var urls = ie.GetHistory();
            foreach (var url in urls)
            {
                Console.WriteLine(url);
            }

            Console.WriteLine("\nFirefox");
            var f = new FirefoxHistoryReader(@"\3fcwvrqq.default");
            f.Filename = "places.sqlite";
            Console.WriteLine(f.DbPath);
            Console.WriteLine(f.Filename);
            Console.WriteLine(f.IsRunning());
            if (!f.IsRunning())
            {
                f.Connect();
            }

            Console.WriteLine("\nChrome");
            var c = new ChromeHistoryReader();
            Console.WriteLine(c.IsRunning());
            Console.WriteLine(c.Version.ToString());
            if (!c.IsRunning())
            {
                c.Connect();
                c.GetHistory();
            }
        }

        public static void RunOS()
        {
            Console.WriteLine(Environment.OSVersion.Version);
            Console.WriteLine(OS.Version);
        }

        public static void RunUser()
        {
            UserManager account = new UserManager(Environment.UserDomainName, "Kids", new TimeSpan(0,0,0,5));
            var users = account.GetUsers();
            Console.WriteLine("Getting users");
            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }
            Console.WriteLine();
            account.StartTimer();
            Console.WriteLine("Hit enter to unlock");
            Console.ReadLine();
            account.UnlockAccount();
            
            //account.GetUserSessionId();
            //Console.WriteLine("Account: Rizowski - id: {0}",account.GetUserSessionId("Kids"));
            //var session = account.GetUserSession("Kids");
            //Console.WriteLine("{0} - {1}", session.UserName, session.SessionId);
            //account.DisconnectUser("Kids");
            //account.LockAccount("Kids");
            //account.UnlockAccount("Kids");

            //account.GetUsers();
            //account.LockAccount("Kids");//.ChangeUserPassword("Parents", "190421", "rizowski");
            //account.StartTimer();
        }

        public static void REST()
        {
            RestService rs = new RestService("http://localhost:3000");
            string token = "AiprpscAqN6hnvNDHSwh";
            //var user = rs.GetUser(token);
            //Console.WriteLine("Email: {0}", user.Email);
            //Console.WriteLine("Auth Token: {0}",user.AuthToken);
            //Console.WriteLine("Username: {0}",user.Username);

            //var accounts = rs.GetAccounts(token, 1);
            //var user = rs.GetUser(token);
            //var computers = rs.GetComputers(token);

            //User u = new User();
            //u.Id = 1;
            //u.Email = "Bob@gmail.com";
            //u.Username = "Rizowski";
            //rs.UpdateUser(token, u);

            Computer c = new Computer
                {
                    Enivroment = "MyEnviroment",
                    IpAddress = "0.0.0.0",
                    Name = "MINE",
                    UserId = 1
                };
            rs.CreateComputer(token,c);

        }

    }
}
