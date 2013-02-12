using System.Collections.Generic;
using Data.Models;

namespace Data.JSON.Models
{
    public interface IApi
    {
        string GetToken(string username, string password);

        User GetUser(string token);

        IEnumerable<Computer> GetComputers(string token);

        IEnumerable<Account> GetAccounts(string token);

        Account GetAccountById(string token, int accountId);

        Computer GetComputerById(string token, int computerId);

        IEnumerable<History> GetHistory(string token, int accountId);

        IEnumerable<Process> GetProcesses(string token, int accountId);

        IEnumerable<Program> GetPrograms(string token, int accountId);

        void UpdateUser(string token, User user);

        void UpdateComputer(string token, Computer computer);

        void UpdateAcount(string token, Account account);

        void CreateComputer(string token, Computer computer);

        void CreateAccount(string token, Account account);

        void CreateHistory(string token, int accountId, History history);

        void CreateProgram(string token, int accountId, Program program);

        void CreateProcess(string token, int accountId, Process process);

        void DeleteComputer(string token, int id);

        void DeleteAccount(string token, int id);

        //IEnumberable<Account> GetAllTrackedAccounts(string token);
    }
}
