using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets.Outgoing
{
    class ChatMessage : Handler
    {
        public int PacketID
        {
            get
            {
                return 0x02;
            }
        }
        public override void Handle(object Client, byte[] Data)
        {
            byte[] _PacketID = Globals.getVarInt(PacketID);
            /*
             * This package has to be finished. But first: World generation!
             */
        }
    }
}
