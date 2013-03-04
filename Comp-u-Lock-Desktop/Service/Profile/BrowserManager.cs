using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Database;
using Database.Models;
using Microsoft.Win32;
using UrlHistoryLibrary;
using Process = System.Diagnostics.Process;
using Timer = System.Timers.Timer;

namespace Service.Profile
{
    public interface IBrowser
    {
        bool IsRunning();
        IEnumerable<History> GetHistory();
    }

    public class InternetExplorerHistoryReader : IBrowser
    {
        private Timer UpdateTimer;
        private DatabaseManager DbManager;
        public Version Version;

        public InternetExplorerHistoryReader()//DatabaseManager dbManager)
        {
            DbManager = new DatabaseManager("settings", "");
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer");
            if (key != null)
                Version = new Version((string)key.GetValue("Version"));
            SetupUpdateTimer(15);
        }

        private void SetupUpdateTimer(double interval)
        {
            Console.WriteLine("Setting up Browser Manager Update Timer");
            UpdateTimer = new Timer(interval * 1000) { AutoReset = true };
            UpdateTimer.Elapsed += ForceUpdate;
            UpdateTimer.Start();
        }

        private void ForceUpdate(object sender, EventArgs eventArgs)
        {
            var histories = GetHistory();
            var dbhistories = DbManager.GetHistories();
            foreach (var history in histories)
            {
                var dbhistory = dbhistories.FirstOrDefault(h => h.Url == history.Url);
                if (dbhistory != null)
                {
                    DbManager.UpdateHistory(dbhistory);
                }
                else
                {
                    DbManager.SaveHistory(history);
                }
            }
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

        public IEnumerable<History> GetHistory()
        {
            var urlHisotry = new UrlHistoryWrapperClass();
            var enumerator = urlHisotry.GetEnumerator();
            var list = new ArrayList();
            enumerator.GetUrlHistory(list);
            List<History> list1 = new List<History>();
            foreach (STATURL item in list)
            {
                var parsedhistory = ParseHistory(item);
                if (parsedhistory != null)
                    list1.Add(parsedhistory);
            }
            return list1;
        }

        #region private
        private History ParseHistory(STATURL url)
        {
            var history = new History();
            history.Title = url.Title;
            var match = Regex.Match(url.URL, @"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
            if (match.Captures.Count >= 1)
            {
                history.Url = match.Captures[0].ToString();
                history.CreatedAt = DateTime.Now;
                history.UpdatedAt = DateTime.Now;
                return history;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
    
}
