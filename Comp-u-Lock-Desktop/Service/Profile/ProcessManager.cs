using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Timers;
using Database;
using Database.Models;
using Process = System.Diagnostics.Process;

namespace Service.Profile
{
    public class ProcessManager : IDisposable
    {
        private const int DOMAIN = 1;
        private const int USERNAME = 0;
        private DatabaseManager DbManager;
        private ManagementEventWatcher startWatch;

        public ProcessManager(DatabaseManager dbManager)
        {
            startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            try
            {
                startWatch.EventArrived += Update;
                startWatch.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            DbManager = dbManager;
        }

        public void Dispose()
        {
            startWatch.Dispose();
        }

        private void Update(object sender, EventArrivedEventArgs e)
        {
            var userAccount = GetProcessOwner(Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value));
            if (userAccount != null)
            {
                if (userAccount.Tracking)
                {
                    var name = e.NewEvent.Properties["ProcessName"].Value;
                    Console.WriteLine("Adding Process {0} for {1}", name, userAccount.Username);
                    DbManager.SaveProcess(new Database.Models.Process
                        {
                            AccountId = userAccount.Id,
                            Name = Convert.ToString(name)
                        });
                }
            }
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

        public Account GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                try
                {
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {
                        return DbManager.GetAccountByName(argList[0]);
                        //return argList[1] + "\\" + argList[0];
                    }
                }
                catch (Exception)
                {
                }
            }
            searcher.Dispose();

            return null;
        }
    }
}
