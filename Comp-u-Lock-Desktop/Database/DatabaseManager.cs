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

        private const string HistoryTable = "history";
        private const string ProcessTable = "account_process";

        private const string RestrictionTable = "Restrictions";
        private const string DayTable = "Days";
        private const string HourTable = "Hours";

        private const string CreateTable = "CREATE TABLE ";
        private const string IfNotExists = " IF NOT EXISTS ";
        private const string InsertInto = "INSERT INTO ";
        private const string Select = "SELECT ";
        private const string SelectAll = "SELECT * FROM ";
        private const string Update = "UPDATE ";

        private const string Set = " SET ";
        private const string All = "* FROM ";
        private const string From = "FROM ";
        private const string Where = " WHERE ";

        private const string Values = " VALUES ";
        private const string End = ";";
        #endregion

        public DatabaseManager(string path, string pass)
        {
            DbPassword = pass;
            DbPath = path + ".sqlite";
            DbExists = File.Exists(DbPath);
            CreateDb();
        }

        public void CreateDb()
        {
            if(!DbExists)
                SQLiteConnection.CreateFile(DbPath);
            DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");//Password="+DbPassword+";");
            //TODO check to see if sqlite is busy before inserting
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
            Console.WriteLine("Checking if Tables exist.");
            const string user = CreateTable + IfNotExists + UsersTable + "(Id integer primary key asc, WebId integer, Username varchar(255), Email varchar(255), AuthToken varchar(255), CreatedAt datetime, UpdatedAt datetime, unique(Email, AuthToken))";
            ExecuteQuery(user);
            const string computer = CreateTable + IfNotExists + ComputersTable + "(Id integer primary key asc, WebId integer, UserId integer, Enviroment varchar(50), Name varchar(50) unique, IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(computer);
            const string account = CreateTable + IfNotExists + AccountsTable + "(Id integer primary key asc, WebId integer, Domain varchar(50), Username varchar(50), Tracking bool, Locked bool, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime, unique(Domain, Username))";
            ExecuteQuery(account);
            const string accountHistory = CreateTable + IfNotExists + HistoryTable + "(Id integer primary key asc, WebId integer, ComputerId integer, Title varchar(150), Url varchar(300), VisitCount integer, CreatedAt datetime, UpdatedAt datetime, unique(ComputerId, Url) on conflict replace)";
            ExecuteQuery(accountHistory);
            const string accountProcess = CreateTable + IfNotExists + ProcessTable + "(Id integer primary key asc, WebId integer, AccountId integer, Name varchar(100), CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProcess);

            const string restriction = CreateTable + IfNotExists + RestrictionTable + "(Id integer primary key asc, WebId integer, AccountId integer, unique(Id, AccountId) on conflict replace)";
            ExecuteQuery(restriction);
            const string day = CreateTable + IfNotExists + DayTable + "(Id integer primary key asc, WebId integer, RestrictionId integer, unique(Id, RestrictionId) on conflict replace)";
            ExecuteQuery(day);
            const string hour = CreateTable + IfNotExists + HourTable + "(Id integer primary key asc, WebId integer, DayId integer, unique(Id, DayId) on conflict replace)";
            ExecuteQuery(hour);
            Console.WriteLine("Done.");
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
            Console.WriteLine("Saving a User");
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
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done Writing User");
            return GetUser();
        }
        public Computer SaveComputer(Computer comp)
        {
            if (comp == null)
                throw new NoNullAllowedException("Computer can't be null");
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a Computer");
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
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done Saving a computer");
            return GetComputer();
        }
        public Account SaveAccount(Account account)
        {
            if (account == null)
                throw new NoNullAllowedException("Account can't be null");
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving an account");
            sb.Append(InsertInto);
            sb.Append(AccountsTable);
            sb.Append("(Domain, Username, Tracking, AllottedTime, UsedTime, Locked, CreatedAt, UpdatedAt) ");
            sb.Append(Values);
            sb.Append("(@domain, @username, @tracking, @allottedTime, @usedTime, @locked, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@domain", account.Domain));
                command.Parameters.Add(new SQLiteParameter("@username", account.Username));
                command.Parameters.Add(new SQLiteParameter("@tracking", account.Tracking));
                command.Parameters.Add(new SQLiteParameter("@allottedTime", account.AllottedTime.TotalSeconds));
                command.Parameters.Add(new SQLiteParameter("@usedTime", account.UsedTime.TotalSeconds));
                command.Parameters.Add(new SQLiteParameter("@locked", account.Locked));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done Saving an Account");
            var savedAccount = GetAccounts().First(a => a.Username == account.Username);
            return savedAccount;
        }
        public History SaveHistory(History history)
        {
            if (history == null)
                throw new NoNullAllowedException("History Object can't be null");
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving History");
            sb.Append(InsertInto);
            sb.Append(HistoryTable);
            sb.Append("(ComputerId, Title, Url, VisitCount, CreatedAt, UpdatedAt)");
            sb.Append(Values);
            sb.Append("(@computerid, @title, @url, @visitcount, @createdAt, @updatedAt)");
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@computerid", history.ComputerId));
                command.Parameters.Add(new SQLiteParameter("@title", history.Title));
                command.Parameters.Add(new SQLiteParameter("@url", history.Url));
                command.Parameters.Add(new SQLiteParameter("@visitcount", history.VisitCount));
                command.Parameters.Add(new SQLiteParameter("@createdAt", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing history");
            var savedHistory = GetHistories().First(h => h.ComputerId == history.ComputerId && h.Url == history.Url);
            return savedHistory;
        }
        public Process SaveProcess(Process process)
        {
            if (process == null)
                throw new NoNullAllowedException("Process object can't be null");
            if (process.AccountId <= 0)
                throw new ArgumentException("AccountId needs to be specified.");
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Saving a Process");
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
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing Process");
            var savedProcess = GetProcesses().First(p => p.AccountId == process.AccountId && p.Name == process.Name);
            return savedProcess;
        }
        #endregion

        #region Update
        public Computer UpdateComputer(Computer computer)
        {
            if (computer == null)
                throw new NullReferenceException("Computer cant be null");
            if(computer.Id <= 0)
                throw new ArgumentException("Computer id cant be less than or equal to 0");
            StringBuilder sb = new StringBuilder();
            sb.Append(Update);
            sb.Append(ComputersTable);
            sb.Append(Set);
            sb.Append("Name=@name, Enviroment=@enviroment, IpAddress=@ipAddress, UpdatedAt=@updatedAt");
            sb.Append(Where);
            sb.Append("Id = " + computer.Id);
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@name", computer.Name));
                command.Parameters.Add(new SQLiteParameter("@enviroment", computer.Enviroment));
                command.Parameters.Add(new SQLiteParameter("@ipAddress", computer.IpAddress));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            return GetComputer();
        }
        public User UpdateUser(User user)
        {
            //TODO NEEDS TO BE DONE
            if (user == null)
                throw new NullReferenceException("User cant be null");
            if (user.Id <= 0)
                throw new ArgumentException("Userid cant be less than or equal to 0");
            StringBuilder sb = new StringBuilder();
            sb.Append(Update);
            sb.Append(UsersTable);
            sb.Append(Set);
            sb.Append("Username=@username, Email=@email, AuthToken=@authtoken, UpdatedAt=@updatedAt");
            sb.Append(Where);
            sb.Append("Id = " + user.Id);
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@username", user.Username));
                command.Parameters.Add(new SQLiteParameter("@email", user.Email));
                command.Parameters.Add(new SQLiteParameter("@authtoken", user.AuthToken));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            return GetUser();
        }
        public Account UpdateAccount(Account account)
        {
            if (account == null)
                throw new NullReferenceException("Account cant be null");
            if (account.Id <= 0)
                throw new ArgumentException("Account id cant be less than or equal to 0");
            StringBuilder sb = new StringBuilder();
            sb.Append(Update);
            sb.Append(AccountsTable);
            sb.Append(Set);
            sb.Append("Domain=@domain, Username=@username, Tracking=@tracking, AllottedTime=@allottedTime, UsedTime=@usedTime, Locked=@locked, UpdatedAt=@updatedAt");
            sb.Append(Where);
            sb.Append("Id = " + account.Id);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                command.Parameters.Add(new SQLiteParameter("@domain", account.Domain));
                command.Parameters.Add(new SQLiteParameter("@username", account.Username));
                command.Parameters.Add(new SQLiteParameter("@tracking", account.Tracking));
                command.Parameters.Add(new SQLiteParameter("@allottedTime", account.AllottedTime.TotalSeconds));
                command.Parameters.Add(new SQLiteParameter("@usedTime", account.UsedTime.TotalSeconds));
                command.Parameters.Add(new SQLiteParameter("@locked", account.Locked));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            return GetAccountById(account.Id);
        }
        public History UpdateHistory(History history)
        {
            if(history == null)
                throw new NoNullAllowedException("History cannot be null");
            StringBuilder sb = new StringBuilder();
            sb.Append(Update);
            sb.Append(HistoryTable);
            sb.Append(Set);
            sb.Append("Title=@title, Url=@url, VisitCount=@visitcount, UpdatedAt=@updatedAt");
            sb.Append(Where);
            sb.Append("Id= " + history.Id);
            sb.Append(End);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
                //command.Parameters.Add(new SQLiteParameter("@computerId", history.ComputerId));
                command.Parameters.Add(new SQLiteParameter("@title", history.Title));
                command.Parameters.Add(new SQLiteParameter("@url", history.Url));
                command.Parameters.Add(new SQLiteParameter("@visitcount", history.VisitCount));
                command.Parameters.Add(new SQLiteParameter("@updatedAt", DateTime.Now));
            DbConnection.Open();
            Console.WriteLine(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            return GetHistoryById(history.Id);
        }
        #endregion

        #region GetAll
        public IEnumerable<Account> GetAccounts()
        {
            List<Account> list = new List<Account>();
            StringBuilder sb = new StringBuilder();
            sb.Append(SelectAll);
            sb.Append(AccountsTable);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Account
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Domain = (string) reader["Domain"],
                            Username = (string) reader["Username"],
                            Tracking = (Convert.ToInt32(reader["Tracking"]) == 1),
                            AllottedTime = TimeSpan.FromSeconds(Convert.ToInt32(reader["AllottedTime"])),
                            Locked = Convert.ToBoolean(reader["Locked"]),
                            UsedTime = TimeSpan.FromSeconds(Convert.ToInt32(reader["UsedTime"])),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        });
                }
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
            var command= new SQLiteCommand(sb.ToString(), DbConnection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new History
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ComputerId = Convert.ToInt32(reader["ComputerId"]),
                        Title = Convert.ToString(reader["Title"]),
                        Url = Convert.ToString(reader["Url"]),
                        VisitCount = Convert.ToInt32(reader["VisitCount"]),
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
            sb.Append(ProcessTable);
            DbConnection.Open();
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Process
                    {
                        Id =        Convert.ToInt32(reader["Id"]),
                        AccountId = Convert.ToInt32(reader["AccountId"]),
                        Name =      Convert.ToString(reader["Name"]),
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
        #endregion

        #region ByAccountId
        public IEnumerable<History> GetHistoriesByAccountId(int computerId)
        {
            return GetHistories().Where(h => h.ComputerId == computerId);
        }

        public IEnumerable<Process> GetProcessesByAccountId(int accountId)
        {
            return GetProcesses().Where(p => p.AccountId == accountId);
        }
        #endregion

        #region byString
        public Account GetAccountByName(string name)
        {
            return GetAccounts().FirstOrDefault(a => a.Username == name);
        }

        #endregion

        public Account GetTrackingAccounts()
        {
            return GetAccounts().FirstOrDefault(a => a.Tracking);
        }

        public void SaveAccounts(int id, List<Account> accounts)
        {
            Console.WriteLine("Saving accounts");
            foreach (var account in accounts)
            {
                account.ComputerId = id;
                SaveAccount(account);
            }
        }

        public void SaveHistories(int computerId, IEnumerable<History> histories )
        {
            foreach (var history in histories)
            {
                history.ComputerId = computerId;
                SaveHistory(history);
            }
        }

        public User GetFullUser()
        {
            var user = GetUser();
            if(user == null)
                user = new User();
            var computer = GetComputer();
            user.Computers = new List<Computer>();
            computer.Accounts = new List<Account>();
            computer.Histories = new List<History>();
            user.Computers.Add(computer);
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                computer.Accounts.Add(account);
            }
            var histories = GetHistories();
            foreach (var history in histories)
            {
                computer.Histories.Add(history);
            }
            for (int i = 0; i < accounts.Count(); i++)
            {
                var account = accounts.ToArray()[i];
                account.Processes = new List<Process>();
                var processes = GetProcessesByAccountId(account.Id);

                foreach (var process in processes)
                {
                    account.Processes.Add(process);
                }
            }
            return user;
        }
    }
}
