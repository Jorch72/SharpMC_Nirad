using SharpMC.Networking.PacketHandler.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Networking.PacketHandler
{
    class PacketHandler
    {
        public void HandlePacket(object Client, byte[] Data)
        {
            int PacketSize = Data[0];
            int PacketID = Data[1];

            switch (PacketID)
            {
                case 0x00:
                    new Handshake().Handle(Client, Data);
                    break;

                case 0x01:
                    new Ping().Handle(Client, Data);
                    break;

                case 0x04:
                    new PlayerPosition().Handle(Client, Data);
                    break;

                case 0x06:
                    new PlayerPositionAndLook().Handle(Client, Data);
                    break;

                default:
                  //  Console.WriteLine("[WARNING] Unknown packet received! ('" + PacketID + "')");
                    //Unknown packet received! ('" + PacketID + "')"
                    ConsoleFunctions.WriteWarningLine("Unknown packet received! ('" + PacketID + "')");
                    break;
            }

            #region Old
            //       LoadHandlers();
       //     bool found = false;

         //   int PacketSize = Data[0];
           // int PacketID = Data[1];
            //foreach(Handler i in PHandlers)
            //{
             //   Console.WriteLine("Handler PID: " + i.PacketID);
               // Console.WriteLine("Our Packet ID: " + PacketID);
                //if (i.PacketID == PacketID)
               // {
                 //   found = true;
                   // i.Handle(Client, Data);
                    //break;
                //}
            //}
            //if (!found)
            //{
            //    Console.WriteLine("No handler found for PacketID: " + PacketID);
            //    Console.WriteLine("Packet size: " + PacketSize);
            //}
            #endregion
        }
    }
}
