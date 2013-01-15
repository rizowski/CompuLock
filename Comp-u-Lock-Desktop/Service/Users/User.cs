using System;
using System.Collections.Generic;
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

        
    }
}
