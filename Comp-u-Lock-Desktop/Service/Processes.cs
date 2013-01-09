using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Diagnostics;
using System.Management;

namespace Service
{
    class Processes
    {
        static void Main(string[] args)
        {
            
        }

        public Process[] GetProcesses()
        {
            Process[] all = Process.GetProcesses();//.Where(p => p.);
            Console.WriteLine(all.Count());
            return all;
        }

        public Boolean IsAdmin()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal princ = new WindowsPrincipal(user);
            var isadmin = princ.IsInRole(WindowsBuiltInRole.Administrator);
            return isadmin;
        }

        public string GetProcessesByUserv2()
        {
            string[] propertiesToSelect = new[] { "Handle", "ProcessId", "Name" };
            SelectQuery processQuery = new SelectQuery("Win32_Process", null, propertiesToSelect);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            using (ManagementObjectCollection processes = searcher.Get())
                foreach (ManagementObject process in processes)
                {
                    object[] outParameters = new object[2];
                    uint result = (uint)process.InvokeMethod("GetOwner", outParameters);

                    if (result == 0)
                    {
                        Console.WriteLine("{0}\\{1} - {2}",(string)outParameters[1],(string)outParameters[0],(string)process["Name"]);

                        // Use process data...
                    }
                    else
                    {
                        Console.WriteLine("There is no owner for {0}", (string)process["Name"]);
                    }
                }
            return null;
        }

        

        public void RunHistory()
        {
            History his = new History();
            
        }

        public void RunProcesses()
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
            Console.Read();
        }

        public void RunPrograms()
        {
            Programs pro = new Programs();
            pro.GetRunningPrograms();
        }
    }
}
