using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using UrlHistoryLibrary;

namespace Data
{
    public class FirefoxHistoryReader
    {
        public string DbPath;
        public string Filename;
        public SQLiteConnection SqlConnection;

        private const string END = ";";
        private const string PLACES = "moz_places";

        public FirefoxHistoryReader(string path, string filename)
        {
            DbPath = path;
            Filename = filename;
        }

        public void Connect()
        {
            SqlConnection = new SQLiteConnection("Data Source=" + DbPath + Filename + END);
            SqlConnection.Open();
        }

        public IEnumerable<URL> GetHistory()
        {
            string sql = "select * from " + PLACES + END;
            SQLiteCommand command = new SQLiteCommand(sql, SqlConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("{0} - {1} - {2}", reader["last_visit_date"],reader["title"], reader["url"]);
            }
            return null;
        }

    }
}
