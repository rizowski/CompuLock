using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Service.Users
{
    public class Programs
    {
        public Programs()
        {
            
        }

        public List<Process> GetRunningPrograms()
        {
            Process[] processlist = Process.GetProcesses();
            List<Process> list = new List<Process>();
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    list.Add(process);
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                }
            }
            return list;
        }

        /*public void GetRunningProgramsPerUser()
        {
            
        }*/
    }
}
