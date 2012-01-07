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

      public virtual void Start() {
         Console.WriteLine("[SERVER] - Server Starting");
         Shutdown = false;
         _ConnectionSocket.Bind(_ServerEndPoint);
         _ConnectionSocket.Listen(1);
         _AcceptConnections();
      }

      protected virtual void _AcceptConnections() {
         Console.WriteLine("[SERVER] - Listening for connections");
         IAsyncResult result = _ConnectionSocket.BeginAccept(new AsyncCallback(OnAcceptCallback), null);
      }
      protected virtual void OnAcceptCallback(IAsyncResult result) {
         Socket soc = null;
         try
         {
            soc = _ConnectionSocket.EndAccept(result);
            Console.WriteLine(string.Format("[SERVER] - Connection from {0}", soc.RemoteEndPoint));
         }
         catch (SocketException ex)
         {
            Console.WriteLine(ex.Message);
         }
         _RecieveData(soc);
         _AcceptConnections();

      }
      protected virtual void _RecieveData(Socket connectedSocket) {
         Console.WriteLine(string.Format("[SERVER] - Ready to recieve on {0}", connectedSocket.LocalEndPoint));
         byte[] buffer = new byte[8192];
         SocketContainer container = new SocketContainer(connectedSocket, buffer);
         IAsyncResult result = connectedSocket.BeginReceive(container.Buffer, 0, container.Buffer.Length, SocketFlags.None, new AsyncCallback(OnRecievDataCallback), container);
      }
      protected virtual void OnRecievDataCallback(IAsyncResult result) {
         SocketContainer container = (SocketContainer)result.AsyncState;
         int bytesRecieved = 0;
         try
         {
            bytesRecieved = container.ConnectionSocket.EndReceive(result);
         }
         catch (SocketException ex)
         {
            Console.WriteLine(ex.Message);
         }

         Console.WriteLine(string.Format("[SERVER] - Received {0} bytes", bytesRecieved));
         Console.WriteLine();

         Console.WriteLine("[SERVER] - Buffer contents:");
         for (int i = 0; i < bytesRecieved; i++)
            Console.Write(container.Buffer[i]);
         Console.WriteLine();

         Console.WriteLine("[SERVER] - Buffer contents:");
         for (int i = 0; i < bytesRecieved; i++)
            Console.Write((char)container.Buffer[i]);
         Console.WriteLine();

         _RecieveData(container.ConnectionSocket);
      }


   }
   internal class SocketContainer
   {
      internal byte[] Buffer { get; private set; }
      internal Socket ConnectionSocket { get; private set; }

      internal SocketContainer(Socket socket, byte[] buffer) {
         Buffer = buffer;
         ConnectionSocket = socket;
      }
   }


}
