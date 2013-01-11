﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class Program
    {
        static  void Main(string[] args)
        {
            /*Console.WriteLine("History:");
            RunHistory();
            Console.WriteLine("\nProcesses:");
            RunProcesses();
            Console.WriteLine("\nPrograms:");
            RunPrograms();*/
            Console.WriteLine("\nProxy Running...");
            RunProxy();
            Console.Read();
        }

        public static void RunHistory()
        {
            History his = new History();
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
            Console.WriteLine("Admin: {0} ", record.IsAdmin());
            Console.WriteLine();
            Console.WriteLine(record.GetProcessesByUserv2());
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
