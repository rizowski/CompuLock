using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Service
{
    public static class Logger
    {
        private const string LogFile = "Logg";
        public static void Write(string message, Status status = Status.Ok)
        {
            StreamWriter sw;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(DateTime.Now);
            sb.Append("]");
            sb.Append("[");
            sb.Append(status.ToString());
            sb.Append("] ");
            sb.Append(message);
            if (!File.Exists(LogFile))
            {
                File.Create(LogFile).Close();
            }
            sw = new StreamWriter(File.Open(LogFile, FileMode.Append));
            sw.WriteLine(sb.ToString());
            sw.Flush();
            sw.Close();
            Console.WriteLine("[Service][{0}] Logged",status.ToString());
        }
    }

    public enum Status
    {
        Ok,
        Error,
        Warning
    }
}
