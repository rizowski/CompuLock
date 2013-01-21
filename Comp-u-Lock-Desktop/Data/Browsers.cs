using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Service.Enviroment;
using UrlHistoryLibrary;

namespace Data
{

    public interface IBrowser
    {
        bool IsRunning();
        IEnumerable<URL> GetHistory();
    }

    public class FirefoxHistoryReader : IBrowser
    {
        public Uri DbPath;
        public string Filename;
        public SQLiteConnection SqlConnection { get; private set; }
        public Version Version { get; private set; }

        public string profile { get; set; }

        private const string END = ";";
        private const string PLACES = "moz_places";

        public FirefoxHistoryReader(string profile)
        {
            this.profile = profile;
            object path;
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe");
            if (key != null)
            {
                path = key.GetValue("");
                if (path != null)
                    Version = new Version(FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion);
            }
            else
            {
                Version = new Version("0");
            }

            switch (OS.Name)
            {
                case Windows.Eight:
                    //Windows 7+ = %APPDATA%\Mozilla\Firefox\Profiles
                    DbPath =
                        new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles"+profile);
                    break;
                case Windows.Seven:
                    //Windows 7+ = @"C:\Users\"+Environment.UserName+@"\AppData\Roaming\Mozilla\Firefox\Profiles"
                    DbPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles"+profile);
                    break;
                case Windows.Vista:
                    //Vista = @"C:\Users\"+Environment.UserName+@"\AppData\Roaming\Mozilla\Firefox\Profiles\
                    DbPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles"+profile);
                    break;
                case Windows.Xp:
                    //Xp = C:\Documents and Settings\Owner\Application Data\Mozilla\Firefox\Profiles\
                    DbPath = new Uri(@"C:\Documents and Settings\" + Environment.UserName + @"\Application Data\Mozilla\Firefox\Profiles"+profile);
                    break;
            }
            if (!File.Exists(DbPath.AbsolutePath))
            {
                throw new FileNotFoundException("No Firefox Profile Folder Found.");
            }
        }

        public bool IsRunning()
        {
            Process[] fire = Process.GetProcessesByName("firefox");
            if (fire.Length != 0)
            {
                return true;
            }
            return false;
        }

        public void Connect()
        {
            SqlConnection = new SQLiteConnection("Data Source=" + DbPath.AbsolutePath + Filename + END);
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

    public class ChromeHistoryReader : IBrowser
    {
        public string DbPath;

        public Version Version { get; private set; }
        public SQLiteConnection SqlConnection { get; private set; }

        private const string HISTORY_DB = "urls";
        private const string END = ";";

        public ChromeHistoryReader()
        {
            object path;
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe");
            if (key != null)
            {
                path = key.GetValue("");
                if (path != null)
                    Version = new Version(FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion);
            }
            else
            {
                Version = new Version("0");
            }
            switch (OS.Name)
            {
                case Windows.Eight:

                    DbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                    break;
                case Windows.Seven:
                    DbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                    break;
                case Windows.Vista:
                    DbPath = @"C:\users\" + Environment.UserName + @"\LocalSettings\Application Data\Google\Chrome\User Data\Default\History";
                    break;
                case Windows.Xp:
                    DbPath = @"C:\Documents and Settings\" + Environment.UserName + @"\Local Settings\Application Data\Google\Chrome\User Data\Default\History";
                    break;
            }
            if (!File.Exists(DbPath))
            {
                throw new FileNotFoundException("No File found for Chrome History.");
            }
        }

        public void Connect()
        {
            // C:\Users\Rizowski\AppData\Local\Google\Chrome\User Data\Default
            SqlConnection = new SQLiteConnection("Data Source="+DbPath + END);
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

        public bool IsRunning()
        {
            Process[] chrome = Process.GetProcessesByName("chrome");
            if (chrome.Length != 0)
            {
                return true;
            }
            return false;
        }
    }

    public class InternetExplorerHistoryReader : IBrowser
    {
        public Version Version;
        public InternetExplorerHistoryReader()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer");
            if (key != null)
                Version = new Version((string)key.GetValue("Version"));
        }

        public bool IsRunning()
        {
            Process[] ie = Process.GetProcessesByName("iexplore");
            if (ie.Length != 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<URL> GetHistory()
        {
            var urlHisotry = new UrlHistoryWrapperClass();
            var enumerator = urlHisotry.GetEnumerator();
            var list = new ArrayList();
            enumerator.GetUrlHistory(list);
            foreach (STATURL item in list)
            {

                Console.WriteLine(item.URL);
            }
            return null;
        }
    }
    
}
