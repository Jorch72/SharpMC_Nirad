using System;
using System.Net.Sockets;
using SharpMC.Data;

class ClientWrapper
{
   public TcpClient Client { get; set; }
   public object Tag { get; set; }
   public Player _Player { get; set; }

   public ClientWrapper(TcpClient client)
   {
       Client = client;
   }
}