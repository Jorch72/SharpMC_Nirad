using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMC.Utils;

namespace SharpMC.Networking.PacketHandler.Packets
{
    class PlayerPosition : Handler
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
            double y = BitConverter.ToDouble(Data, 11);
            double z = BitConverter.ToDouble(Data, 20);
            bool onGround = BitConverter.ToBoolean(Data, 29);

            if (Client._Player != null)
                Client._Player.Position.setPosition((long)X, (long)y, (long)z);
            /* TODO:
             * FIX THIS SHIT!@!@@#!
             * 
             * Maybe save onGround, however i don't think it is very important ATM.
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
