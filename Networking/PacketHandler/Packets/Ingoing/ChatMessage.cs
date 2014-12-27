using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets.Ingoing
{
	class ChatMessage : Handler
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
			/*
			 * We should handle chat messages here :)
			 */
		}

	}
}

