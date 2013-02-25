using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
            var historyList = new List<History>();
            enumerator.GetUrlHistory(list);
            foreach (STATURL item in list)
            {
                historyList.Add(new Database.Models.History
                    {
                        Title = item.Title,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Domain = item.URL
                    });
            }
            return historyList;
        }
    }
    
}
