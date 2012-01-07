using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ClientServer
{
   public class Server
   {

      protected Socket _ConnectionSocket { get; set; }
      protected IPEndPoint _ServerEndPoint { get; set; }

      public IPAddress IPAddress { get; protected set; }
      public int Port { get; protected set; }
      public bool Connected { get; protected set; }
      public bool Shutdown { get; protected set; }

      public Server(IPAddress ipAddress, int port) {
         IPAddress = ipAddress;
         Port = port;
         _ServerEndPoint = new IPEndPoint(IPAddress, Port);
         _ConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         Shutdown = true;
      }
      public Server(string ipAddress, int port)
         : this(IPAddress.Parse(ipAddress), port) {
      }

      public void Start() {
         Console.WriteLine("[SERVER] - Server Starting");
         Shutdown = false;
         _ConnectionSocket.Bind(_ServerEndPoint);
         _ConnectionSocket.Listen(1);
         _AcceptConnections();
      }

      protected void _AcceptConnections() {
         Console.WriteLine("[SERVER] - Listening for connections");
         IAsyncResult result = _ConnectionSocket.BeginAccept(new AsyncCallback(OnAcceptCallback), null);
      }
      protected void OnAcceptCallback(IAsyncResult result) {
         try
         {
            Socket soc = _ConnectionSocket.EndAccept(result);
            Console.WriteLine(string.Format("[SERVER] - Connection from {0}", soc.RemoteEndPoint));
         }
         catch (SocketException ex)
         {
            Console.WriteLine(ex.Message);
         }
         _AcceptConnections();
      }

   }


}
