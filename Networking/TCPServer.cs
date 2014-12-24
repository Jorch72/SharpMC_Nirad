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
                TcpClient client = Globals._ServerListener.AcceptTcpClient();
                ConsoleFunctions.WriteDebugLine("A new connection has been made!");
                Globals.ActiveConnections++;

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommNew));
                clientThread.Start(client);
            }
        }

        private void HandleClientCommNew(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            //Buffer size of 4096 Bytes, reason: I guess we don't need more?
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
}