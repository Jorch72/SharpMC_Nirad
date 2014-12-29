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
				Globals.updateTitle ();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommNew));
                clientThread.Start(client);
            }
        }

        private void HandleClientCommNew(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            ClientWrapper Client = new ClientWrapper(tcpClient);

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
						ConsoleFunctions.WriteDebugLine("Packet ID: " + Globals.v2Int32(message, 1)[0]);

                        PacketHandler.PacketHandler PH = new PacketHandler.PacketHandler();
                        Thread handler = new Thread(() => PH.HandlePacket(Client, message));
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
			ConsoleFunctions.WriteDebugLine ("A client disconnected!");
			if (Utils.PlayerHelper.isConnectedPlayer (Client)) 
			{
				ConsoleFunctions.WriteInfoLine("Player '" + Utils.PlayerHelper.getPlayer(Client).Username + "' disconnected!");
				Globals.Players.Remove (Utils.PlayerHelper.getPlayer (Client));
				Globals.PlayerOnline--;
				Globals.updateTitle ();
			}
            tcpClient.Close();
			Globals.ActiveConnections--;
			Globals.updateTitle ();
        }

    }
}