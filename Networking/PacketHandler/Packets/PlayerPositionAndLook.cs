using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets
{
    class PlayerPositionAndLook : Handler
    {
        public int PacketID
        {
            get
            {
                return 0x06;
            }
        }

        public override void Handle(ClientWrapper Client, byte[] Data)
        {
            int DataLength = Globals.v2Int32(Data, 1)[0];
            double X = BitConverter.ToDouble(Data, 2);
            double y = BitConverter.ToDouble(Data, 11);
            double z = BitConverter.ToDouble(Data, 19);
            float yaw = BitConverter.ToSingle(Data, 5);
            float pitch = BitConverter.ToSingle(Data, 6);
            bool onGround = BitConverter.ToBoolean(Data, 7);

            if (Client._Player != null)
                Client._Player.Position.setPosition(X, y, z);
            /* TODO:
             * Save the received information...
            */
        }

     //   private double Read8Bytes(byte[] Data, int offset)
    //    {
         //   byte[] d = new byte[8];
        //    Array.Copy(Data, offset, d, 0, 8);
       //     BitConverter.ToDouble()
      //  }
    }
}
