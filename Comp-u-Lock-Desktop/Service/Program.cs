using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Data;

namespace Service
{
    public class Program
    {
        static  void Main(string[] args)
        {
            /*Console.WriteLine("History:");
            RunHistory();*/
            Console.WriteLine("\nSQLiteHistory");
            GetSQLiteHistory();
            /*Console.WriteLine("\nProcesses:");
            RunProcesses();*/
           /* Console.WriteLine("\nPrograms:");
            RunPrograms();*/
            Console.Read();

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
            var ie = new InternetExplorer();
            Console.WriteLine(ie.Version);
            Console.WriteLine(ie.IsRunning());
            ie.GetHistory();

            Console.WriteLine("Firefox");
            var fire = new Firefox();
            Console.WriteLine(fire.Version);
            Console.WriteLine(fire.IsRunning());

            Console.WriteLine("Chrome");
            var chrome = new Chrome();
            Console.WriteLine(chrome.Version);
            Console.WriteLine(chrome.IsRunning());

        }

        public static void GetSQLiteHistory()
        {
            /*Firefox f = new Firefox();
            FirefoxHistoryReader fh = new FirefoxHistoryReader(f.HistoryPath.AbsolutePath + @"\3fcwvrqq.default\","places.sqlite");
            fh.Connect();
            fh.GetHistory();*/

            Chrome c = new Chrome();
            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
            ChromeHistoryReader ch = new ChromeHistoryReader(c.HistoryPath, "History");
            ch.Connect();
            ch.GetHistory();
        }

    }
}
