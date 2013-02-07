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

        List<Computer> GetComputers(string token);

        List<Account> GetAccounts(string token, int computerId);

        List<AccountHistory> Histories(string token, int accountId);

        List<AccountProcess> GetProcesses(string token, int accountId);

        List<AccountProgram> GetPrograms(string token, int accountId);

        void UpdateUser(string token, User user);

        void UpdateComputer(string token, Computer computer);

        void UpdateAcount(string token, Account account);

        void WriteComputer(string token, Computer computer);

        void WriteAccount(string token, int computerId, Account account);

        void WriteHistory(string token, int accountId, AccountHistory history);

        void WriteProgram(string token, int accountId, AccountProgram program);

        void WriteProcess(string token, int accountId, AccountProcess process);
    }
}
