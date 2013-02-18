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

        public IEnumerable<Program> GetRunningPrograms()
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
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };
                    list.Add(program);
                }
            }
            return list;
        }
    }
}
