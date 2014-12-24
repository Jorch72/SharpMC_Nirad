using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC
{
    class Positions
    {
        public static int getPositionStructure(int x, int y, int z)
        {
            int poop = ((x & 0x3FFFFFF) << 38) | ((y & 0xFFF) << 26) | (z & 0x3FFFFFF);
            return poop;
        }

        public static long GetX(long Value)
        {
            long x = Value >> 38;
            long y = (Value >> 26) & 0xFFF;
            long z = Value << 38 >> 38;
            return x;
        }

        public static long GetY(long Value)
        {
            long x = Value >> 38;
            long y = (Value >> 26) & 0xFFF;
            long z = Value << 38 >> 38;
            return y;
        }

        public static long GetZ(long Value)
        {
            long x = Value >> 38;
            long y = (Value >> 26) & 0xFFF;
            long z = Value << 38 >> 38;
            return z;
        }
    }
}
