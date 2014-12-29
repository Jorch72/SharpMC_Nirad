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
            ClientWrapper cWrapper = (ClientWrapper)Client;
			int[] _INT = Globals.v2Int32 (Data, 0);
			int PacketSize = _INT[0];
			int NextData = _INT[1];
			ConsoleFunctions.WriteDebugLine ("Packetsize: " + PacketSize.ToString() + " Next data: " + NextData.ToString());
			int PacketID = Globals.v2Int32(Data, NextData)[0];

            switch (PacketID)
            {
                case 0x00:
                    new Handshake().Handle(cWrapper, Data);
                    break;

				case 0x01:
					if (PacketSize == 9)
						new Ping ().Handle (cWrapper, Data);
					else
						new SharpMC.Networking.PacketHandler.Packets.Ingoing.ChatMessage().Handle (cWrapper, Data);
                    break;

                case 0x04:
                    new PlayerPosition().Handle(cWrapper, Data);
                    break;

                case 0x06:
                    new PlayerPositionAndLook().Handle(cWrapper, Data);
                    break;

                case 0x03:
                    new SharpMC.Networking.PacketHandler.Packets.Ingoing.PlayerOnGround().Handle(cWrapper, Data);
                    break;

                case 0x05:
                    new SharpMC.Networking.PacketHandler.Packets.Ingoing.PlayerLook().Handle(cWrapper, Data);
                    break;

                default:
                    ConsoleFunctions.WriteWarningLine("Unknown packet received! ('" + PacketID + "')");
                    break;
            }

        }
    }
}
