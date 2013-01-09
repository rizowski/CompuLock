﻿using System;
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
                        Console.WriteLine("{0}\\{1} - {2}",outParameters[1], outParameters[0], process["Name"]);

                        // Use process data...
                    }
                    else
                    {
                        Console.WriteLine("There is no owner for {0}", (string)process["Name"]);
                    }
                }
            return null;
        }
    }
}
