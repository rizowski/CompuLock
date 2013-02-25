using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using Database.Enviroment;
using Microsoft.Win32;
using UrlHistoryLibrary;

namespace Service.Profile
{

    public interface IBrowser
    {
        bool IsRunning();
        IEnumerable<URL> GetHistory();
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
