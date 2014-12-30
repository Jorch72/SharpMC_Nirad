using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Data
{
    class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

      /*  public byte[] ToBytes()
        {
            long pos = getPosition();
            byte[] Bytes = BitConverter.GetBytes(pos);
            return Bytes;
        } */

        public void setPosition(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void setPosition(long val)
        {
            long x = val >> 38;
            long y = (val >> 26) & 0xFFF;
            long z = val << 38 >> 38;

            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets the position to an int array.
        /// </summary>
        /// <returns>0 = X, 1 = Y, 2 = Z</returns>
     //   public long[] getPositionToArray()
      //  {
     //       long[] Data = new long[3];
     //       Data[1] = Y;
     //       Data[2] = Z;
       //     return Data;
      //  }

        public long getPosition()
        {
            long poop = (((int)X & 0x3FFFFFF) << 38) | (((int)Y & 0xFFF) << 26) | ((int)Z & 0x3FFFFFF);
            return poop;
        }
    }
}
