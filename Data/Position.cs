using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Data
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public byte[] ToBytes()
        {
            long pos = getPosition();
            byte[] Bytes = BitConverter.GetBytes(pos);
            return Bytes;
        }

        public void setPosition(long x, long y, long z)
        {
            X = (int)x;
            Y = (int)y;
            Z = (int)z;
        }

        public void setPosition(long val)
        {
            long x = val >> 38;
            long y = (val >> 26) & 0xFFF;
            long z = val << 38 >> 38;

            X = (int)x;
            Y = (int)y;
            Z = (int)z;
        }

        private void FromBytes(byte[] Bytes)
        {
            long val = getPosition();
            long x = val >> 38;
            long y = (val >> 26) & 0xFFF;
            long z = val << 38 >> 38;
        }

        public long getPosition()
        {
            long poop = ((X & 0x3FFFFFF) << 38) | ((Y & 0xFFF) << 26) | (Z & 0x3FFFFFF);
            return poop;
        }
    }
}
