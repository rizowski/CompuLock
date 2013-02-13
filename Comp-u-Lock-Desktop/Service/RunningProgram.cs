using System;
using System.Collections.Generic;
using Database;
using Database.Enviroment;
using Database.Models;
using Service.Users;
using Processes = Service.Users.Processes;
using Programs = Service.Users.Programs;

namespace Service
{
    public class RunningProgram
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
            Console.WriteLine("\nRun DbAccounts");
            RunDbAccounts();
            //Console.WriteLine("\nRun REST");
            //REST();
            /*Console.WriteLine("\nRun REST 2.0");
            REST2();*/
            Console.Read();

        }

        private static void RunDbAccounts()
        {

            DatabaseServer ds = new DatabaseServer("Data", true);
            ds.StartServer();
            DatabaseClient dc = new DatabaseClient("http://localhost:8080");
            var user = new User
                {
                    AuthToken = "HOLYCRAPITWORKS",
                    DateCreated = DateTime.Now,
                    Email = "crouska@gmail.com",
                    LastUpdated = DateTime.Now,
                    Username = "rizowski"
                };
            
            var comp = new Computer();
            comp.Enviroment = "Windows 8";
            comp.Name = "Rizos Computer";
            comp.IpAddress = "127.0.0.1";
            comp.DateCreated = DateTime.Now;
            comp.DateUpdated = DateTime.Now;
            dc.Save(user);
            dc.Save(comp);
            var account = new Account()
                {
                    Domain = "RIZOWSKI",
                    Username = "Rizowski",
                    Tracking = true,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                };
            dc.Save(account);

            //var user = dc.FindUserByAuthToken("HOLYCRAPITWORKS");
            //TODO Take each item and put it into the database. Having one main USer object?
            //Console.WriteLine(user.Email);
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

            RestService rs = new RestService("http://localhost:3000", "api/v1/");
            string token = "AiprpscAqN6hnvNDHSwh";

            /*var user = rs.GetUser(token);
            Console.WriteLine("Email: {0}", user.Email);
            Console.WriteLine("Username: {0}", user.Username);*/

            //var accounts = rs.GetAccounts(token);
            //var user = rs.GetUser(token);
            //var computers = rs.GetComputers(token);

            //User u = new User();
            //u.Id = 1;
            //u.Email = "Bob@gmail.com";
            //u.Username = "Rizowski";
            //rs.UpdateUser(token, u);

            //Computer c = new Computer
            //    {
            //        Enviroment = "MyEnviroment",
            //        IpAddress = "0.0.0.0",
            //        Name = "MINE",
            //        UserId = 1
            //    };
            //rs.CreateComputer(token, c);

            //var histories = rs.GetHistory(token, 2);
            //foreach (var accountHistory in histories)
            //{
            //    Console.WriteLine("Domain: {0}",accountHistory.Domain);
            //}

        }

        public static void REST2()
        {
            //var server = "http://localhost:3000";
            //var api = "api/v1/";
            //string token = "AiprpscAqN6hnvNDHSwh";

            //RestUser u = new RestUser(server, api);

            //var user = u.Get(token);
            //Console.WriteLine("Id:{0}",user.Id);
            //Console.WriteLine("Username: {0}", user.Username);
            //Console.WriteLine("Email: {0}", user.Email);
            //user.Username = "Rizowski";

            //var newuser = new User(user.Id, "Rizowski", user.Email);

            //user = u.Update(token, newuser);
            //Console.WriteLine("Id:{0}", user.Id);
            //Console.WriteLine("Username: {0}", user.Username);
            //Console.WriteLine("Email: {0}", user.Email);

            //RestComputer c = new RestComputer(server, api);

            //Computer comp = new Computer( 1, "Bobs", "Windows Eight", "0.0.0.0");

            //var create = c.Create(token, comp);
            //Console.WriteLine(create.Id);
            //Console.WriteLine("Name: {0}", create.Name);
            //Console.WriteLine("Enviroment: {0}", create.Enviroment);
            //Console.WriteLine("Ip: {0}", create.IpAddress);

            //var oldnew = new Computer(create.Id, create.UserId, "Not Bobs", "Crap", "1.1.1.1");

            //var newcomp = c.Update(token, oldnew);
            //Console.WriteLine(newcomp.Id);
            //Console.WriteLine("Name: {0}", newcomp.Name);
            //Console.WriteLine("Enviroment: {0}", newcomp.Enviroment);
            //Console.WriteLine("Ip: {0}", newcomp.IpAddress);
        }

    }
}
