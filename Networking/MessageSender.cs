using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharpMC.Networking
{
    class Network
    {
        public static void SendResponse(TcpClient tcpClient, byte[] Data)
        {
         //   List<byte> actData = new List<byte>(Data[0] + 1);
           // for (int i = 0; i < (Data[0] + 1); i++)
            //{
             //   actData.Add(Data[i]);
           // }
            tcpClient.NoDelay = false;
                NetworkStream clientStream = tcpClient.GetStream();
              
                clientStream.Write(Data, 0, Data.Length);
                clientStream.Flush();

                ConsoleFunctions.WriteDebugLine("Packet send with Packet ID: " + Data[1]);
                ConsoleFunctions.WriteDebugLine("Packet send with Packet Length: " + Data[0]);
                ConsoleFunctions.WriteDebugLine("Actual packet length: " + Data.Length);
                
        }
    }
}
