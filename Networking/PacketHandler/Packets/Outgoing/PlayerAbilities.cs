using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets.Outgoing
{
    class PlayerAbilities : Handler
    {
        public int PacketID
        {
            get
            {
                return 0x39;
            }
        }
        public override void Handle(object Client, byte[] Data)
        {
            byte[] _PacketID = Globals.getVarInt(PacketID);
            byte[] _Flags = new byte[1];
            _Flags[0] = 0000;

            byte[] FlyinSpeed = BitConverter.GetBytes(1f);
            byte[] WalkinSpeed = BitConverter.GetBytes(1f);

            byte[] TotalSize = Globals.getVarInt(_PacketID.Length + _Flags.Length + FlyinSpeed.Length + WalkinSpeed.Length);
            byte[] ToSend = Globals.concatBytes(TotalSize, _PacketID, _Flags, FlyinSpeed, WalkinSpeed);
            Network.SendResponse((TcpClient)Client, ToSend);
        }
    }
}
