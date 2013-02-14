using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Management;

namespace Service.Profile
{
    public class Processes
    {
        private const int DOMAIN = 1;
        private const int USERNAME = 0;

        public Processes()
        {
            
        }

        public Process[] GetProcesses()
        {
            Process[] all = Process.GetProcesses();
            Console.WriteLine(all.Count());
            return all;
        }

        public IEnumerable<ManagementObject> GetProcessesByUser(string user = null)
        {
            string[] propertiesToSelect = new[] { "Handle", "ProcessId", "Name" };
            SelectQuery processQuery = new SelectQuery("Win32_Process", null, propertiesToSelect);

            Dictionary<string, List<ManagementObject>> store = new Dictionary<string, List<ManagementObject>>();
            List<ManagementObject> processies = new List<ManagementObject>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            using (ManagementObjectCollection processes = searcher.Get())
                foreach (ManagementObject process in processes)
                {
                    object[] outParameters = new object[3];
                    //using reflection
                    uint result = (uint)process.InvokeMethod("GetOwner", outParameters);

                    if (result == 0)
                    {
                        if (user == null) // prints all
                        {
                            #if (DEBUG)
                                Console.WriteLine("{0}\\{1} - {2}", outParameters[DOMAIN], outParameters[USERNAME],
                                              process["Name"]);
                            #endif
                            processies.Add(process);
                        }
                        else if (outParameters[USERNAME].Equals(user)) // prints user processes
                        {
                            #if (DEBUG)
                                Console.WriteLine("{0}\\{1} - {2}", outParameters[DOMAIN], outParameters[USERNAME],
                                              process["Name"]);
                            #endif
                            processies.Add(process);
                        }
                    }
                    else
                    {
                        // TODO doesn't need to print
                        #if (DEBUG)
                            Console.WriteLine("There is no owner for {0}", process["Name"]);
                        #endif
                    }
                }
            return processies;
        }
    }
}
