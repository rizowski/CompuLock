using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Timers;
using Cassia;
using Database;
using Database.Models;
using Process = System.Diagnostics.Process;

namespace Service.Profile
{
    public class ProcessManager
    {
        private const int Domain = 1;
        private const int Username = 0;
        private DatabaseManager DbManager;

        private Timer UpdateTimer;

        private ITerminalServicesManager manager;
        public ProcessManager()
        {
            Console.WriteLine("Process Manager Started");

            manager = new TerminalServicesManager();

            UpdateTimer = new Timer(5000){AutoReset = true};
            UpdateTimer.Elapsed += Update;
            UpdateTimer.Start();

            DbManager = new DatabaseManager("settings", "");
        }

        private void Update(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Console.WriteLine("Process Check");
            using ( var server = manager.GetLocalServer())
            {
                server.Open();
                var processes = server.GetProcesses();
                foreach (var compProcess in processes)
                {
                    var owner = GetProcessOwner(compProcess.SessionId);
                    if (owner == null) continue;
                    if (!owner.Tracking) continue;

                    var ownerProcesses = DbManager.GetProcessesByAccountId(owner.WebId);
                    var found = ownerProcesses.FirstOrDefault(p => p.Name == compProcess.ProcessName);
                    if (found == null)
                    {
                        DbManager.SaveProcess(new Database.Models.Process
                            {
                                AccountId = owner.WebId,
                                Name = compProcess.ProcessName,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            });
                    }
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
                                Console.WriteLine("{0}\\{1} - {2}", outParameters[Domain], outParameters[Username],
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
                            Console.WriteLine("{0}\\{1} - {2}", outParameters[Domain], outParameters[Username],
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
                            Console.WriteLine("{0}\\{1} - {2}", outParameters[Domain], outParameters[Username],
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
                    var owner = obj.InvokeMethod("GetOwner", argList);
                    int returnVal = Convert.ToInt32(owner);
                    if (returnVal == 0)
                    {
                        return DbManager.GetAccountByName(argList[0]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            searcher.Dispose();

            return null;
        }
    }
}
