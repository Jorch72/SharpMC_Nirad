using System;
using System.Net.Sockets;

public class ClientWrapper
{
   public TcpClient Client { get; set; }
   public object Tag { get; set; }

   public ClientWrapper(TcpClient client)
   {
       Client = client;
   }
}