using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
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

        #region Constants
        private const string UsersTable = "Users";
        private const string ComputersTable = "Computers";
        private const string AccountsTable = "Accounts";

        private const string HistoryTable = "account_history";
        private const string ProcessTable = "account_process";
        private const string ProgramTable = "account_program";

        private const string CreateTable = "create table ";
        private const string InsertInto = "insert or ignore into ";
        private const string Select = "select ";
        private const string SelectAll = "select * from ";
        private const string All = "* from ";
        private const string From = "from ";
        private const string Where = " where ";

        private const string Values = " values ";
        private const string End = ";";
        #endregion

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
                Console.WriteLine("File Found");
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
            const string user = CreateTable + UsersTable + "(Username varchar(255), Email varchar(255), AuthToken varchar(255) unique on conflict replace, CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(user);
            const string computer = CreateTable + ComputersTable + "(Id integer primary key asc, UserId integer, Enviroment varchar(50), Name varchar(50) unique on conflict replace, IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(computer);
            const string account = CreateTable + AccountsTable + "(Id integer primary key asc, ComputerId integer, Domain varchar(50), Username varchar(50), Tracking bool, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime, unique(Domain, Username) on conflict replace)";
            ExecuteQuery(account);
            const string accountHistory = CreateTable + HistoryTable + "(Id integer primary key asc, AccountId integer, Title varchar(150), Domain varchar(150), Url varchar(300), VisitCount integer, CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Domain) on conflict replace)";
            ExecuteQuery(accountHistory);
            const string accountProcess = CreateTable + ProcessTable + "(Id integer primary key asc, AccountId integer, Name varchar(100), CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProcess);
            const string accountProgram = CreateTable + ProgramTable + "(Id integer primary key asc, AccountId integer, Name varchar(100), OpenCount integer, CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProgram);
            Console.WriteLine("Done creating tables");
        }
        public void ExecuteQuery(string query)
        {
            var command = new SQLiteCommand(query, DbConnection);
            DbConnection.Open();
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public Computer GetComputer()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(ComputersTable);
            sb.Append(End);
            DbConnection.Open();
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader(CommandBehavior.SingleResult);
            Computer comp = null;
            while (reader.Read())
            {
                comp = new Computer
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Enviroment = (string) reader["Enviroment"],
                        Name = (string) reader["Name"],
                        IpAddress = (string) reader["IpAddress"],
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    };
            }
            DbConnection.Close();
            return comp;
        }
        public User GetUser()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(UsersTable);
            sb.Append(End);
            DbConnection.Open();
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader(CommandBehavior.SingleResult);
            User user = null;
            while (reader.Read())
            {
                user = new User
                    {
                        Username = (string) reader["Username"],
                        Email = (string) reader["Email"],
                        AuthToken = (string) reader["AuthToken"],
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    };
            }
            DbConnection.Close();
            return user;
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
            sb.Append("(@username, @email, @authtoken, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@username", user.Username));
                command.Parameters.Add(new SQLiteParameter("@email", user.Email));
                command.Parameters.Add(new SQLiteParameter("@authtoken", user.AuthToken));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine("Sql line using: {0}", sb);
            command.ExecuteNonQuery();
            DbConnection.Close();
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
            sb.Append("(@userid, @name, @enviroment, @ipaddress, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@userid", comp.UserId));
                command.Parameters.Add(new SQLiteParameter("@name", comp.Name));
                command.Parameters.Add(new SQLiteParameter("@enviroment", comp.Enviroment));
                command.Parameters.Add(new SQLiteParameter("@ipaddress", comp.IpAddress));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine("Sql line using: {0}", sb);
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing Computer.");
        }

        public void SaveAccount(Account account)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving an account.");
            sb.Append(InsertInto);
            sb.Append(AccountsTable);
            sb.Append("(ComputerId, Domain, Username, Tracking, AllottedTime, UsedTime, CreatedAt, UpdatedAt) ");
            sb.Append(Values);
            sb.Append("(@computerId, @domain, @username, @tracking, @allottedTime, @usedTime, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@computerId", account.ComputerId));
                command.Parameters.Add(new SQLiteParameter("@domain", account.Domain));
                command.Parameters.Add(new SQLiteParameter("@username", account.Username));
                command.Parameters.Add(new SQLiteParameter("@tracking", account.Tracking));
                command.Parameters.Add(new SQLiteParameter("@allottedTime", account.AllottedTime));
                command.Parameters.Add(new SQLiteParameter("@usedTime", account.UsedTime));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine("Sql line using: {0}", sb);
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing Account.");
        }

        public void SaveHistory(History history)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a History.");
            sb.Append(InsertInto);
            sb.Append(HistoryTable);
            sb.Append("(AccountId, Title, Domain, Url, VisitCount, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            sb.Append("(@accountid, @title, @domain, @url, @visitcount, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@accountid", history.AccountId));
                command.Parameters.Add(new SQLiteParameter("@title", history.Title));
                command.Parameters.Add(new SQLiteParameter("@domain", history.Domain));
                command.Parameters.Add(new SQLiteParameter("@url", history.Url));
                command.Parameters.Add(new SQLiteParameter("@visitcount", history.VisitCount));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine("Sql line using: {0}", sb);
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing History.");
        }

        public void SaveProcess(Process process)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a Process.");
            sb.Append(InsertInto);
            sb.Append(ProcessTable);
            sb.Append("(AccountId, Name, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            sb.Append("(@accountid, @name, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@accountid", process.AccountId));
                command.Parameters.Add(new SQLiteParameter("@name", process.Name));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine("Sql line using: {0}", sb);
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing Process.");
        }

        public void SaveProgram(Program program)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a Program.");
            sb.Append(InsertInto);
            sb.Append(ProgramTable);
            sb.Append("(AccountId, Name, OpenCount, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            sb.Append("(@accountid, @name, @opencount, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@accountid", program.AccountId));
                command.Parameters.Add(new SQLiteParameter("@name", program.Name));
                command.Parameters.Add(new SQLiteParameter("@opencount", program.OpenCount));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine("Sql line using: {0}", sb);
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing Program.");
        }
        #endregion

        #region GetAll
        public IEnumerable<Account> GetAccounts()
        {
            List<Account> list = new List<Account>();
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(AccountsTable);
            DbConnection.Open();
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Account
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ComputerId = Convert.ToInt32(reader["ComputerId"]),
                        Domain = (string) reader["Domain"],
                        Username = (string) reader["Username"],
                        Tracking = (Convert.ToInt32(reader["Tracking"]) == 1),
                        AllottedTime = Convert.ToInt32(reader["AllottedTime"]),
                        UsedTime = Convert.ToInt32(reader["UsedTime"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    });
                
                
            }
            DbConnection.Close();
            return list;
        }
        #endregion
        #region GetById
        public IEnumerable<History> GetHistoryForAccount(int accountid)
        {
            List<History> list  = new List<History>();
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(HistoryTable);
            sb.Append(Where);
            sb.Append("AccountId ='" + accountid + "'");
            sb.Append(End);
            DbConnection.Open();
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new History
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        AccountId = Convert.ToInt32(reader["AccountId"]),
                        Title = (string) reader["Title"],
                        Domain = (string) reader["Domain"],
                        Url = (string) reader["Url"],
                        VisitCount = Convert.ToInt32(reader["VisitCount"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    });
            }
            DbConnection.Close();
            return list;
        }

        public Process GetProcessById(int id)
        {
            throw new NotImplementedException();
        }

        public Program GetProgramById(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FindByName

        #endregion

    }
}
