using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Database.Enviroment;
using Database.Models;
using REST;
using RestSharp;
using Service.Profile;

namespace Service
{
    public class RunningProgram
    {
        static  void Main(string[] args)
        {
            /*Console.WriteLine("\nRun Program");
            RunGUIProgram();*/
            //Console.WriteLine("\nCycle");
            //Cylce();

           /* Console.WriteLine("\nRun DbAccounts");
            try
            {
                RunDbAccounts();
            }
            catch (Exception e )
            {
                Console.WriteLine(e);
            }*/

            /*Console.WriteLine("\nRun ComputerManager");
            RunComputerManager();*/

            /*Console.WriteLine("\nRun Process manager");
            RunProcessManager();*/

            /*Console.WriteLine("\nRun ProgramManager");
            RunProgramManager();*/

            //TODO PRESENT
            /*Console.WriteLine("Test SQLite Injection");
            TestSqlInjection();*/

            /*Console.WriteLine("\nRun Rest");
            RunREST();*/

            /*Console.WriteLine("\nRun InfoGatherer");
            RunInfoGatherer();*/

            Console.WriteLine("Runing Program");
            MainServicer();
            Console.Read();

        }

        private static void RunDbAccounts()
        {

            /*DatabaseClient dc = new DatabaseClient("settings", "myPass");*/
            /*dc.SendUser(new User
            {
                AuthToken = "MyAuthToken",
                Email = "crouska@gmail.com",
                Username = "Rizowski"
            });
            var user = dc.GetUser();
            Console.WriteLine("{0} - {1}", user.Email, user.AuthToken);
            Console.WriteLine();
            dc.CreateComputer(new Computer
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

        }

        public static void RunOS()
        {
            Console.WriteLine(Environment.OSVersion.Version);
            Console.WriteLine(OS.Version);
        }

        public static void RunComputerManager()
        {
            ComputerManager cm = new ComputerManager();
            var accounts = cm.GetAccounts();
            var computer = cm.GetComputer();
            Console.WriteLine("{0} - {1} |Address: {2}",computer.Enviroment, computer.Name, computer.IpAddress);
            foreach (var account in accounts)
            {
                Console.WriteLine("{0} - {1}", account.Domain, account.Username);
            }
        }

        public static void RunProcessManager()
        {
            ProcessManager pm = new ProcessManager();
            var processes = pm.GetAllProcesses();
            Console.WriteLine("All processes");
            foreach (var process in processes)
            {
                Console.WriteLine("{0} - {1}", process.AccountId, process.Name);
            }
            Console.WriteLine();
            Console.Read();
            Console.WriteLine("All Account processes");
            var account = new Account
                {
                    Id = 1,
                    Username = "Rizowski"
                };
            var userProcesses = pm.GetProcessesByUser(account);
            foreach (var userProcess in userProcesses)
            {
                Console.WriteLine("{0} - {1}",userProcess.AccountId, userProcess.Name);
            }
            Console.WriteLine();
            Console.Read();
            Console.WriteLine("No Owner");
            var nos = pm.GetAllProcessesWithNoOwner();
            foreach (var process in nos)
            {
                Console.WriteLine("{0} - {1}", process.AccountId, process.Name);
            }
        }


        public static void RunInfoGatherer()
        {
            var authToken = "DNs7qtsM4HaNzJbyzH2N";
            var server = "http://localhost:3000";
            var api = "api/v1/";
            if(File.Exists("settings.sqlite"))
                File.Delete("settings.sqlite");
            DatabaseManager dc = new DatabaseManager("settings", "myPass");

            User user = new User
                {
                    AuthToken = authToken
                };
            RestService rs = new RestService(server, api);

            user = rs.GetUser(user.AuthToken);
            if (user == null)
                Console.WriteLine("Error with User");
            try
            {
                dc.SaveUser(user);
                Console.WriteLine("User Saved");
            }
            catch (Exception e)
            {
                Console.WriteLine("Save User:");
                Console.WriteLine(e);
            }

            ComputerManager cm = new ComputerManager();
            var computer = cm.GetComputer();
            try
            {
                computer = dc.SaveComputer(computer);
                Console.WriteLine("Computer Saved.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Save Computer:");
                Console.WriteLine(e);
            }
            
            var accounts = cm.GetAccounts();
            foreach (var account in accounts)
            {
                account.ComputerId = computer.Id;
                try
                {
                    dc.SaveAccount(account);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Save Account");
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Information Gathered:");
            Console.WriteLine("Web Account:\nEmail: {0}\nAuth Token: {1}", user.Email, user.AuthToken);
            Console.WriteLine("\nComputer:\nName: {0}\n", computer.Name);
            Console.WriteLine("Accounts({0}):", accounts.Count);
            foreach (var account in accounts)
            {
                Console.WriteLine("Username: {0}\nTracking: {1}\n", account.Username, account.Tracking);
            }
            Console.WriteLine("Press any key to save computer data to web.");
            computer.UserId = user.WebId;
            computer = rs.CreateComputer(authToken, computer);
            Console.WriteLine("Press any key to save Accounts to web.");
            foreach (var account in accounts)
            {
                account.ComputerId = computer.WebId;
                rs.SaveAccount(authToken, account);
            }
        }

        public static void RunGUIProgram()
        {
            MainService s = new MainService();
        }

        public static void RunREST()
        {
            //Local Token
            var local = "DNs7qtsM4HaNzJbyzH2N";
            //Service token
            var ser = "qT7qVKqWYrssubigpgXw";
            RestService rs = new RestService("http://localhost:3000", "api/v1/");

            var user = rs.GetUser(ser);

            var computer = user.Computers.First();
            var account = computer.Accounts.First(a => a.WebId == 2);

            //TODO Make because broken history now with computer
            /*account.Programs.Add(new Program
                {
                    AccountId = account.WebId,
                    Name = "Visual Studio 2012"
                });
            account.Processes = new List<Process>();
            account.Processes.Add(new Process
                {
                    AccountId = account.WebId,
                    Name = "Chrome"
                });
            account.Histories.Add(new History
                {
                    AccountId = account.WebId,
                    Domain = "MySpace.com",
                    Title = "MySpace"
                });*/
            rs.UpdateUser(ser, user);
            //RunInfoGatherer();
        }

        public static void MainServicer()
        {
            MainService s = new MainService();
            s.SaveHistory();
        }

    }
}