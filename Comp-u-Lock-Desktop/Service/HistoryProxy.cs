using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Service
{
    class HistoryProxy
    {
        
        private int Port;
        private TcpListener Listener;
        
        public HistoryProxy(int port)
        {
            Port = port;
            Listener = new TcpListener(IPAddress.Any, Port);
        }

        public void Start()
        {
            Listener.Start();
        }

        public void Accept()
        {
            var client = Listener.AcceptSocket();
            var clientConnection = new ClientConnection(client);
            clientConnection.StartHandle();
        }
    }

    class ClientConnection
    {
        private Socket client;
        public ClientConnection(Socket client)
        {
            this.client = client;
        }

        public void StartHandle()
        {
            Thread handler = new Thread(Handler);
            //handler.Priority = ThreadPriority.AboveNormal;
            handler.Start();
        }

        public void Handler()
        {
            bool received = true;
            string end = "\r\n";

            string request = "";
            string requestTempLine = "";
            List<string> requestLines = new List<string>();
            byte[] requestBuffer = new byte[1];
            byte[] responseBuffer = new byte[1];

            requestLines.Clear();

            try
            {
                //handling request
                while (received)
                {
                    this.client.Receive(requestBuffer);
                    string fromByte = ASCIIEncoding.ASCII.GetString(requestBuffer);
                    request += fromByte;
                    requestTempLine += fromByte;

                    if (requestTempLine.EndsWith(end))
                    {
                        requestLines.Add(requestTempLine.Trim());
                        requestTempLine = "";
                    }

                    if (request.EndsWith(end + end))
                    {
                        received = false;
                    }
                }
                Console.WriteLine("Raw Request Received...");
                Console.WriteLine(request);

                //rebuild header and send back to host
                string remoteHost = requestLines[0].Split(' ')[1].Replace("http://", "").Split('/')[0];
                string requestFile = requestLines[0].Replace("http://", "").Replace(remoteHost, "");
                requestLines[0] = requestFile;

                request = "";
                foreach (string line in requestLines)
                {
                    request += line;
                    request += end;
                }

                Socket destServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                destServerSocket.Connect(remoteHost, 80);
        
                destServerSocket.Send(ASCIIEncoding.ASCII.GetBytes(request));

                while (destServerSocket.Receive(responseBuffer) != 0)
                {
                    //Console.Write(ASCIIEncoding.ASCII.GetString(responseBuffer));
                    this.client.Send(responseBuffer);
                }

                destServerSocket.Disconnect(false);
                destServerSocket.Dispose();
                this.client.Disconnect(false);
                this.client.Dispose();
            }
            catch (Exception e)
            {
                // If the host isn't valid there is no point in storing it. More research into this later.
                Console.WriteLine("Error Occured: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
