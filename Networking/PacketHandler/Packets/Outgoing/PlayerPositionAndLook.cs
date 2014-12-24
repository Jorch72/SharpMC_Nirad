using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets.Outgoing
{
    class PlayerPositionAndLook : Handler
    {
         public int PacketID
        {
            get
            {
                return 0x08;
            }
        }
         public override void Handle(object Client, byte[] Data)
         {
             byte[] _PacketID = Globals.getVarInt(PacketID);
             byte[] X = BitConverter.GetBytes((double)0);
             byte[] Y = BitConverter.GetBytes((double)0);
             byte[] Z = BitConverter.GetBytes((double)50);
             byte[] Yaw = BitConverter.GetBytes(0f);
             byte[] Pitch = BitConverter.GetBytes(0f);
             byte[] Flags = new byte[1];
             Flags[0] = 0;

             byte[] TotalSize = Globals.getVarInt(_PacketID.Length + X.Length + Y.Length + Z.Length + Yaw.Length + Pitch.Length + Flags.Length);
             byte[] ToSend = Globals.concatBytes(TotalSize, _PacketID, X, Y, Z, Yaw, Pitch, Flags);
             Network.SendResponse((TcpClient)Client, ToSend);
         }
    }
}
