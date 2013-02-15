using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace Database
{
    public class DatabaseManager
    {
        public string DbPath { get; set; }
        public SQLiteConnection DbConnection { get; set; }
        public DatabaseManager(string path, string pass)
        {
            var DbPath = path + ".sqlite";
            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
                DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;");
                DbConnection.Open();
                DbConnection.ChangePassword(pass);
                CreateTables();
            }
            else
            {
                DbConnection = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;Password="+pass+";");
            }
        }

        public SQLiteConnection Connect(string pass)
        {
            DbConnection.SetPassword(pass);
            return DbConnection.OpenAndReturn();
        }

        public void Disconnect()
        {
            DbConnection.Close();
            DbConnection.Dispose();
        }

        public void CreateTables()
        {
            string user = "create table users (Username varchar(255), Email varchar(255), AuthToken varchar(255), CreatedAt datetime, UpdatedAt datetime)";
            string computer = "create table computers (Id integer primary key asc, UserId integer, Enviroment varchar(50), Name varchar(50), IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            string account = "create table accounts(Id integer primary key asc, ComputerId integer, Domain varchar(50), Username varchar(50), Tracking integer, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime)";

            string accountHistory = "create table account_history (Id integer primary key asc, AccountId integer, Domain varchar(150), Url varchar(300), LastVisited datetime, VisitCount integer, , CreatedAt datetime, UpdatedAt datetime)";
            string accountProcess = "create table account_process (Id integer primary key asc, AccountId integer, Name varchar(100), LastRun datetime, CreatedAt datetime, UpdatedAt datetime)";
            string accountProgram = "create table account_program (Id integer primary key asc, AccountId integer, Name varchar(100), OpenCount integer, LastRan datetime, CreatedAt datetime, UpdatedAt datetime)";
            var command = new SQLiteCommand(computer, DbConnection);
            command.ExecuteNonQuery();
            command.CommandText = user;
            command.ExecuteNonQuery();
            command.CommandText = account;
            command.ExecuteNonQuery();
            command.CommandText = accountHistory;
            command.ExecuteNonQuery();
            command.CommandText = accountProcess;
            command.ExecuteNonQuery();
            command.CommandText = accountProgram;
            command.ExecuteNonQuery();
            DbConnection.Close();
        }
    }
}
