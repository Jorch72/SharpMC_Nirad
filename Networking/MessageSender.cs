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
            try
            {
                tcpClient.NoDelay = false;
                NetworkStream clientStream = tcpClient.GetStream();

                clientStream.Write(Data, 0, Data.Length);
                clientStream.Flush();

            }
            catch(Exception ex)
            {
                ConsoleFunctions.WriteErrorLine("FUCK, We failed to send a packet... The following error occured: " + ex.Message);
            }
                ConsoleFunctions.WriteDebugLine("Packet send with Packet ID: " + Data[1]);
                ConsoleFunctions.WriteDebugLine("Packet send with Packet Length: " + Data[0]);
                ConsoleFunctions.WriteDebugLine("Actual packet length: " + Data.Length);
                
        }
    }
}
