using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SharpMC.Data
{
    class Player
    {
        public string Username { get; set; }
        public string UUID { get; set; }
        public Position Position { get; set; }
        public Gamemode Gamemode { get; set; }
		public ClientWrapper Client { get; set; }

        public Player()
        {
            
        }

        public void SaveToFile()
        {
            if (!Directory.Exists("players/"))
                Directory.CreateDirectory("players/");

            //byte[] _Username = Encoding.UTF8.GetBytes(Username);
           // byte[] _UUID = Encoding.UTF8.GetBytes(UUID);
           // byte[] _Position = Position.ToBytes();

            string __Username = Utils.Base64.Encode(Username);
            string __UUID = Utils.Base64.Encode(UUID);
           // string __Position = Utils.Base64.Encode(Position.getPosition().ToString());
            int __GameMode = Gamemode._Gamemode;
            string FullString = __Username + "|" + __UUID + "|" + Position.X + "|" + Position.Y + "|" + Position.Z + "|" + __GameMode;

           // if (!File.Exists("players/" + UUID + ".data"))
               // File.Create("players/" + UUID + ".data");
            File.WriteAllText("players/" + UUID + ".data", FullString);
        }
    }
}
