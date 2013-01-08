using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Service
{
    class Record
    {
        static void Main(string[] args)
        {
            Process[] all = Process.GetProcesses();
            foreach (Process proc in all)
            {
                string name = proc.ProcessName;
                Console.WriteLine(name);
            }
            Console.Read();
        }
    }
}
