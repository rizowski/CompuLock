using System;
using System.Diagnostics;
using System.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;

namespace Test
{
    [TestClass]
    public class Service
    {
        [TestMethod]
        public void ProcessesByUser_OnlyReturnsUserProcesses()
        {
            int user = 0;
            string username = Environment.UserName;
            Processes proc = new Processes();

            var processes = proc.GetProcessesByUser(username);

            foreach (var process in processes)
            {
                object[] outParameters = new object[2];
                var response = (uint)process.InvokeMethod("GetOwner", outParameters);
                if(response != 0)
                    Assert.Fail("{0} process has no owner when expecting {1}", process["name"], username);
                Assert.AreEqual(username, outParameters[user]);
            }
        }

        [TestMethod]
        public void Programs_CorrectCountIsValidated()
        {
            Process[] processlist = Process.GetProcesses();
            Programs prog = new Programs();
            var list = prog.GetRunningPrograms();
            var testcount = 0;

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    testcount += 1;
                }
            }
            Assert.AreEqual(testcount, list.Count);
            
        }

        public void Proxy_RequestIsMadeAndServerRespondsRecorded()
        {
            
        }
    }
}
