using System;
using System.Collections.Generic;
using System.Data.SQLite;

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
            List<URL> list = new List<URL>();
            while (reader.Read())
            {
                string url = (string) reader["url"];
                string title = (string) reader["title"];
                int visitcount = (int) reader["visit_count"];
                long visitDate = (long) reader["last_visit_date"];

                list.Add(new URL(visitDate,url,title,"Firefox",visitcount));
                Console.WriteLine("{0} - {1} - {2}", reader["last_visit_date"],reader["title"], reader["url"]);
            }
            reader.Close();
            return list;
        }

    }

    public class ChromeHistoryReader
    {
        public string DbPath;
        public string FileName;
        public SQLiteConnection SqlConnection;
        private const string HISTORY_DB = "urls";
        private const string END = ";";

        public ChromeHistoryReader(string path, string filename)
        {
            DbPath = path;
            FileName = filename;
        }

        public void Connect()
        {
            // C:\Users\Rizowski\AppData\Local\Google\Chrome\User Data\Default
            SqlConnection = new SQLiteConnection("Data Source="+DbPath + FileName + END);
            SqlConnection.Open();
        }

        public IEnumerable<URL> GetHistory()
        {
            string sql = "select * from " + HISTORY_DB + END;
            SQLiteCommand command = new SQLiteCommand(sql, SqlConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<URL> list = new List<URL>();
            while (reader.Read())
            {
                long date = (long) reader["last_visit_time"];
                int visit = Convert.ToInt32(reader["visit_count"]);
                list.Add(new URL(date,(string) reader["url"], (string) reader["title"],"Chrome",visit));
                Console.WriteLine("{0} - {1} - {2}", reader["last_visit_time"], reader["title"], reader["url"]);
            }
            reader.Close();
            return list;
        }
    }
}
