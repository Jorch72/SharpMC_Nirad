using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets
{
    class Ping : Handler
    {
        public int PacketID
        {
            get
            {
                return 0x01;
            }
        }
        public override void Handle(object Client, byte[] Data)
        {
            ConsoleFunctions.WriteDebugLine("Handling PING Request!");
            TcpClient client = (TcpClient)Client;
            Network.SendResponse(client, Data);
        }
    }
}
