using System.Collections.Generic;
using System.Linq;
using Database;
using Database.Models;

namespace Service.Db
{
    public class DatabaseClient : DatabaseManager
    {

        public DatabaseClient(string path, string pass):base(path, pass)
        {

        }

        #region ById
        public Account GetAccountById(int id)
        {
            return GetAccounts().First(a => a.Id == id);
        }

        public History GetHistoryById(int id)
        {
            return GetHistories().First(h => h.Id == id);
        }

        public Process GetProcessById(int id)
        {
            return GetProcesses().First(p => p.Id == id);
        }

        public Program GetProgramById(int id)
        {
            return GetPrograms().First(p => p.Id == id);
        }
        #endregion

        #region ByAccountId
        public IEnumerable<History> GetHistoriesByAccountId(int accountId)
        {
            return GetHistories().Where(h => h.AccountId == accountId);
        } 

        public IEnumerable<Program> GetProgramsByAccountId(int accountId)
        {
            return GetPrograms().Where(p => p.AccountId == accountId);
        } 

        public IEnumerable<Process> GetProcessesByAccountId(int accountId)
        {
            return GetProcesses().Where(p => p.AccountId == accountId);
        }
        #endregion
    }
}
