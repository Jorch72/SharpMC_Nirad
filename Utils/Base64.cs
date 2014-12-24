using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMC.Utils
{
    class Base64
    {
        public static string Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Encode(byte[] plainData)
        {
            return System.Convert.ToBase64String(plainData);
        }

        public static string Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static long DecodeToLong(string base64EncodedData)
        {
            string data = Decode(base64EncodedData);
            long Data = Convert.ToInt64(data);
            return Data;
        }

        public static byte[] DecodeToByte(string base64EncodedData)
        {
            return Convert.FromBase64String(base64EncodedData);
        }
    }
}
