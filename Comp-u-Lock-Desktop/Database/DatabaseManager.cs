﻿using System;
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
        private const string ProgramTable = "account_program";

        private const string RestrictionTable = "Restrictions";
        private const string DayTable = "Days";
        private const string HourTable = "Hours";

        private const string CreateTable = "create table ";
        private const string IfNotExists = " if not exists ";
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
            DbPath = path + ".sqlite";
            DbExists = File.Exists(DbPath);
            CreateDb();
        }

        public void CreateDb()
        {
            if(!DbExists)
                SQLiteConnection.CreateFile(DbPath);
            DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");//Password="+DbPassword+";");
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
            const string user = CreateTable + IfNotExists + UsersTable + "(Id integer primary key asc, WebId integer, Username varchar(255), Email varchar(255), AuthToken varchar(255), CreatedAt datetime, UpdatedAt datetime, unique(Email, AuthToken) on conflict replace)";
            ExecuteQuery(user);
            const string computer = CreateTable + IfNotExists + ComputersTable + "(Id integer primary key asc, WebId integer, UserId integer, Enviroment varchar(50), Name varchar(50) unique on conflict replace, IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            ExecuteQuery(computer);
            const string account = CreateTable + IfNotExists + AccountsTable + "(Id integer primary key asc, WebId integer, ComputerId integer, Domain varchar(50), Username varchar(50), Tracking bool, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime, unique(Domain, Username) on conflict replace)";
            ExecuteQuery(account);
            const string accountHistory = CreateTable + IfNotExists + HistoryTable + "(Id integer primary key asc, WebId integer, ComputerId integer, Title varchar(150), Url varchar(300), VisitCount integer, CreatedAt datetime, UpdatedAt datetime, unique(ComputerId, Url) on conflict replace)";
            ExecuteQuery(accountHistory);
            const string accountProcess = CreateTable + IfNotExists + ProcessTable + "(Id integer primary key asc, WebId integer, AccountId integer, Name varchar(100), CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProcess);
            const string accountProgram = CreateTable + IfNotExists + ProgramTable + "(Id integer primary key asc, WebId integer, AccountId integer, Name varchar(100), OpenCount integer, CreatedAt datetime, UpdatedAt datetime, unique(AccountId, Name) on conflict replace)";
            ExecuteQuery(accountProgram);

            const string restriction = CreateTable + IfNotExists + RestrictionTable + "(Id integer primary key asc, WebId integer, AccountId integer, unique(Id, AccountId) on conflict replace)";
            ExecuteQuery(restriction);
            const string day = CreateTable + IfNotExists + DayTable + "(Id integer primary key asc, WebId integer, RestrictionId integer, unique(Id, RestrictionId) on conflict replace)";
            ExecuteQuery(day);
            const string hour = CreateTable + IfNotExists + HourTable + "(Id integer primary key asc, WebId integer, DayId integer, unique(Id, DayId) on conflict replace)";
            ExecuteQuery(hour);
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
            Logger.Write("Saving a User");
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
            Logger.Write(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Logger.Write("Done Writing User");
            return GetUser();
        }

        public Computer SaveComputer(Computer comp)
        {
            if (comp == null)
                throw new NoNullAllowedException("Computer can't be null");
            StringBuilder sb = new StringBuilder();
            Logger.Write("Saving a Computer");
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
            Logger.Write(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Logger.Write("Done Saving a computer");
            return GetComputer();
        }

        public Account SaveAccount(Account account)
        {
            if (account == null)
                throw new NoNullAllowedException("Account can't be null");
            if (account.ComputerId <= 0)
                throw new ArgumentException("Computer Id is required.");
            StringBuilder sb = new StringBuilder();
            Logger.Write("Saving an account");
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
            Logger.Write(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Logger.Write("Done Saving an Account");
            var savedAccount = GetAccounts().First(a => a.Username == account.Username);
            return savedAccount;
        }

        public History SaveHistory(History history)
        {
            if (history == null)
                throw new NoNullAllowedException("History Object can't be null");
            StringBuilder sb = new StringBuilder();
            Logger.Write("Saving History");
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
            Logger.Write(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close(); 
            Logger.Write("Done writing history");
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
            Logger.Write("Saving a Process");
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
            Logger.Write(sb.ToString());
            command.ExecuteNonQuery();
            DbConnection.Close();
            Logger.Write("Done writing Process");
            var savedProcess = GetProcesses().First(p => p.AccountId == process.AccountId && p.Name == process.Name);
            return savedProcess;
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
            var reader = new SQLiteCommand(sb.ToString(), DbConnection).ExecuteReader();
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
    }
}
