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
        public bool DbExists { get; set; }

        #region Constants
        private const string UsersTable = "Users";
        private const string ComputersTable = "Computers";
        private const string AccountsTable = "Accounts";

        private const string HistoryTable = "account_history";
        private const string ProcessTable = "account_process";
        private const string ProgramTable = "account_program";

        private const string RestrictionTable = "Restrictions";
        private const string DayTable = "Days";
        private const string HourTable = "Hours";

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
            DbExists = File.Exists(DbPath);
            if (File.Exists(DbPath))
            {
                DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");//Password="+DbPassword+";");
            }
            else
            {
                CreateDb();
                DbExists = File.Exists(DbPath);
            }
        }

        public void CreateDb()
        {
            SQLiteConnection.CreateFile(DbPath);
            DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");
            CreateTables();
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
            const string user = CreateTable + UsersTable + "(Id integer primary key asc, WebId integer, Username varchar(255), Email varchar(255), AuthToken varchar(255), CreatedAt datetime, UpdatedAt datetime, unique(Email, AuthToken) on conflict replace)";
            ExecuteQuery(user);
            Console.Write("Creating.");
            const string computer = CreateTable + ComputersTable + "(Id integer primary key asc, WebId integer, UserId integer, Enviroment varchar(50), Name varchar(50) unique on conflict replace, IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(computer);
            Console.Write(".");
            const string account = CreateTable + AccountsTable + "(Id integer primary key asc, WebId integer, ComputerId integer, Domain varchar(50), Username varchar(50), Tracking bool, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime, unique(Domain, Username) on conflict replace)";
            ExecuteQuery(account);
            Console.Write(".");
            const string accountHistory = CreateTable + HistoryTable + "(Id integer primary key asc, WebId integer, AccountId integer, Title varchar(150), Domain varchar(150), Url varchar(300), VisitCount integer, CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Domain, Url) on conflict replace)";
            ExecuteQuery(accountHistory);
            Console.Write(".");
            const string accountProcess = CreateTable + ProcessTable + "(Id integer primary key asc, WebId integer, AccountId integer, Name varchar(100), CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProcess);
            Console.Write(".");
            const string accountProgram = CreateTable + ProgramTable + "(Id integer primary key asc, WebId integer, AccountId integer, Name varchar(100), OpenCount integer, CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProgram);
            Console.Write(".");

            const string restriction = CreateTable + RestrictionTable + "(Id integer primary key asc, WebId integer, AccountId integer, unique(Id, AccountId) on conflict replace)";
            ExecuteQuery(restriction);
            Console.Write(".");
            const string day = CreateTable + DayTable + "(Id integer primary key asc, WebId integer, RestrictionId integer, unique(Id, RestrictionId) on conflict replace)";
            ExecuteQuery(day);
            Console.Write(".");
            const string hour = CreateTable + HourTable + "(Id integer primary key asc, WebId integer, DayId integer, unique(Id, DayId) on conflict replace)";
            ExecuteQuery(hour);
            Console.Write(".\n");

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
                        Id = Convert.ToInt32(reader["Id"]),
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
            SQLiteDataReader reader;
            User user = null;
            try
            {
                reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader(CommandBehavior.SingleResult);
                
                while (reader.Read())
                {
                    user = new User
                    {
                        Username = Convert.ToString(reader["Username"]),
                        Email = Convert.ToString(reader["Email"]),
                        AuthToken = Convert.ToString(reader["AuthToken"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            DbConnection.Close();
            return user;
        }

        #region Insert

        public User SaveUser(User user)
        {
            if(user == null)
                throw new NoNullAllowedException("User can't be null");
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
            Console.WriteLine("Getting user.");
            return GetUser();
        }

        public Computer SaveComputer(Computer comp)
        {
            if (comp == null)
                throw new NoNullAllowedException("Computer can't be null");
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
            Console.WriteLine("Getting Computer");
            return GetComputer();
        }

        public Account SaveAccount(Account account)
        {
            if (account == null)
                throw new NoNullAllowedException("Account can't be null");
            if (account.ComputerId <= 0)
                throw new ArgumentException("Computer Id is required.");
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
            Console.WriteLine("Getting Account.");
            var savedAccount = GetAccounts().First(a => a.Username == account.Username);
            return savedAccount;
        }

        public History SaveHistory(History history)
        {
            if (history == null)
                throw new NoNullAllowedException("History Object can't be null");
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
            var savedHistory = GetHistories().First(h => h.AccountId == history.AccountId && h.Domain == history.Domain);
            return savedHistory;
        }

        public Process SaveProcess(Process process)
        {
            if (process == null)
                throw new NoNullAllowedException("Process object can't be null");
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
            Console.WriteLine("Getting Process");
            var savedProcess = GetProcesses().First(p => p.AccountId == process.AccountId && p.Name == process.Name);
            return savedProcess;
        }

        public Program SaveProgram(Program program)
        {
            if (program == null)
                throw new NoNullAllowedException("Program object can't be null");
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
            Console.WriteLine("Getting Program");
            var savedProgram = GetPrograms().First(p => p.AccountId == program.AccountId && p.Name == program.Name);
            return savedProgram;
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
                        AllottedTime = TimeSpan.FromSeconds(Convert.ToDouble(reader["AllottedTime"])),
                        UsedTime = TimeSpan.FromSeconds(Convert.ToDouble(reader["UsedTime"])),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    });
            }
            DbConnection.Close();
            return list;
        }
        public IEnumerable<History> GetHistories()
        {
            List<History> list = new List<History>();
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(HistoryTable);
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
        public IEnumerable<Program> GetPrograms()
        {
            List<Program> list = new List<Program>();
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(AccountsTable);
            DbConnection.Open();
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Program
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        AccountId = Convert.ToInt32(reader["AccountId"]),
                        Name = (string) reader["Name"],
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    });
            }
            DbConnection.Close();
            return list;
        }
        public IEnumerable<Process> GetProcesses()
        {
            List<Process> list = new List<Process>();
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(AccountsTable);
            DbConnection.Open();
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Process
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        AccountId = Convert.ToInt32(reader["AccountId"]),
                        Name = (string) reader["Name"],
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                    });
            }
            DbConnection.Close();
            return list;
        }
        #endregion

        #region FindByName

        #endregion

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
