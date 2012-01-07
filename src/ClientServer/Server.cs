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
      protected bool _Shutdown { get; set; }

      public IPAddress IPAddress { get; protected set; }
      public int Port { get; protected set; }
      public bool Connected { get; protected set; }

      public Server(IPAddress ipAddress, int port) {
         IPAddress = ipAddress;
         Port = port;
         _ServerEndPoint = new IPEndPoint(IPAddress, Port);
         _ConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      }
      public Server(string ipAddress, int port)
         : this(IPAddress.Parse(ipAddress), port) {
      }

      public virtual void Start() {
         Console.WriteLine("Server Starting");
         _Shutdown = false;
         _ConnectionSocket.Bind(_ServerEndPoint);
         _ConnectionSocket.Listen(1);
         _AcceptConnections();
      }

      protected virtual void _AcceptConnections() {
         Console.WriteLine("Listening for connections");
         _ConnectionSocket.BeginAccept(OnAcceptCallback, null);

      }
      protected void OnAcceptCallback(IAsyncResult result) {

         try
         {
            byte[] initialBuffer;
            int bytesTransfered;
            Socket soc = _ConnectionSocket.EndAccept(out initialBuffer, out bytesTransfered, result);
            Console.WriteLine(string.Format("Connection from {0}", soc.RemoteEndPoint));
            Console.WriteLine(string.Format("Initial buffer Size: {0}", initialBuffer.LongLength));
            Console.WriteLine(string.Format("Bytes transfered: {0}", bytesTransfered));
            Console.WriteLine("Buffer Contents:");
            foreach (byte b in initialBuffer)
            {
               Console.Write((int)b);
            }
            Console.WriteLine();
            Console.WriteLine("Buffer Contents Characters");
            foreach (byte b in initialBuffer)
            {
               Console.Write((char)b);
            }
         }
         catch (SocketException ex)
         {
            Console.WriteLine(ex.Message);
         }

      }

   }


}
