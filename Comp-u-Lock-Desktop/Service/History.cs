using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Data;
using Microsoft.Win32;
using UrlHistoryLibrary;

namespace Service
{
    public interface IBrowser
    {
        bool IsRunning();
        IEnumerable<URL> GetHistory();
    }

    public class InternetExplorer : IBrowser
    {
        public Version Version;
        public InternetExplorer()
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

    public class Firefox : IBrowser
    {
        public Version Version;
        public Uri HistoryPath;

        public Firefox()
        {
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
                    HistoryPath =
                        new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles");
                    break;
                case Windows.Seven:
                    //Windows 7+ = @"C:\Users\"+Environment.UserName+@"\AppData\Roaming\Mozilla\Firefox\Profiles"
                    HistoryPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles");
                    break;
                case Windows.Vista:
                    //Vista = @"C:\Users\"+Environment.UserName+@"\AppData\Roaming\Mozilla\Firefox\Profiles\
                    HistoryPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles\");
                    break;
                case Windows.Xp:
                    //Xp = C:\Documents and Settings\Owner\Application Data\Mozilla\Firefox\Profiles\
                    HistoryPath = new Uri(@"C:\Documents and Settings\" + Environment.UserName + @"\Application Data\Mozilla\Firefox\Profiles\");
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

        public IEnumerable<URL> GetHistory()
        {
            return null;
        }
    }

    public class Chrome : IBrowser
    {
        public Version Version;
        public string HistoryPath;
        public Chrome()
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

                    HistoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\";
                    break;
                case Windows.Seven:
                    HistoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\";
                    break;
                case Windows.Vista:
                    HistoryPath = @"C:\users\" + Environment.UserName + @"\LocalSettings\Application Data\Google\Chrome\User Data\Default\";
                    break;
                case Windows.Xp:
                    HistoryPath = @"C:\Documents and Settings\" + Environment.UserName + @"\Local Settings\Application Data\Google\Chrome\User Data\Default\";
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

        public IEnumerable<URL> GetHistory()
        {
            return null;
        }
    }
}
