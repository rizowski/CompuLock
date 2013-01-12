using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TrotiNet;

namespace Service
{
    /*class TrackHistory
    {
        public TrackHistory(int port)
        {
            var server = new TcpServer(port, false);
            server.Start(TransparentProxy.CreateProxy);

            server.InitListenFinished.WaitOne();
            if (server.InitListenException != null)
                throw server.InitListenException;
            
        }

        private class TransparentProxy : ProxyLogic
        {
            public TransparentProxy(HttpSocket socketBP) : base(socketBP)
            {
            }

            static new public TransparentProxy CreateProxy(HttpSocket clientSocket)
            {
                return new TransparentProxy(clientSocket);
            }

            protected override void OnReceiveRequest()
            {
                Console.WriteLine("-> " + RequestLine + " from HTTP referer " +
                    RequestHeaders.Referer);
            }

            protected override void OnReceiveResponse()
            {
                Console.WriteLine("<- " + ResponseStatusLine +
                    " with HTTP Content-Length: " +
                    (ResponseHeaders.ContentLength ?? 0));
            }
        }*/
    /*class HistoryProxy
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
                    client.Receive(requestBuffer);
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
#if (DEBUG)
                Console.WriteLine("Raw Request Received...");
                Console.WriteLine(request);
#endif

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
                    client.Send(responseBuffer);
                }

                destServerSocket.Disconnect(false);
                destServerSocket.Dispose();
                client.Disconnect(false);
                client.Dispose();
            }
            catch (Exception e)
            {
                // If the host isn't valid there is no point in storing it. More research into this later.
#if (DEBUG)
                Console.WriteLine("Error Occured: " + e.Message);
                //Console.WriteLine(e.StackTrace);
#endif
            }
        }

        public void SendResponse(string response)
        {
            
        }

        
    }*/
    /*//http://msdn.microsoft.com/en-us/library/system.net.sockets.tcplistener.aspx
    public delegate void Request(object sender, RequestEventArgs e);

    public class HistoryProxy2
    {
        public event Request OnRequest;

        
        public HistoryProxy2()
        {
            
        }

    }

    public class RequestEventArgs : EventArgs
    {
        public string Header;
        public string RequestBody;
    }*/

    /*public class TrackHistory
    {
        public int port;
        public IPAddress address;
        public TcpListener server;
        public TrackHistory(IPAddress address, int port)
        {
            try
            {
                // Set the TcpListener on port 13000.
                this.port = port;
                this.address = address;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(address, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("<- {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("-> {0}", data);
                    }
                    Console.WriteLine("Closing??");
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }*/

    public class Proxy
    {
        public IPAddress Address;
        public int Port;
        
        public TcpListener Server;
        private int ByteSize;
        private string Data;

        public Proxy(IPAddress address, int port, int bytesize=256)
        {
            Address = address;
            Port = port;
            ByteSize = bytesize;
            
            Server = new TcpListener(Address, Port);
        }

        //starts the TCPSERVER then waits for a connection.
        public void Start()
        {
            Server.Start();
            Begin();
        }

        private void Begin()
        {
            Byte[] bytes = new Byte[ByteSize];
            while (true)
            {
                int i;
                string data;

                Console.WriteLine("Waiting for connection...");
                TcpClient client =  Server.AcceptTcpClient();
                NetworkStream clientStream = client.GetStream();

                while ((i = clientStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = Encoding.ASCII.GetString(bytes, 0, i);
#if (Debug)
                    Console.WriteLine("-> {0}", Data);
#endif

                    // Process the data sent by the client.
                    data = data.ToUpper();
                    var head = new HttpHeader(data);
                    var headerlines = head.Split();
                    if (head.Secure)
                    {
                        // confirm with client and tunnel
                    }
                    else
                    {
                        // proceed like normal
                    }
                }
                
            }
        }


        //To client = Server Response
        /*private void WriteResponse()
        {
            while (true)
            {
                byte[] msg = Encoding.ASCII.GetBytes(data);

                // Send back a response.
                clientStream.Write(msg, 0, msg.Length);
                Console.WriteLine("-> {0}", data);
            }
            
        }*/

        public void Stop()
        {
            Server.Stop();
        }

        
    }

    public class HttpHeader
    {
        public string OriginalRequst;
        
        public string Method { get { return _httpmethod; } }
        public string FileRequest { get { return _file; } }
        public string Protocol { get { return _httProtocol; } }
        public string[] HttpOptions { get { return _arguments; } }
        public bool Secure { get
        {
            if (Method == "CONNECT")
            {
                return true;
            }
            return false;
        } }

        private string[] _arguments;
        private string _httProtocol;
        private string _httpmethod;
        private string _file;
        private const string LINEND = "\r\n";

        public HttpHeader(string data)
        {
            OriginalRequst = data;
        }

        public string[] Split(string httpHeader = null)
        {
            _arguments = Regex.Split(httpHeader ?? OriginalRequst, LINEND);
            string[] firstLine = _arguments[0].Split(' ');
            _httpmethod = firstLine[0];
            _file = firstLine[1];
            _httProtocol = firstLine[2];

            return _arguments;
        }
    }
}
