using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Database.Models;
using Process = System.Diagnostics.Process;

namespace Service.Profile
{
    public class ProcessManager
    {
        private const int DOMAIN = 1;
        private const int USERNAME = 0;

        public ProcessManager()
        {
            
        }

        public Process[] GetProcesses()
        {
            Process[] all = Process.GetProcesses();
            Console.WriteLine(all.Count());
            return all;
        }

        public IEnumerable<Database.Models.Process> GetProcessesByUser(Account account)
        {
            string[] propertiesToSelect = new[] { "Handle", "ProcessId", "Name" };
            SelectQuery processQuery = new SelectQuery("Win32_Process", null, propertiesToSelect);

            List<Database.Models.Process> proceses = new List<Database.Models.Process>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            {
                using (ManagementObjectCollection processes = searcher.Get())
                {
                    foreach (ManagementObject process in processes)
                    {
                        object[] outParameters = new object[3];
                        //using reflection
                        uint result = (uint) process.InvokeMethod("GetOwner", outParameters);

                        if (result == 0)
                        {
                            if (account.Username != null)
                            {
#if (DEBUG)
                                Console.WriteLine("{0}\\{1} - {2}", outParameters[DOMAIN], outParameters[USERNAME],
                                                  process["Name"]);
#endif
                                proceses.Add(new Database.Models.Process
                                {
                                    Name = (string)process["Name"],
                                    AccountId = account.Id,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            return proceses;
        }

        public IEnumerable<Database.Models.Process> GetAllProcesses()
        {
            string[] propertiesToSelect = new[] { "Handle", "ProcessId", "Name" };
            SelectQuery processQuery = new SelectQuery("Win32_Process", null, propertiesToSelect);

            List<Database.Models.Process> proceses = new List<Database.Models.Process>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            {
                using (ManagementObjectCollection processes = searcher.Get())
                {
                    foreach (ManagementObject process in processes)
                    {
                        object[] outParameters = new object[3];
                        uint result = (uint) process.InvokeMethod("GetOwner", outParameters);

                        if (result == 0)
                        {
#if (DEBUG)
                            Console.WriteLine("{0}\\{1} - {2}", outParameters[DOMAIN], outParameters[USERNAME],
                                              process["Name"]);
#endif
                            //TODO Search for Account by name adding ID
                            proceses.Add(new Database.Models.Process
                                {
                                    Name = (string) process["Name"],
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                });
                        }
                    }
                }
            }
            return proceses;
        }

        public IEnumerable<Database.Models.Process> GetAllProcessesWithNoOwner()
        {
            string[] propertiesToSelect = new[] { "Handle", "ProcessId", "Name" };
            SelectQuery processQuery = new SelectQuery("Win32_Process", null, propertiesToSelect);

            List<Database.Models.Process> proceses = new List<Database.Models.Process>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            {
                using (ManagementObjectCollection processes = searcher.Get())
                {
                    foreach (ManagementObject process in processes)
                    {
                        object[] outParameters = new object[3];
                        //using reflection
                        uint result = (uint) process.InvokeMethod("GetOwner", outParameters);

                        if (result != 0)
                        {
#if (DEBUG)
                            Console.WriteLine("{0}\\{1} - {2}", outParameters[DOMAIN], outParameters[USERNAME],
                                              process["Name"]);
#endif
                            proceses.Add(new Database.Models.Process
                                {
                                    Name = (string) process["Name"],
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                });
                        }
                    }
                }
            }
            return proceses;
        }
    }
}
