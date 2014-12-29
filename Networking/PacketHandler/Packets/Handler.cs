using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets
{
    class Handler
    {
        public int PacketID { get; set; }
        public virtual void Handle(ClientWrapper Client, byte[] Data) {  }
    }
}
