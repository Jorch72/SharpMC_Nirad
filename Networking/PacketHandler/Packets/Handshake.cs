using SharpMC.Data;
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
        public override void Handle(object Client, byte[] Data)
        {
              TcpClient tcpClient = (TcpClient)Client;

                string Host = Encoding.UTF8.GetString(Data, 4, Data[3]);
                ushort actualPort;
                if (BitConverter.IsLittleEndian)
                    actualPort = BitConverter.ToUInt16(new byte[2] { (byte)Data[14], (byte)Data[13] }, 0);
                else
                    actualPort = BitConverter.ToUInt16(new byte[2] { (byte)Data[13], (byte)Data[14] }, 0);
                try
                {
                    int HandShakeState = Globals.v2Int32(Data, 15);

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
        }

        private void LoginRequest(TcpClient tcpClient, byte[] Data)
        {   
            string Username = "kennyvv";
            byte[] _Username = Encoding.UTF8.GetBytes(Username);

            string UUID = Globals.getUUID(Username);
            Guid g = Guid.Parse(UUID);
            UUID = g.ToString();

            SharpMC.Utils.PlayerHelper.addPlayer(new Player() { Username = Username, UUID = UUID, Gamemode = new Gamemode() { _Gamemode = 1 }, Position = new Position() { X = 0, Y = 0, Z = 50} });

            byte[] _UUID = Encoding.UTF8.GetBytes(UUID);
            
            byte[] PacketID = Globals.getVarInt(0x02);
            byte[] UUIDLength = Globals.getVarInt(_UUID.Length);
            byte[] UsernameLength = Globals.getVarInt(_Username.Length);
            byte[] TotalLength = Globals.getVarInt(PacketID.Length + _UUID.Length + _Username.Length + UUIDLength.Length + UsernameLength.Length);

            byte[] Response = Globals.concatBytes(TotalLength, PacketID, UUIDLength, _UUID, UsernameLength, _Username);
            Network.SendResponse(tcpClient, Response);
            
            CompressionLevel(tcpClient, Data);
            PlayResponse(tcpClient, Data);
            SpawnPositionResponse(tcpClient, Data);
        }

        private void CompressionLevel(TcpClient tcpClient, byte[] Data)
        {
            byte[] PacketID = Globals.getVarInt(0x46);
            byte[] compressionLVL = Globals.getVarInt(-1);
            byte[] PacketSize = Globals.getVarInt(PacketID.Length + compressionLVL.Length);
            byte[] Response = Globals.concatBytes(PacketSize, PacketID, compressionLVL);

            Network.SendResponse(tcpClient, Response);
        }

        private void PlayResponse(TcpClient tcpClient, byte[] Data)
        {
            byte[] PacketID = Globals.getVarInt(0x01);
            byte[] EntityID = BitConverter.GetBytes(1);
            byte Gamemode = (byte)1;
            byte Dimension = (byte)0;
            byte Difficulty = (byte)0;
            byte MaxPlayers = (byte)Globals.PlayersMax;

            byte[] AsArray = new byte[4];
            AsArray[0] = Gamemode;
            AsArray[1] = Dimension;
            AsArray[2] = Difficulty;
            AsArray[3] = MaxPlayers;

            string Lvltype = "default";
            byte[] _LVLType = Encoding.UTF8.GetBytes(Lvltype);
            byte[] LVLTypeLength = Globals.getVarInt(_LVLType.Length);

            byte[] debugInfo = new byte[1];
            debugInfo[0] = 0x00;

            byte[] PacketSize = Globals.getVarInt(EntityID.Length + AsArray.Length + _LVLType.Length + LVLTypeLength.Length + debugInfo.Length + 1);
            
            byte[] Send = Globals.concatBytes(PacketSize, PacketID, EntityID, AsArray, LVLTypeLength, _LVLType, debugInfo);
            Network.SendResponse(tcpClient, Send);
        }

        private void StatusResponse(TcpClient tcpClient, byte[] message)
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

        private void SpawnPositionResponse(TcpClient tcpClient, byte[] Data)
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
