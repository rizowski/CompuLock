using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
namespace Service
{
    class History
    {
        //Enviroment.GetFolderPath(Enviroment.SpecialFolder.ApplicationData)
        public Version OsVersion { get
        {
            // Enviroment.osVerions
            var version = Environment.OSVersion.Version.ToString().Split('.');
            //double os = double.Parse(version[0]) + double.Parse(version[1])/10;
            var major = int.Parse(version[0]);
            var minor = int.Parse(version[1]);
            var build = int.Parse(version[2]);
            var revision = int.Parse(version[3]);
            return new Version(major,minor,build,revision);
        } }
        public PlatformID Platform
        {
            get { return Environment.OSVersion.Platform; }
        }

        public string ChromeHistoryPath { get; set; }
        public string FirefoxHistoryPath { get; set; }
        public string IeHistoryPath { get; set; }

        public History()
        {
            Console.WriteLine("{0}.{1}", OsVersion.Major, OsVersion.Minor);
        }

        public void GetChromeHistory()
        {
            switch (Platform)
            {
                    case PlatformID.Win32NT:
                    {
                        //Xp = C:\Documents and Settings\<USER>\Local Settings\Application Data\Google\Chrome\User Data\Default\
                        // Vista = C:\users\<USER>\Local Settings\Application Data\Google\Chrome\User Data\Default\
                        //windows7 + = C:\Users\[username]\AppData\Local\Google\Chrome\User Data\Default

                        //Check to see what version of windws you are on
                        if (0==0)
                        {
                            
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public void GetFirefoxHistory()
        {
            switch (Platform)
            {
                    case PlatformID.Win32NT:
                    {
                        //Xp = C:\Documents and Settings\Owner\Application Data\Mozilla\Firefox\Profiles\
                        //Vista = C:\Users\Owner\AppData\Roaming\Mozilla\Firefox\Profiles\
                        //Windows 7+ = %APPDATA%\Mozilla\Firefox\Profiles
                        // Check what version of Windows you are on.
                        if (0==0)
                        {
                            
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public void GetIeHistory()
        {
            switch (Platform)
            {
                case PlatformID.Win32NT:
                    {
                        // Check what version of Windows you are on.
                        if (0 == 0)
                        {

                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
