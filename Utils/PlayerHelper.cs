using SharpMC.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SharpMC.Utils
{
    class PlayerHelper
    {
        public static bool addPlayer(Player p)
        {
            if (Globals.Players.Contains(p))
            {
                return false;
            }
            else
            {
				Globals.PlayerOnline++;
                Globals.Players.Add(p);
                return true;
            }
        }

        public static Player getPlayer(string UUID)
        {
            foreach (Player i in Globals.Players)
            {
                if (i.UUID == UUID)
                {
                    return i;
                }
            }
            return getFromPlayerFile(UUID);
        }

		public static Player getPlayer(TcpClient client)
		{
			foreach (Player i in Globals.Players) 
			{
				if (i.Client == client) 
				{
					return i;
				}
			}
			throw new Exception ("We didn't find any player...");
		}

        public static Player getFromPlayerFile(string UUID)
        {
            if (File.Exists("players/" + UUID + ".data"))
            {
                string Data = string.Empty;
                using (StreamReader sr = new StreamReader("players/" + UUID + ".data"))
                {
                    Data = sr.ReadToEnd();
                }
                string[] Splitted = Data.Split('|');
                string _Username = Utils.Base64.Decode(Splitted[0]);
                string _UUID = Utils.Base64.Decode(Splitted[1]);
                long _Position = Utils.Base64.DecodeToLong(Splitted[2]);
                Player _Player = new Player() { Username = _Username, UUID = _UUID, Position = new Position() { X = (int)Positions.GetX(_Position), Y = (int)Positions.GetY(_Position), Z = (int)Positions.GetZ(_Position) } };
                return _Player;
            }
            else
            {
               return new Player() { Username = "Unknown", UUID = UUID, Gamemode = new Gamemode() { _Gamemode = 1 }, Position = new Position() { X = 0, Y = 0, Z = 50} };
            }
        }
    }
}
