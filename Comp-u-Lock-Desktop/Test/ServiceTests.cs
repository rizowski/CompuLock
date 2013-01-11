using System;
using System.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;

namespace Test
{
    [TestClass]
    public class Service
    {
        [TestMethod]
        public void PrcessesByUser_OnlyReturnsUserProcesses()
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
    }
}
