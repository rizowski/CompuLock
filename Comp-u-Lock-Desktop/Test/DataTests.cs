using System;
using System.Collections.Generic;
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
        [TestMethod]
        public void SavingAccount()
        {
            var name = "test";

            var account1 = new Account
                {
                    CreatedAt = DateTime.Now,
                    Username = name,
                    Domain = "Domain",
                    Admin = false,
                    AllottedTime = 342546546,
                    UpdatedAt = DateTime.Now
                };
            var account2 = new Account
                {
                    CreatedAt = DateTime.Now,
                    Username = "Test2",
                    Domain = "MyineDomain",
                    Admin = false,
                    AllottedTime = 0,
                    UpdatedAt = DateTime.Now
                };
            List<Account> list = new List<Account>();
            list.Add(account1);
            list.Add(account2);
            DbManager.SaveAccounts(1,list);
            var dbaccounts = (List<Account>)DbManager.GetAccounts();
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i], dbaccounts[i]);
            }
                Assert.AreEqual(list,dbaccounts);
        }
    }
}
