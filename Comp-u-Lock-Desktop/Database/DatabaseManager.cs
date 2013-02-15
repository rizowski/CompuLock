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
            DbConnection.Open();
            Console.WriteLine("Creating tables");
            string user = "create table users (Username varchar(255), Email varchar(255), AuthToken varchar(255), CreatedAt datetime, UpdatedAt datetime)";
            string computer = "create table computers (Id integer primary key asc, UserId integer, Enviroment varchar(50), Name varchar(50), IpAddress varchar(16), CreatedAt datetime, UpdatedAt datetime)";
            string account = "create table accounts(Id integer primary key asc, ComputerId integer, Domain varchar(50), Username varchar(50), Tracking integer, AllottedTime integer, UsedTime integer, CreatedAt datetime, UpdatedAt datetime)";

            string accountHistory = "create table account_history (Id integer primary key asc, AccountId integer, Domain varchar(150), Url varchar(300), LastVisited datetime, VisitCount integer, CreatedAt datetime, UpdatedAt datetime)";
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
            Console.WriteLine("Done creating tables");
        }

        public void SaveUser(User user)
        {
            Console.WriteLine("Saving a User.");
            string sql = "insert into Users (Username, Email, AuthToken, CreatedAt, UpdatedAt) ";
            string values = "values (";
            string end = ");";
            StringBuilder sb = new StringBuilder();
            sb.Append(sql);
            sb.Append(values);
            sb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}'", user.Username, user.Email, user.AuthToken, DateTime.Now,
                            DateTime.Now);
            sb.Append(end);
            Console.WriteLine("Sql line using: {0}", sb);
            var command = new SQLiteCommand(sb.ToString(), DbConnection);
            DbConnection.Open();
            command.ExecuteNonQuery();
            DbConnection.Close();
            Console.WriteLine("Done writing User.");
        }
    }
}
