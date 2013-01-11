using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Diagnostics;
using System.Management;

namespace Services
{
    public class Processes
    {
        private const int DOMAIN = 1;
        private const int USERNAME = 0;
        private const string GENERAL_KEY = "general";

        public Processes()
        {

        }

        public Process[] GetProcesses()
        {
            Process[] all = Process.GetProcesses();
            Console.WriteLine(all.Count());
            return all;
        }

        //not sure why I had this method.
        /*public Boolean IsAdmin()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal princ = new WindowsPrincipal(user);
            var isadmin = princ.IsInRole(WindowsBuiltInRole.Administrator);
            return isadmin;
        }*/

        // http://msdn.microsoft.com/library/windows/desktop/aa394372.aspx
        public List<ManagementObject> GetProcessesByUser(string user = null)
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
