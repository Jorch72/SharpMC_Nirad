using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SharpMC.Networking
{
    class Server
    {
        public void ListenForClients()
        {
            Globals._ServerListener.Start();
            ConsoleFunctions.WriteServerLine("Ready for connections...");
            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = Globals._ServerListener.AcceptTcpClient();
                ConsoleFunctions.WriteServerLine("A new connection has been made!");
                Globals.ActiveConnections++;

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommNew));
                clientThread.Start(client);
            }
        }

        private void HandleClientCommNew(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;
            while (true)
            {
                bytesRead = 0;
                try
                {
                    //if (clientStream.DataAvailable)
                        bytesRead = clientStream.Read(message, 0, 4096);
                    if (bytesRead > 0)
                    {
                        ConsoleFunctions.WriteDebugLine("Packet received. Time: " + DateTime.Now.ToLocalTime());
                        ConsoleFunctions.WriteDebugLine("Packet ID: " + message[1]);
                        #region OldSwitch
                        /*  switch (message[1])
                        {
                            case 0x00:
                                Console.WriteLine("[DEBUG] 0x00 received!");
                                // Console.WriteLine("Ping received! (" + message[1].ToString() + ")");
                                Console.WriteLine("Length info: " + message[0] + "/" + message.Length);

                                Console.Write("Type of request: ");
                                if (message[0] == 15)
                                {
                                    Console.Write("Handshake \n");
                                    Console.WriteLine("Protocol version: " + message[2]);
                                    string Host = Encoding.UTF8.GetString(message, 4, message[3]);
                                    ushort actualPort;
                                    if (BitConverter.IsLittleEndian)
                                        actualPort = BitConverter.ToUInt16(new byte[2] { (byte)message[14], (byte)message[13] }, 0);
                                    else
                                        actualPort = BitConverter.ToUInt16(new byte[2] { (byte)message[13], (byte)message[14] }, 0);
                                    Console.WriteLine("Server adress: " + Host + ":" + actualPort.ToString());
                                    Console.WriteLine("Next state: " + message[16]);
                                }
                                else if (message[0] == 1)
                                {
                                    Console.WriteLine("Status request \n");
                                   // StatusResponse(tcpClient, message);
                                }
                                else
                                {
                                    Console.WriteLine("WTF?! 0x00 with a length of: " + message[0]);
                                }
                                Console.WriteLine();

                                break;
                            case 0x01:
                                Networking.Network.SendResponse(tcpClient, message);
                                Console.WriteLine("[DEBUG] 0x01 received!");
                                // Console.WriteLine("Ping package received! (" + message[1].ToString() + ")");
                                Console.WriteLine("Length info: " + message[0] + "/" + message.Length);
                                break;
                            default:
                                Console.WriteLine("Unknown packet! Ignoring! (" + message[1].ToString() + ")");
                                break;
                        }
                       * */
                        #endregion

                        PacketHandler.PacketHandler PH = new PacketHandler.PacketHandler();
                        Thread handler = new Thread(() => PH.HandlePacket(tcpClient, message));
                        handler.Start();

                    }
                    if (bytesRead == 0)
                    {
                        //Close connection with user. as he disconnected!
                        break;
                    }
                }
                catch(Exception ex)
                {
                    ConsoleFunctions.WriteErrorLine("ERROR! \n" + ex.Message);
                    break;
                }
                
            }
            Globals.ActiveConnections--;
            tcpClient.Close();
        }

    }
    

    #region NOPE!
    class TcpServerV2
    {
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        void ProcessIncomingData(object obj)
        {
            TcpClient client = (TcpClient)obj;
          //  StringBuilder sb = new StringBuilder();
            List<byte> d = new List<byte>();
            Byte[] Data = new Byte[4096];
           
            using (NetworkStream stream = client.GetStream())
            {
                if (stream.DataAvailable)
                {
                    stream.Read(Data, 0, 4096);
                }

                if (Data.Length > 0)
                {
                    //We actually received Data here!
                    int Length = Data[2];
                    int ourLength = Data.Length;
                  //  ConsoleFunctions.WriteDebugLine(ourLength + "/" + Length);

                    PacketHandler.PacketHandler PH = new PacketHandler.PacketHandler();
                    Thread handler = new Thread(() => PH.HandlePacket(client, Data));
                    handler.Start();

                }
            }
        }

        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);

            ThreadPool.QueueUserWorkItem(ProcessIncomingData, client);
            tcpClientConnected.Set();
        }

        public void start(int Port)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Port);
            TcpListener listener = new TcpListener(endpoint);
            listener.Start();

            while (true)
            {
                tcpClientConnected.Reset();
                listener.BeginAcceptTcpClient(new AsyncCallback(ProcessIncomingConnection), listener);
                tcpClientConnected.WaitOne();
            }
        }
    }

    #endregion

    #region AlsoNope
    class TcpServer
    {
        private PacketHandler.PacketHandler PH = new PacketHandler.PacketHandler();
        private TcpListener _server;
        private Boolean _isRunning;

        public TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();

            _isRunning = true;

            Thread ClientLoop = new Thread(() => LoopClients());
            ClientLoop.Start();
            ConsoleFunctions.WriteServerLine("TCP Server started!");
            //LoopClients();
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();

                // client found.
                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            // sets two streams
          //  StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
          //  StreamReader sReader = new StreamReader(client.GetStream(), Encoding.UTF8);
            NetworkStream clientStream = client.GetStream();
            
            //Boolean bClientConnected = true;
            // sData = null;
            int Received = 0;
            byte[] Buffer = new byte[4 * 1024];
            while (client.Connected)
            {
                //Wait for first data...
                ConsoleFunctions.WriteDebugLine("Waiting for data...");
                try
                {
                    Received = clientStream.Read(Buffer, 0, (4 * 1024));
                }
                catch(Exception ex)
                {
                    ConsoleFunctions.WriteErrorLine("We had an error! \n" + ex.Message);
                }
                if (Received == 0)
                {
                    //Our client disconnected, Handle it.
                    ConsoleFunctions.WriteErrorLine("We received 0 bytes, did a client disconnect?");
                    client.Close();
                   // client.Close();
                    return;
                }

                if (Received > 0)
                {
                    //We received actual data.
                    //Lets handle it.
                    Thread handler = new Thread(() => PH.HandlePacket(client, Buffer));
                    handler.Start();
                }
            }
            client.Close();
        }
    }
    #endregion
}
