using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Database.Enviroment;
using Database.Models;
using REST;
using REST.Service;
using Service.Db;
using Service.Profile;
using Processes = Service.Profile.Processes;
using Programs = Service.Profile.Programs;

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
            //Console.WriteLine("\nRun REST");
            //REST();
            /*Console.WriteLine("\nRun Program");
            RunGUIProgram();*/
            //Console.WriteLine("\nCycle");
            //Cylce();

            Console.WriteLine("\nRun DbAccounts");
            try
            {
                RunDbAccounts();
            }
            catch (Exception e )
            {
                Console.WriteLine(e);
            }
            
            /*Console.WriteLine("\nRun REST 2.0");
            REST2();*/
            Console.Read();

        }

        private static void RunDbAccounts()
        {

            DatabaseClient dc = new DatabaseClient("settings", "myPass");
            /*dc.SaveUser(new User
            {
                AuthToken = "MyAuthToken",
                Email = "crouska@gmail.com",
                Username = "Rizowski"
            });
            var user = dc.GetUser();
            Console.WriteLine("{0} - {1}", user.Email, user.AuthToken);
            Console.WriteLine();
            dc.SaveComputer(new Computer
            {
                UserId = 1,
                Enviroment = "Windows 8",
                Name = "Rizowski-Lappy",
                IpAddress = "192.168.1.1"
            });
            var computer = dc.GetComputer();
            Console.WriteLine("{0} - {1}", computer.Name, computer.Enviroment);
            Console.WriteLine();
            dc.SaveAccount(new Account
            {
                ComputerId = 1,
                Domain = "WORKGROUP",
                Username = "Rizowski",
                Tracking = true
            });
            var accounts = dc.GetAccounts();
            var account = accounts.First(a => a.Id == 1);
            Console.WriteLine("{0} - {1}", account.Username, account.Tracking);
            Console.WriteLine();
            dc.SaveHistory(new History
            {
                AccountId = 1,
                Title = "Facebook",
                Domain = "Facebook.com",
                Url = "/Rizowski",
                VisitCount = 1
            });
            var histories = dc.GetHistoryForAccount(1);
            foreach (var history in histories)
            {
                Console.WriteLine("{0} - {1}", history.Title, history.Domain);
            }
            Console.WriteLine();
            dc.SaveProcess(new Process
                {
                    AccountId = 1,
                    Name = "System32.exe"
                });
            dc.SaveProgram(new Program
                {
                    AccountId = 1,
                    Name = "Visual Studio 2012",
                    OpenCount = 1
                });*/
            
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
            var users = ComputerManager.GetUsers();
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

        public static void REST2()
        {
            var server = "http://localhost:3000";
            var api = "api/v1/";
            string token = "AiprpscAqN6hnvNDHSwh";

            RestUser u = new RestUser(server, api);
            User user = new User
                {
                    Id = 1,
                    Username = "Rizowski",
                    /*Computers = new List<Computer>
                        {
                            new Computer{
                                Id = 2,
                                UserId = 1, 
                                Name = "Desktop", 
                                Enviroment = "Windows 8", 
                                IpAddress = "192.168.1.10"}
                        },*/
                    Email = "crouska@gmail.com"
                };
            u.Update(token, user);

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

        public static void Cylce()
        {
            MainService ms = new MainService();
            ms.testing();
        }
        public static void RunGUIProgram()
        {
            MainService s = new MainService();
        }


    }
}
