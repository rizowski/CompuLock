using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service
{
    public class Program
    {
        static  void Main(string[] args)
        {
            /*Console.WriteLine("Processes:");
            RunProcesses();*/
           /* Console.WriteLine("\nPrograms:");
            RunPrograms();*/
            Console.WriteLine("\nProxy Running...");
            RunProxy();
            /*Console.WriteLine("Delegate");
            RunDelegate();*/
            /*Console.WriteLine("Testing TCP");
            TestTCP();*/
            Console.Read();

        }

        public static void RunProcesses()
        {
            var record = new Processes();
            var processes = record.GetProcesses();
            foreach (var process in processes)
            {
                Console.WriteLine(process.ProcessName);
            }
            Console.WriteLine();
            var userprocesses = record.GetProcessesByUser("Rizowski");
            Console.WriteLine();
            Console.WriteLine("Named Processes");
            foreach (var process in userprocesses)
            {
                object[] outParameters = new object[2];
                process.InvokeMethod("GetOwner", outParameters);
                Console.WriteLine("{0} - {1}",outParameters[0], process["name"]);
            }
        }

        public static void RunPrograms()
        {
            Programs pro = new Programs();
            pro.GetRunningPrograms();
        }

        /*public static void RunProxy()
        {
            HistoryProxy hp = new HistoryProxy(9095);
            hp.Start();
            while (true)
            {
                hp.Accept();
            }

        }*/

        public static void RunProxy()
        {
            Proxy p = new Proxy(IPAddress.Loopback, 9095);
            p.Start();
            
        }
        #region Delegate Example
        public static void RunDelegate()
        {
            MyCalc calc = new MyCalc();
            calc.OnKnowAnswer += calc_OnKnowAnswer;

            calc.Add(5, 6);
        }

        // The thing that is fired when this event is triggered.
        private static void calc_OnKnowAnswer(object sender, AnswerEventArgs e)
        {
            //get information about what is being sent by casting the sender.
            var calc = (MyCalc) sender;
            Console.WriteLine(e.Answer);
        }

        public delegate void KnowAnswer(object sender, AnswerEventArgs e);

        //Class holding the information I want to pass around, ex response class
        public class AnswerEventArgs : EventArgs
        {
            public int Answer;
        }

        //The class that does everything. Ex: Proxy class that listens
        public class MyCalc
        {
            public event KnowAnswer OnKnowAnswer;
            public void Add(int one, int two)
            {
                AnswerEventArgs e = new AnswerEventArgs();
                e.Answer = one + two;
                OnKnowAnswer(this, e);
            }
        }
        #endregion
        /*public static void TestTCP()
        {
            HistoryProxy hp = new HistoryProxy(9095);
            hp.Start();
            hp.Accept();
        }*/
    }
}
