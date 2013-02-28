using System;
using System.IO;
using Database;
using Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class DataTests
    {
        private DatabaseManager DbManager;
        public DataTests()
        {
            DbManager = new DatabaseManager("test","nopass");
        }
        /*[TestMethod]
        public void TestMethod1()
        {
        }*/

        [TestCleanup]
        public void Cleanup()
        {
            if(File.Exists("test.sqlite"))
                File.Delete("test.sqlite");
        }
        [TestMethod]
        public void SavingAccount()
        {
            var name = "test";
            
            var start = DbManager.SaveAccount(new Account
                {
                    CreatedAt = DateTime.Now,
                    Username = name,
                    Domain = "Domain",
                    Admin = false,
                    AllottedTime = TimeSpan.FromSeconds(342546546),
                    UpdatedAt = DateTime.Now
                });
            var savedAccount = DbManager.GetAccountByName(name);
            Assert.AreEqual(start, savedAccount);
        }
    }
}
