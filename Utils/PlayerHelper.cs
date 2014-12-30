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
        public static Player addPlayer(Player p)
        {
            if (Globals.Players.Contains(p))
            {
                throw new NotSupportedException("This player is already added!");
            }
            else
            {
				Globals.PlayerOnline++;
				Globals.updateTitle ();
                Globals.Players.Add(p);
                return p;
            }
        }

		public static bool isConnectedPlayer(ClientWrapper client)
		{
			foreach (Player i in Globals.Players) 
			{
				if (i.Client == client) {
					return true;
				}
			}
			return false;
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

		public static Player getPlayer(ClientWrapper client)
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
                double _X = Convert.ToDouble(Splitted[2]);
                double _Y = Convert.ToDouble(Splitted[3]);
                double _Z = Convert.ToDouble(Splitted[4]);
                int __Gamemode = Convert.ToInt32(Splitted[5]);
                Player _Player = new Player() { Username = _Username, UUID = _UUID, Position = new Position() { X = _X, Y = _Y, Z = _Z }, Gamemode = new Gamemode(){ _Gamemode = __Gamemode} };
                return _Player;
            }

            else
            {
                throw new FileNotFoundException("No player found!");
            }
        }
    }
}
