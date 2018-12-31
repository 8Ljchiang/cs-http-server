using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private Router _router;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public Server(Router router)
        {
            _router = router;
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

        public void ListenWithTCPListener(int port)
        {
            TcpListener server = null;

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(ipAddress, port);

                // Start listening for client requests.
                server.Start();
                Console.WriteLine($"{ipAddress}:{port} is starting listener socket...");

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine(" *** Waiting for connection...");
                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    ShowConnectionInfo(client);

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

        private void HandleListen()
        {

        }

        private void HandleConnection()
        {

        }

        private void HandleError()
        {

        }

        private void Initialize()
        {

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
            Console.WriteLine("----- {0} connected at {1} -----", client.ToString(), new DateTime().ToString());
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