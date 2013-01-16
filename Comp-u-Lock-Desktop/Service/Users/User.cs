using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace Service.Users
{
    class UserAccount
    {
        public long TimeLimit;
        public string UserName;
        public string Domain;


        public UserAccount(string domain, string username, long timelimit)
        {
            Domain = domain;
            UserName = username;
            TimeLimit = timelimit;
        }

        public void ChangeUserPassword()
        {
            
        }

        public void IsCorrectPassword(string domain=null, string username=null, string password=null)
        {
            PrincipalContext pc = new PrincipalContext(ContextType.Domain);
                // validate the credentials
                bool isValid = pc.ValidateCredentials(username, password);
                Console.WriteLine(isValid);
        }
    }
}
