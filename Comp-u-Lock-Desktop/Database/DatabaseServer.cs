using System;
using System.Threading;
using Raven.Client.Embedded;

namespace Database.Models
{
    public class DatabaseServer
    {
        public EmbeddableDocumentStore Store;
        public bool Started { get; set; }

        private Thread serverThread;
        public DatabaseServer(string location, bool server)
        {
            Console.WriteLine("Database Server");
            Store = new EmbeddableDocumentStore {DataDirectory = location, UseEmbeddedHttpServer = server};
        }

        public static void Main(string[] args)
        {
            try
            {
                var server = new DatabaseServer("Data", true);
                server.StartServer();
                while (true)
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public void StartServer()
        {
            serverThread = new Thread(() => Store.Initialize());
            serverThread.Start();
            Started = true;
        }

        public void StopServer()
        {
            Store.Dispose();
            serverThread.Abort();
            Started = false;
        }

    }
}
