using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;

namespace Service.DataService
{
    class AccountService : Service<Account, DatabaseEntities>
    {
        public IQueryable<Account> Search(string query)
        {
            var searchResults = All().Where(a => a.Username.Contains(query));
            return searchResults;
        } 
    }
}
