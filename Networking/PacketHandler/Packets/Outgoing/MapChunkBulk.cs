using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets.Outgoing
{
    class MapChunkBulk : Handler
    {
		public int PacketID
        {
            get
            {
                return 0x26;
            }
        }
        public override void Handle(ClientWrapper Client, byte[] Data)
        {
			/*
             *	I have no idea howto do the world generation! :(
             *	So, i cannot really do a packet that sends chunks...
             *	Can i? :P
             */
        }
    }
}
