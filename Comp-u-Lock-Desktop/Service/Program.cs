using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class Program
    {
        static  void Main(string[] args)
        {
            Console.WriteLine("Processes:");
            RunProcesses();
           /* Console.WriteLine("\nPrograms:");
            RunPrograms();*/
            /*Console.WriteLine("\nProxy Running...");
            RunProxy();*/
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

        public static void RunProxy()
        {
            HistoryProxy hp = new HistoryProxy(9095);
            hp.Start();
            while (true)
            {
                hp.Accept();
            }

        }
    }
}
