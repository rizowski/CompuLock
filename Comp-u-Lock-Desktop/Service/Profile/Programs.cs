using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Service.Profile
{
    public class Programs
    {
        public Programs()
        {
            
        }

        public List<Database.Models.Program> GetRunningPrograms()
        {
            Process[] processlist = Process.GetProcesses();
            List<Database.Models.Program> list = new List<Database.Models.Program>();
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    var program = new Database.Models.Program
                        {
                            Name = process.ProcessName,
                            LastRun = DateTime.Now
                        };
                    list.Add(program);
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                }
            }
            return list;
        }
    }
}
