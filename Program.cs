using SharpMC.Data;
using SharpMC.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SharpMC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#############################");
            Console.WriteLine("#       SharpMC 1.0.0       #");
            Console.WriteLine("#       Minecraft 1.8       #");
            Console.WriteLine("#     Written by Kennyvv    #");
            Console.WriteLine("#############################");
            ConsoleFunctions.WriteServerLine("Loading important stuf...");

            #region WorldGeneration
			 /*
			  * We need to have some nice world generation shit here.
			  * I have to implement this tho... ;(
			  */
			Globals.Generator.GenerateChunk(new Vector2(0,0));
            #endregion

            ConsoleFunctions.WriteServerLine("Preparing server for connections...");
            Thread serverThread = new Thread(() => new Server().ListenForClients());
            serverThread.Start();
            Console.Title = "SharpMC Server 1.0.0 - By Kennyvv/Wuppie";
        }
    }

    class Globals
    {
        public static TcpListener _ServerListener = new TcpListener(IPAddress.Any, 25565);
       // public static Thread _listenThread;
        public static int ProtocolVersion = 47;
        public static string ProtocolName = "1.8";
        private static string Title = "SharpMC";
        public static bool isDebug = false;

        public static string ServerMOTD = "SharpMC - By Kennyvv/Wuppie";
        public static int PlayerOnline = 0;
        public static int PlayersMax = 250;
        public static List<Player> Players = new List<Player>();
		public static FlatLandGenerator Generator = new FlatLandGenerator();

        public static int tovarint(int number)
        {
            int returnValue = 0;
            bool exit = false;

            while (!exit)
            {
                double digit = number % 128;
                if (digit == number)
                {
                    exit = true;
                }
                else
                {
                    number = number / 128;
                    digit += 128;
                }
                returnValue += (char)digit;
            }
            return returnValue;
        }

        public static string reverse(string a)
        {
            string temp = "";
            int i, j;
            for (j = 0, i = a.Length - 1; i >= 0; i--, j++)
                temp += a[i];
            return temp;
        }

        public static string jsonEncode(string text)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsLetterOrDigit(c))
                {
                    result.Append(c);
                }
                else
                {
                    result.Append("\\u");
                    result.Append(((int)c).ToString("x4"));
                }
            }
            return result.ToString();
        }

        public int readNextVarInt()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            byte[] tmp = new byte[1];
            while (true)
            {
              //  Receive(tmp, 0, 1, SocketFlags.None);
                k = tmp[0];
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }

        public static byte[] getVarInt(int paramInt)
        {
            List<byte> bytes = new List<byte>();
            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }

        public static byte[] concatBytes(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);
            byte[] RESULT = result.ToArray();

            byte[] poop = new byte[1];
            poop[0] = (byte)result.ToArray().Length;
            poop.CopyTo(RESULT, 0);
            return result.ToArray();
        }

        private static int _ActiveConnections = 0;
        public static int ActiveConnections
        {
            get
            {
                return _ActiveConnections;
            }
            set
            {
                _ActiveConnections = value;
                PlayerOnline = value;
              //  Console.Title = Title + " | Active connections: " + _ActiveConnections;
            }
        }

        public static int v2Int32(byte[] Data, int startIndex)
        {
            
            int shift = 0;
            int k = 0;
            int i = 0;
            for (int b = startIndex; b < Data.Length; b++)
            {
                k = Data[b];
                i |= ((k & 0x7F) << shift);
                if (shift > 7 * 5) throw new OverflowException("Varint is probably too big :O");
                if ((k & 0x80) != 128) break;
                shift += 7;   
            }
            return i;
        }


        public static string getUUID(string username)
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
            string[] _result = result.Split('"');
            string UUID = _result[3];
            return UUID;
        }

    }
}
