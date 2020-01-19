using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sauna.RPI
{
    internal class StateObject
    {
        public Socket WorkSocket { get; set; }

        public const int BufferSize = 1024;

        public byte[] Buffer { get; private set; } = new byte[BufferSize];

        public StringBuilder Payload { get; private set; } = new StringBuilder();
    }

    internal class Server
    {
        internal ManualResetEvent AllDone;

        internal event Action<string> CommandReceived;

        public Server()
        {
            AllDone = new ManualResetEvent(false);
        }

        /// <summary>
        /// Listen for incoming connection on localhost
        /// </summary>
        /// <returns></returns>
        internal async Task StartListenAsync()
        {
            //var ipHostInfo = Dns.GetHostEntry("127.0.0.1");
            var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 31337);

            Console.WriteLine($"Local address and port : {localEndpoint.ToString()}");

            var listener = new Socket(localEndpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await Task.Run(() =>
            {
               try
               {
                   listener.Bind(localEndpoint);
                   listener.Listen(10);

                   while (true)
                   {
                       AllDone.Reset();

                       Console.WriteLine("Waiting for a connection...");
                       listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                       AllDone.WaitOne();
                   }
               }
               catch (Exception e)
               {
                   Console.WriteLine(e.ToString());
               }

           }).ConfigureAwait(false);

            Console.WriteLine("Closing the listener...");
        }

        /// <summary>
        /// Accept an incoming connection and start receiving data on it
        /// </summary>
        /// <param name="result"></param>
        void AcceptCallback(IAsyncResult result)
        {
            // Get the socket that handles the client request.  
            var listener = result.AsyncState as Socket;
            var handler = listener.EndAccept(result);

            // Signal the main thread to continue.  
            AllDone.Set();

            // Create the state object.  
            var state = new StateObject() { WorkSocket = handler };

            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        /// <summary>
        /// Wait until the connection is closed until processing the received data
        /// </summary>
        /// <param name="result"></param>
        void ReadCallback(IAsyncResult result)
        {
            var state = result.AsyncState as StateObject;
            var handler = state.WorkSocket;

            // Read data from the client socket.  
            var read = handler.EndReceive(result);

            // Data was read from the client socket.  
            if (read > 0)
            {
                state.Payload.Append(Encoding.ASCII.GetString(state.Buffer, 0, read));

                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            else
            {
                if (state.Payload.Length > 1)
                {
                    // All the data has been read from the client;  
                    // display it on the console.  
                    var command = state.Payload.ToString();
                    Console.WriteLine($"Read {command.Length} bytes from socket.\n Data : {command}");

                    ProcessCommand(state.WorkSocket, command);
                }

                handler.Close();
            }
        }

        void ProcessCommand(Socket socket, string command)
        {
            Console.WriteLine($"Received command: {command}");

            CommandReceived?.Invoke(command);
        }
    }
}