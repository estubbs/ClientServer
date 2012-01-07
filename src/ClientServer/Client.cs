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
      public bool _Shutdown { get; set; }

      public IPAddress IPAddress { get; protected set; }
      public int Port { get; protected set; }
      public bool Connected { get; protected set; }

      public Client(IPAddress address, int port) {
         IPAddress = address;
         Port = port;
         _ClientEndPoint = new IPEndPoint(address, port);
         _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         _Shutdown = true;
      }
      public Client(string address, int port)
         : this(IPAddress.Parse(address), port) {
      }

      public virtual void Connect() {
         _Shutdown = false;
         _StartConnecting();
      }
      public virtual void SendData() {
         _StartSending(_Socket);

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
            Console.WriteLine(ex.Message);
         }
      }


      protected virtual void _StartConnecting() {
         Console.WriteLine(string.Format("[CLIENT] - Attempting to connect to {0}", _ClientEndPoint));
         _Socket.BeginConnect(_ClientEndPoint, _OnConnectedCallback, null);
      }
      protected virtual void _OnConnectedCallback(IAsyncResult result) {
         try
         {
            _Socket.EndConnect(result);
            Console.WriteLine(string.Format("[CLIENT] - Connection established to {0}", _Socket.RemoteEndPoint));
         }
         catch (SocketException ex)
         {
            Console.WriteLine(ex.Message);
         }
         if (!_Socket.Connected)
            _StartConnecting();
      }
   }
}
