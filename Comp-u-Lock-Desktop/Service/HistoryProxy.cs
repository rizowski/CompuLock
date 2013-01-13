using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using UrlHistoryLibrary;

namespace Service
{
    public interface IBrowser
    {
        bool IsRunning();
        void GetHistory();
    }

    public class InternetExplorer : IBrowser
    {
        public Version Version;
        public InternetExplorer()
        {
            try
            {
                Version = new Version((string)Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer")
                    .GetValue("Version"));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.Message);
                Version = new Version("0");
            }
            
            Console.WriteLine(Version);
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

        public void GetHistory()
        {
            var urlHisotry = new UrlHistoryWrapperClass();
            var enumerator = urlHisotry.GetEnumerator();
            var list = new ArrayList();
            enumerator.GetUrlHistory(list);
            foreach (STATURL item in list)
            {
                Console.WriteLine(item.URL);
            }

        }
    }

    public class Firefox : IBrowser
    {
        public Version Verson;
        public Uri HistoryPath;

        public Firefox()
        {
            switch (OS.Name)
            {
                case Windows.Eight:
                    //Windows 7+ = %APPDATA%\Mozilla\Firefox\Profiles
                    break;
                case Windows.Seven:
                    //Windows 7+ = %APPDATA%\Mozilla\Firefox\Profiles
                    break;
                case Windows.Vista:
                    //Vista = C:\Users\Owner\AppData\Roaming\Mozilla\Firefox\Profiles\
                    break;
                case Windows.Xp:
                    //Xp = C:\Documents and Settings\Owner\Application Data\Mozilla\Firefox\Profiles\
                    break;
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

        public void GetHistory()
        {
            
        }
    }

    public class Chrome : IBrowser
    {
        public Version Version;
        public Uri HistoryPath;
        public Chrome()
        {
            switch (OS.Name)
            {
                case Windows.Eight:
                    HistoryPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default");
                    break;
                case Windows.Seven:
                    HistoryPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default");
                    break;
                case Windows.Vista:
                    HistoryPath = new Uri(@"C:\users\" + Environment.UserName + @"\Local Settings\Application Data\Google\Chrome\User Data\Default\");
                    break;
                case Windows.Xp:
                    HistoryPath = new Uri(@"C:\Documents and Settings\" + Environment.UserName + @"\Local Settings\Application Data\Google\Chrome\User Data\Default\");
                    break;
            }
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

        public void GetHistory()
        {
            
        }
    }
}
