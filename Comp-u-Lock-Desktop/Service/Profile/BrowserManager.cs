using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Database.Enviroment;
using Database.Models;
using Microsoft.Win32;
using UrlHistoryLibrary;
using Process = System.Diagnostics.Process;

namespace Service.Profile
{

    public interface IBrowser
    {
        bool IsRunning();
        IEnumerable<History> GetHistory();
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
