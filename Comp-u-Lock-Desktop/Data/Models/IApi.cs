using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Models
{
    public interface IApi
    {
        string GetToken(string username, string password);

        User GetUser(string token);

        IEnumerable<Computer> GetComputers(string token);

        IEnumerable<Account> GetAccounts(string token);

        Account GetAccountById(string token, int accountId);

        Computer GetComputerById(string token, int computerId);

        IEnumerable<AccountHistory> GetHistory(string token, int accountId);

        IEnumerable<AccountProcess> GetProcesses(string token, int accountId);

        IEnumerable<AccountProgram> GetPrograms(string token, int accountId);

        void UpdateUser(string token, User user);

        void UpdateComputer(string token, Computer computer);

        void UpdateAcount(string token, Account account);

        void CreateComputer(string token, Computer computer);

        void CreateAccount(string token, Account account);

        void CreateHistory(string token, int accountId, AccountHistory history);

        void CreateProgram(string token, int accountId, AccountProgram program);

        void CreateProcess(string token, int accountId, AccountProcess process);

        void DeleteComputer(string token, int id);

        void DeleteAccount(string token, int id);

        //IEnumberable<Account> GetAllTrackedAccounts(string token);
    }
}
