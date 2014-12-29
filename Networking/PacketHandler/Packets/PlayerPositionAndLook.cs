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
                return 0x04;
            }
        }

        public override void Handle(ClientWrapper Client, byte[] Data)
        {
            int DataLength = Globals.v2Int32(Data, 1)[0];
            double X = BitConverter.ToDouble(Data, 2);
            double y = BitConverter.ToDouble(Data, 3);
            double z = BitConverter.ToDouble(Data, 4);
            float yaw = BitConverter.ToSingle(Data, 5);
            float pitch = BitConverter.ToSingle(Data, 6);
            bool onGround = BitConverter.ToBoolean(Data, 7);

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
