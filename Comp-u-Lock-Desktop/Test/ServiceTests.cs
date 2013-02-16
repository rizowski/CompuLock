using System;
using System.Diagnostics;
using Database.Enviroment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Profile;

namespace Test
{
    [TestClass]
    public class Service
    {
        

        

        [TestMethod]
        public void IEBrowser_IsNotRunningShouldBeTrue()
        {
            //Process[] procs = Process.GetProcessesByName("iexplore");
            //InternetExplorer ie = new InternetExplorer();
            //Assert.AreEqual((procs.Length !=0),ie.IsRunning());
        }

        [TestMethod]
        public void ChromeBrowser_IsNotRunningShouldBeTrue()
        {
            //Process[] procs = Process.GetProcessesByName("firefox");
            //Firefox f = new Firefox();
            //Assert.AreEqual((procs.Length != 0), f.IsRunning());
        }

        [TestMethod]
        public void FirefoxBrowser_IsNotRunningShouldBeTrue()
        {
            //Process[] procs = Process.GetProcessesByName("chrome");
            //Chrome c = new Chrome();
            //Assert.AreEqual((procs.Length != 0), c.IsRunning());
        }

        public void IEBrowser_IsInstalled()
        {

        }

        public void FirefoxBrowser_IsInstalled()
        {
            
        }

        public void ChromeBrowser_IsInstalled()
        {

        }

        [TestMethod]
        public void OS_GetsCorrectOSVerion()
        {
            Assert.AreEqual(Environment.OSVersion.Version, OS.Version);
        }

        [TestMethod]
        public void OS_GetsCorrectOSName()
        {
            Assert.AreEqual(Windows.Eight, OS.Name);
        }
    }
}
