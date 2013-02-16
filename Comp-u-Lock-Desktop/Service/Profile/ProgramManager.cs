using System;
using System.Collections.Generic;
using Database.Models;
using Process = System.Diagnostics.Process;

namespace Service.Profile
{
    public class ProgramManager
    {
        public ProgramManager()
        {
            
        }

        public List<Program> GetRunningPrograms()
        {
            Process[] processlist = Process.GetProcesses();
            List<Program> list = new List<Program>();
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    var program = new Program
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
