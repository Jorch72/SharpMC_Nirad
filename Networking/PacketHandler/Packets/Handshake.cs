using SharpMC.Data;
using SharpMC.Networking.PacketHandler.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharpMC.Networking.PacketHandler.Packets
{
    class Handshake : Handler
    {
        public int PacketID
        {
            get
            {
                return 0x00;
            }
        }
        public override void Handle(ClientWrapper Client, byte[] Data)
        {
            ClientWrapper tcpClient = Client;
				/*
				 * I know Host and ActualPort are currently not used.
				 * I will use them to verify people are not using a proxy later on.
				 */ 
            int[] _Hostdata = Globals.v2Int32(Data, 3);
            int HostLength = _Hostdata[0];
            int NextData = _Hostdata[1];

            string Host = Encoding.UTF8.GetString(Data, NextData, HostLength);

                ushort actualPort;
                if (BitConverter.IsLittleEndian)
                    actualPort = BitConverter.ToUInt16(new byte[2] { (byte)Data[14], (byte)Data[13] }, 0);
                else
                    actualPort = BitConverter.ToUInt16(new byte[2] { (byte)Data[13], (byte)Data[14] }, 0);


                try
                {
					/*
					 * We get the Handshake state here.
					 * This way we know what we need to handle.
					 * If the handshake state is 1 then we know we have to handle a status request.
					 * If the handshake state is 2 then we know we have to handle a login request.
					 */ 
                    int HandShakeState = Globals.v2Int32(Data, 15)[0];
					
                    if (HandShakeState == 1)
                    {
                        ConsoleFunctions.WriteDebugLine("Handling Status Request!");
                        StatusResponse(tcpClient, Data);
                    }
                    else if (HandShakeState == 2)
                    {
                        ConsoleFunctions.WriteDebugLine("Handling Login Request!");
                        LoginRequest(tcpClient, Data);
                    }
                    else
                    {
                        ConsoleFunctions.WriteDebugLine("We received an unknown Handshake state! WTF \nStopping");
                        return;
                    }
                }
                catch (Exception ex)
                {
					/*
					 * Seems we have an error here. 
					 * That's sad... We log it for debugging purposes.
					 */
                    ConsoleFunctions.WriteDebugLine(ex.ToString());
                }
        }

        private void LoginRequest(ClientWrapper tcpClient, byte[] Data)
        {   
			/*
			 * We need to get the username another way than Hardcode it in...
			 * I don't see where to get it from at the moment tho :S
			 */

            /*
                Oops this code below was published accidentaly!
                I was checking for a way i can retrieve the username. Still seems bugged tho, really strange.
            */
            int[] UsernameStuff = Globals.v2Int32(Data, 3);
            int _UsernameLength = UsernameStuff[0];
            int NextIndex = UsernameStuff[1];
            ConsoleFunctions.WriteDebugLine("Username length: " + _UsernameLength + " | Next index: " + NextIndex);
            string uName = Encoding.UTF8.GetString(Data, NextIndex, _UsernameLength);
            ConsoleFunctions.WriteDebugLine("Username: " + uName);


            string Username = "kennyvv";
            byte[] _Username = Encoding.UTF8.GetBytes(Username);

			//We grab the UUID for the Username.
            string UUID = Globals.getUUID(Username);
            Guid g = new Guid(UUID);
            UUID = g.ToString();
			byte[] _UUID = Encoding.UTF8.GetBytes(UUID);
			byte[] PacketID = Globals.getVarInt(0x02);
			byte[] UUIDLength = Globals.getVarInt(_UUID.Length);

			//We create new player data.
			//This is the data we can retrieve later on using the PlayerHelper class.
			SharpMC.Utils.PlayerHelper.addPlayer(new Player() { Username = Username, UUID = UUID, Gamemode = new Gamemode() { _Gamemode = 1 }, Position = new Position() { X = 0, Y = 0, Z = 50}, Client = tcpClient });
		
            byte[] UsernameLength = Globals.getVarInt(_Username.Length);
            byte[] TotalLength = Globals.getVarInt(PacketID.Length + _UUID.Length + _Username.Length + UUIDLength.Length + UsernameLength.Length);

            byte[] Response = Globals.concatBytes(TotalLength, PacketID, UUIDLength, _UUID, UsernameLength, _Username);
            Network.SendResponse(tcpClient, Response);
            
			//We send all packets needed for the player to spawn in the game.
			//We still need to send the chunks here.
			//However our world generation class is currently not done.
            CompressionLevel(tcpClient, Data);
            PlayResponse(tcpClient, Data);
            SpawnPositionResponse(tcpClient, Data);
            new Outgoing.PlayerAbilities().Handle(tcpClient, Data);
            new Outgoing.PlayerPositionAndLook().Handle(tcpClient, Data);

        }

		/*
		 * This functions sends a packet to turn off compression.
		 */
        private void CompressionLevel(ClientWrapper tcpClient, byte[] Data)
        {
            byte[] PacketID = Globals.getVarInt(0x46);
            byte[] compressionLVL = Globals.getVarInt(-1);
            byte[] PacketSize = Globals.getVarInt(PacketID.Length + compressionLVL.Length);
            byte[] Response = Globals.concatBytes(PacketSize, PacketID, compressionLVL);

            Network.SendResponse(tcpClient, Response);
        }

		/*
		 * The play response is currently not handled very wel.
		 * But for now i guess this is the best way for me to do it.
		 * Let's hope it works :P
		 */
        private void PlayResponse(ClientWrapper tcpClient, byte[] Data)
        {
			//Get the packet ID
            byte[] PacketID = Globals.getVarInt(0x01);
			//The entity id for the player (Also not the correct way of doing it, but our temporaily solution.
			//We will load it from our Player we made on Handshake later on.
            byte[] EntityID = BitConverter.GetBytes(1);
			byte Gamemode = (byte)1; //We set the gamemode to 1 (Creative)
			byte Dimension = (byte)0; //Dimension 0 (Overworld)
			byte Difficulty = (byte)0; //Difficulty 0 (Peacefull)
            byte MaxPlayers = (byte)Globals.PlayersMax; //This is definetely not the corrrect way, good enough for now tho.

			//We convert the above data into an array so we can use it in our Globals.concatbytes method.
            byte[] AsArray = new byte[4];
            AsArray[0] = Gamemode;
            AsArray[1] = Dimension;
            AsArray[2] = Difficulty;
            AsArray[3] = MaxPlayers;

			//We convert the string into a UTF8 byte array.
            string Lvltype = "default";
            byte[] _LVLType = Encoding.UTF8.GetBytes(Lvltype);
            byte[] LVLTypeLength = Globals.getVarInt(_LVLType.Length);

			//We set the debug info to false.
            byte[] debugInfo = new byte[1];
            debugInfo[0] = 0x00;

            byte[] PacketSize = Globals.getVarInt(EntityID.Length + AsArray.Length + _LVLType.Length + LVLTypeLength.Length + debugInfo.Length + 1);
            
            byte[] Send = Globals.concatBytes(PacketSize, PacketID, EntityID, AsArray, LVLTypeLength, _LVLType, debugInfo);
            Network.SendResponse(tcpClient, Send);
        }

        private void StatusResponse(ClientWrapper tcpClient, byte[] message)
        {
            //HWID : d38d0d94-032a-40e3-b6bf-fa3edd570d5c
            string json = "{\"version\": {\"name\": \"" + Globals.ProtocolName + "\",\"protocol\": " + Globals.ProtocolVersion.ToString() + "},\"players\": {\"max\": " + Globals.PlayersMax + ",\"online\": " + Globals.PlayerOnline + "},\"description\": {\"text\":\"" + Globals.ServerMOTD + "\"}, \"favicon\": \"data:image/png;base64,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\"}";
            byte[] jason = Encoding.UTF8.GetBytes(json);

            byte[] PacketID = Globals.getVarInt(0x00);
            byte[] jsonSize = Globals.getVarInt(jason.Length);
            byte[] PacketSize = Globals.getVarInt(PacketID.Length + jsonSize.Length + jason.Length);

            byte[] SendData = Globals.concatBytes(PacketSize, PacketID, jsonSize, jason);

            Network.SendResponse(tcpClient, SendData);

        }

        private void SpawnPositionResponse(ClientWrapper tcpClient, byte[] Data)
        {
            byte[] PacketID = Globals.getVarInt(0x05);
            long XYZ = Positions.getPositionStructure(0, 0, 50);
            byte[] _XYZ = BitConverter.GetBytes(XYZ);

            byte[] PacketSize = Globals.getVarInt(PacketID.Length + _XYZ.Length);
            byte[] SendData = Globals.concatBytes(PacketSize, PacketID, _XYZ);

            Network.SendResponse(tcpClient, SendData);
        }
    }
}
