using System;
using System.Threading;
using Raven.Client.Embedded;

namespace Database.Models
{
    public class DatabaseServer
    {
        public EmbeddableDocumentStore Store;

        private Thread serverThread;
        public DatabaseServer(string location, bool server)
        {
            Store = new EmbeddableDocumentStore {DataDirectory = location, UseEmbeddedHttpServer = server};
        }

        public void StartServer()
        {
            serverThread = new Thread(() => Store.Initialize());
            serverThread.Start();
        }

        public void StopServer()
        {
            Store.Dispose();
            serverThread.Abort();
        }

    }
}
