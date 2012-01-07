using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ClientServer
{
   public class Client
   {
      protected Socket _Socket { get; set; }
      protected IPEndPoint _ClientEndPoint { get; set; }

      public bool Shutdown { get; protected set; }
      public bool Connected { get; protected set; }
      public IPAddress IPAddress { get; protected set; }
      public int Port { get; protected set; }

      public Client(IPAddress address, int port) {
         IPAddress = address;
         Port = port;
         _ClientEndPoint = new IPEndPoint(address, port);
         _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         Shutdown = true;
      }

      public Client(string address, int port)
         : this(IPAddress.Parse(address), port) {
      }

      public virtual void Connect() {
         Shutdown = false;
         _StartConnecting(new ConnectionInfo(_Socket, _ClientEndPoint));
      }

      public virtual void SendData() {
         _StartSending(_Socket);
      }

      protected static void _StartConnecting(ConnectionInfo info) {
         Console.WriteLine(string.Format("[CLIENT] - Attempting to connect to {0}", info.RemoteEndPoint));
         info.Socket.BeginConnect(info.RemoteEndPoint, new AsyncCallback(_OnConnectingCallback), info);
      }

      protected static void _OnConnectingCallback(IAsyncResult result) {
         ConnectionInfo info = (ConnectionInfo)result.AsyncState;
         try
         {
            info.Socket.EndConnect(result);
            Console.WriteLine(string.Format("[CLIENT] - Connection established to {0}", info.RemoteEndPoint));
         }
         catch (SocketException ex)
         {
            Console.WriteLine(string.Format("[CLIENT] - {0}", ex.Message));
         }
         if (!info.Socket.Connected)
            _StartConnecting(info);
      }

      protected static void _StartSending(Socket socket) {
         byte[] buffer = Encoding.ASCII.GetBytes("Hello, world!");
         SocketContainer container = new SocketContainer(socket, buffer);
         Console.WriteLine("[CLIENT] - Begin sending data");
         socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(_OnSendingCallback), container);
      }

      protected static void _OnSendingCallback(IAsyncResult result) {
         SocketContainer container = (SocketContainer)result.AsyncState;
         int bytesSent = 0;
         try
         {
            bytesSent = container.ConnectionSocket.EndSend(result);
            Console.WriteLine(string.Format("[CLIENT] - Sent {0} bytes", bytesSent));
         }
         catch (SocketException ex)
         {
            Console.WriteLine(string.Format("[CLIENT] - {0}", ex.Message));
         }
      }

      protected class ConnectionInfo
      {
         public virtual Socket Socket { get; protected set; }
         public virtual IPEndPoint RemoteEndPoint { get; protected set; }

         public ConnectionInfo(Socket socket, IPEndPoint endPoint) {
            Socket = socket;
            RemoteEndPoint = endPoint;
         }
      }
   }
}
