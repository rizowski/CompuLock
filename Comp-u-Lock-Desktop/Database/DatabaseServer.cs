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
            Store = new EmbeddableDocumentStore {DataDirectory = location, UseEmbeddedHttpServer = server};
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
