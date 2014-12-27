using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets.Ingoing
{
    class PlayerLook : Handler
    {
        public int PacketID
        {
            get
            {
                return 0x05;
            }
        }
        public override void Handle(object Client, byte[] Data)
        {

           //bool OnGround = BitConverter.ToBoolean(Data, 2);
            /*
             * Todo: Receive data.... (PlayerLook Packer)
             * Todo: DO SOMETHING WITH THE DATA!
             */
        }
    }
}
