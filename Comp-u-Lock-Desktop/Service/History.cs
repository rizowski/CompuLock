using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
namespace Service
{
    public class History
    {
        public Uri ChromeHistoryPath { get; set; }
        public Uri FirefoxHistoryPath { get; set; }
        public Uri IeHistoryPath { get; set; }

        public History()
        {
            Console.WriteLine("{0}.{1}", Os.Version.Major, Os.Version.Minor);
            switch (Os.Name)
            {
                case Windows.Xp:
                    IeHistoryPath = new Uri("");//Xp = 
                    FirefoxHistoryPath = new Uri("");//Xp = C:\Documents and Settings\Owner\Application Data\Mozilla\Firefox\Profiles\
                    ChromeHistoryPath = new Uri(@"C:\Documents and Settings\" + Environment.UserName + @"\Local Settings\Application Data\Google\Chrome\User Data\Default\");
                    break;
                case Windows.Vista:
                    IeHistoryPath = new Uri("");//Vista = 
                    FirefoxHistoryPath = new Uri("");//Vista = C:\Users\Owner\AppData\Roaming\Mozilla\Firefox\Profiles\
                    ChromeHistoryPath = new Uri(@"C:\users\" + Environment.UserName + @"\Local Settings\Application Data\Google\Chrome\User Data\Default\");
                    break;
                case Windows.Seven:
                    IeHistoryPath = new Uri("");//Windows 7 = 
                    FirefoxHistoryPath = new Uri("");//Windows 7+ = %APPDATA%\Mozilla\Firefox\Profiles
                    ChromeHistoryPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default");
                    break;
                case Windows.Eight:
                    IeHistoryPath = new Uri("");//Windows 8 = C:\Users\Rizowski\AppData\Local\Microsoft\Windows\History
                    FirefoxHistoryPath = new Uri("");//Windows 7+ = %APPDATA%\Mozilla\Firefox\Profiles
                    ChromeHistoryPath = new Uri(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default");
                    break;
                default:
                    break;
            }
        }

        public void GetChromeHistory()
        {

        }

        public void GetFirefoxHistory()
        {
        }

        public void GetIeHistory()
        {
        }
    }
}
