using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

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
			string Message = "Hello world";
			Message = "{ \"text\": \"" + Message + "\" }";
            byte[] _PacketID = Globals.getVarInt(PacketID);
			byte[] _Message = Encoding.UTF8.GetBytes (Message);
			byte[] _MSGLength = Globals.getVarInt (_Message.Length);
			byte[] _Size = Globals.getVarInt (_PacketID.Length + _Message.Length + _MSGLength.Length + 1);

			byte[] ToSend = Globals.concatBytes (_Size, _PacketID, _MSGLength, _Message);

			Network.SendResponse ((TcpClient)Client, ToSend);
            /*
             * Package seems to be bugged....
             * This needs to be fixed :>
             * But first:
             * WORLD GENERATION!
             */
        }
    }
}
