using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Database.Models;

namespace Database
{
    public class DatabaseManager
    {
        public string DbPath { get; set; }
        public SQLiteConnection DbConnection { get; set; }
        private string DbPassword;

        private const string UsersTable = "Users";
        private const string ComputersTable = "Computers";
        private const string AccountsTable = "Accounts";

        private const string HistoryTable = "account_history";
        private const string ProcessTable = "account_process";
        private const string ProgramTable = "account_program";

        private const string CreateTable = "create table ";
        private const string InsertInto = "insert into ";
        private const string Values = " values (";
        private const string End = ");";
        public DatabaseManager(string path, string pass)
        {
            DbPassword = pass;
            var DbPath = path + ".sqlite";
            if (!File.Exists(DbPath))
            {
                Console.WriteLine("File doesn't exist");
                SQLiteConnection.CreateFile(DbPath);
                DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");
                DbConnection.Open();
                //Console.WriteLine("Setting Password to {0}", DbPassword);
                //DbConnection.ChangePassword(pass);
                DbConnection.Close();
                //DbConnection.SetPassword(pass);
                CreateTables();
            }
            else
            {
                DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");//Password="+DbPassword+";");
            }
        }

        public SQLiteConnection Connect(string pass)
        {
            //DbConnection.SetPassword(pass);
            return DbConnection.OpenAndReturn();
        }

        public void Disconnect()
        {
            DbConnection.Close();
            DbConnection.Dispose();
        }

        public void CreateTables()
        {
            Console.WriteLine("Creating tables");
            const string user = CreateTable + UsersTable+"(Username varchar(255), Email varchar(255), AuthToken varchar(255), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(user);
            const string computer = CreateTable + ComputersTable+"(Id integer primary key asc, UserId integer, Enviroment varchar(50), Name varchar(50), IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(computer);
            const string account = CreateTable + AccountsTable+"(Id integer primary key asc, ComputerId integer, Domain varchar(50), Username varchar(50), Tracking integer, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(account);
            const string accountHistory = CreateTable + HistoryTable+"(Id integer primary key asc, AccountId integer, Domain varchar(150), Url varchar(300), VisitCount integer, CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(accountHistory);
            const string accountProcess = CreateTable + ProcessTable+"(Id integer primary key asc, AccountId integer, Name varchar(100), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(accountProcess);
            const string accountProgram = CreateTable + ProgramTable+"(Id integer primary key asc, AccountId integer, Name varchar(100), OpenCount integer, CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(accountProgram);
            Console.WriteLine("Done creating tables");
        }

        #region Insert

        public void SaveUser(User user)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a User.");
            sb.Append(InsertInto);
            sb.Append(UsersTable);
            sb.Append("(Username, Email, AuthToken, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}'", user.Username, user.Email, user.AuthToken, DateTime.Now,
                            DateTime.Now);
            sb.Append(End);
            Console.WriteLine("Sql line using: {0}", sb);
            ExecuteQuery(sb.ToString());
            Console.WriteLine("Done writing User.");
        }

        public void SaveComputer(Computer comp)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a computer.");
            sb.Append(InsertInto);
            sb.Append(ComputersTable);
            sb.Append("(UserId, Name, Enviroment, IpAddress, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            var now = DateTime.Now;
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}', '{5}'", comp.UserId, comp.Name, comp.Enviroment, comp.IpAddress, now,
                            now);
            sb.Append(End);
            Console.WriteLine("Sql line using: {0}", sb);
            ExecuteQuery(sb.ToString());
            Console.WriteLine("Done writing Computer.");
        }

        public void SaveAccount(Account account)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving an account.");
            sb.Append(InsertInto);
            sb.Append(AccountsTable);
            sb.Append("(ComputerId, Domain, Username, Tracking, AllottedTime, UsedTime, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            var now = DateTime.Now;
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'", account.ComputerId, account.Domain, account.Username, account.Tracking, account.AllottedTime, account.UsedTime, now, now);
            sb.Append(End);
            Console.WriteLine("Sql line using: {0}", sb);
            ExecuteQuery(sb.ToString());
            Console.WriteLine("Done writing Computer.");
        }

        public void SaveHistory(History history)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a History.");
            sb.Append(InsertInto);
            sb.Append(HistoryTable);
            sb.Append("(AccountId, Domain, Url, VisitCount, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            var now = DateTime.Now;
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}', '{5}'", history.AccountId, history.Domain, history.Url, history.VisitCount, now, now);
            sb.Append(End);
            Console.WriteLine("Sql line using: {0}", sb);
            ExecuteQuery(sb.ToString());
            Console.WriteLine("Done writing Computer.");
        }

        public void SaveProcess(Process process)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a History.");
            sb.Append(InsertInto);
            sb.Append(ProcessTable);
            sb.Append("(AccountId, Name, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            var now = DateTime.Now;
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}'", process.AccountId, process.Name, now, now);
            sb.Append(End);
            Console.WriteLine("Sql line using: {0}", sb);
            ExecuteQuery(sb.ToString());
            Console.WriteLine("Done writing Computer.");
        }

        public void SaveProgram(Program program)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a History.");
            sb.Append(InsertInto);
            sb.Append(ProgramTable);
            sb.Append("(AccountId, Name, OpenCount, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            var now = DateTime.Now;
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}'", program.AccountId, program.Name, program.OpenCount, now, now);
            sb.Append(End);
            Console.WriteLine("Sql line using: {0}", sb);
            ExecuteQuery(sb.ToString());
            Console.WriteLine("Done writing Computer.");
        }

        public void ExecuteQuery(string query)
        {
            var command = new SQLiteCommand(query, DbConnection);
            DbConnection.Open();
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        /*public void Save<T>(string table, T t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into ");
            sb.Append(table);
        }*/
        #endregion

        public User GetUserById(int id)
        {
            return null;
        }

        public Computer GetComputerById(int id)
        {
            return null;
        }

        public Account GetAccountById(int id)
        {
            return null;
        }

        public History GetHistoryById(int id)
        {
            return null;
        }

        public Process GetProcessById(int id)
        {
            return null;
        }

        public Program GetProgramById(int id)
        {
            return null;
        }

        public IEnumerable<T> GetAll<T>()
        {
            DbConnection.Open();
            var items = DbConnection.Database.OfType<T>();
            DbConnection.Close();
            return items;
        }
    }
}
