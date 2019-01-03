using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private Router _router;
        private readonly Boolean _isBlocking = true;
        private TcpListener _listener;
        private CommandDispatcher _dispatcher;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public Server(Router router, bool isBlocking = true)
        {
            _router = router;
            _isBlocking = isBlocking;
            Initialize();
        }

        public void On(string eventType, Action<Dictionary<string, object>> handler)
        {
            _dispatcher.RegisterCommandHandler(eventType, handler);
        }

        public void ListenWithTCPListener(int port)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("port", port);
            CallEventWithDispatcher("listen", payload);

            AcceptConnections();

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private void AcceptConnections() 
        {
            // Enter the listening loop.
            while (true)
            {
                Console.WriteLine(" *** Waiting for connection...");

                Dictionary<string, object> payload = new Dictionary<string, object>();
                try
                {
                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    // TODO: Change this to non-blocking.

                    if (_listener.Pending())
                    {
                        TcpClient client = _listener.AcceptTcpClient();
                        ShowConnectionInfo(client);

                        payload.Add("connection", client);
                    }
                    else
                    {
                        Console.WriteLine("No pending connections...");
                    }
                }
                catch (Exception ex)
                {
                    //HandleError(ex.Message);
                    Dictionary<string, object> errorPayload = new Dictionary<string, object>()
                    {
                        { "error", ex.Message }
                    };

                    CallEventWithDispatcher("error", errorPayload);
                }
                finally
                {
                    //HandleConnection((TcpClient) payload["connection"]);
                    CallEventWithDispatcher("connection", payload);
                }
            }
        }

        private void HandleClose()
        {
            // Stop listening for new clients.
            _listener.Stop();
        }

        private void HandleListen(int port)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                // TcpListener server = new TcpListener(port);
                _listener = new TcpListener(ipAddress, port);

                // Start listening for client requests.
                _listener.Start();
                Console.WriteLine($"{ipAddress}:{port} is starting listener socket...");
            }
            catch (SocketException ex)
            {
                //HandleError(ex.Message);
                //Console.WriteLine("SocketException: {0}", e);
                Dictionary<string, object> errorPayload = new Dictionary<string, object>()
                    {
                        { "error", ex.Message }
                    };

                CallEventWithDispatcher("error", errorPayload);
                CallEventWithDispatcher("close", errorPayload);
            }
        }

        private void HandleConnection(TcpClient client)
        {
            try
            {
                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                string requestString = GetDataFromStream(stream);
                Console.WriteLine("Received:");
                Console.WriteLine(requestString);

                Request clientRequest = RequestBuilder.CreateRequest(requestString);

                Response response = _router.HandleRequest(clientRequest);

                string responseString = ResponseBuilder.CreateResponseString(response);

                // Send back a response.
                byte[] msg = Encoding.ASCII.GetBytes(responseString);
                stream.Write(msg, 0, msg.Length);

                Console.WriteLine("Sent:");
                Console.WriteLine(responseString);
            }
            catch (InvalidPathException ex)
            {
                Dictionary<string, object> errorPayload = new Dictionary<string, object>()
                    {
                        { "error", ex.Message }
                    };

                CallEventWithDispatcher("error", errorPayload);
            }
            catch (InvalidMethodException ex)
            {
                Dictionary<string, object> errorPayload = new Dictionary<string, object>()
                    {
                        { "error", ex.Message }
                    };

                CallEventWithDispatcher("error", errorPayload);
            }
            catch (InvalidRequestStringException ex)
            {
                Dictionary<string, object> errorPayload = new Dictionary<string, object>()
                    {
                        { "error", ex.Message }
                    };

                CallEventWithDispatcher("error", errorPayload);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> errorPayload = new Dictionary<string, object>()
                    {
                        { "error", ex.Message }
                    };

                CallEventWithDispatcher("error", errorPayload);
            }
            finally
            {
                // Shutdown and end connection
                client.Close();
            }
        }

        private void HandleError(string message)
        {
            Console.WriteLine(" *** Error:\n {0}\n", message);
        }

        private void Initialize()
        {
            _dispatcher = new CommandDispatcher();

            void ListenHandler(Dictionary<string, object> payload)
            {
                HandleListen((int)payload["port"]);
            }

            void ErrorHandler(Dictionary<string, object> payload)
            {
                HandleError((string)payload["error"]);
            }

            void ConnectionHandler(Dictionary<string, object> payload)
            {
                HandleConnection((TcpClient)payload["connection"]);
            }

            void CloseHandler(Dictionary<string, object> payload)
            {
                HandleClose();
            }

            _dispatcher.RegisterCommandHandler("listen", ListenHandler);
            _dispatcher.RegisterCommandHandler("error", ErrorHandler);
            _dispatcher.RegisterCommandHandler("connection", ConnectionHandler);
            _dispatcher.RegisterCommandHandler("close", CloseHandler);
        }

        private void CallEventWithDispatcher(string eventType, Dictionary<string, object> payload)
        {
            if (_isBlocking)
            {
                _dispatcher.Process(eventType, payload);
            }
            else
            {
                _dispatcher.ProcessAsync(eventType, payload);
            }
        }

        private string GetDataFromStream(NetworkStream stream)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            Byte[] bytes = new Byte[256];
            String data = null;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = Encoding.ASCII.GetString(bytes, 0, i);

                // Process the data sent by the client.
                //data = data.ToUpper();

                sb.AppendLine(data);

                if (i < bytes.Length)
                {
                    break;
                }
            }

            string requestString = sb.ToString();
            return requestString;
        }

        private void ShowConnectionInfo(TcpClient client)
        {
            Console.WriteLine("----- {0} connected at {1} -----", client.ToString(), DateTime.Now.ToString());
        }

        public void Listen(int port)
        {
            // 0. Define parameters for listener constructor.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // 1a. Define TCP/IP socket Listener parameters. 
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 1.b Bind and start listener.
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {

                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    //listener.BeginAccept(
                    //new AsyncCallback(AcceptCallback),
                    //listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();


                }

                // 2. Accept Incoming Conncetions

                // 3. Get data from Connection.

                // 4. Convert data to string.

                // 5. Convert string to Request Object.

                // 6. Process Request object and Build response object.

                // 7. Convert Response object to string.

                // 8. Convert response string to data.

                // 9. Send data to connection.

                // 10. Close connections

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
//Socket client = listener.AcceptSocket();
//Console.WriteLine("Connection accepted.");

//var childSocketThread = new Thread(() =>
//{
//    byte[] data = new byte[100];
//    int size = client.Receive(data);
//    Console.WriteLine("Recieved data: ");
//    for (int i = 0; i < size; i++)
//        Console.Write(Convert.ToChar(data[i]));

//    Console.WriteLine();

//    client.Close();
//});
//childSocketThread.Start();