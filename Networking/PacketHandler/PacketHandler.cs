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
			int[] _INT = Globals.v2Int32 (Data, 0);
			int PacketSize = _INT[0];
			int NextData = _INT[1];
			ConsoleFunctions.WriteDebugLine ("Packetsize: " + PacketSize.ToString() + " Next data: " + NextData.ToString());
			int PacketID = Globals.v2Int32(Data, NextData)[0];

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

                case 0x03:
                    new SharpMC.Networking.PacketHandler.Packets.Ingoing.PlayerOnGround().Handle(Client, Data);
                    break;

                case 0x05:
                    new SharpMC.Networking.PacketHandler.Packets.Ingoing.PlayerLook().Handle(Client, Data);
                    break;

                default:
                    ConsoleFunctions.WriteWarningLine("Unknown packet received! ('" + PacketID + "')");
                    break;
            }

        }
    }
}
