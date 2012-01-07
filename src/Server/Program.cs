using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
   class Program
   {
      public static void Main(string[] args) {
         Console.WriteLine("SERVER");
         char c = char.MaxValue;
         while (true)
         {
            c = (char)Console.Read();
            switch (c)
            {
               case '1':
                  connect();
                  break;
               case '2':
                  disconnect();
                  return;
               default: continue;
            }
         }

      }
      private static void connect() {

         IPEndPoint ep = new IPEndPoint(IPAddress.Any, 9657);
         Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         mySocket.Bind(ep);
         mySocket.Listen(1);

         Console.WriteLine("Listening for connection");
         IAsyncResult result = mySocket.BeginAccept(new AsyncCallback(AcceptConnection), mySocket);
      }
      public static void AcceptConnection(IAsyncResult result) {
         Socket soc = (Socket)result.AsyncState;
         Console.WriteLine("Connection from");
         SocketContainer container = new SocketContainer { Buffer = new byte[2], Socket = soc.EndAccept(result) };
         startGettingData(container);
      }
      private static void startGettingData(SocketContainer mySocket) {

         IAsyncResult res2 = mySocket.Socket.BeginReceive(mySocket.Buffer, 0, 2, SocketFlags.None, new AsyncCallback(AcceptData), mySocket);
      }
      public static void AcceptData(IAsyncResult result) {
         Console.WriteLine("starting to get data now");
         SocketContainer mySocket = (SocketContainer)result.AsyncState;
         var data = mySocket.Socket.EndReceive(result);
         Console.WriteLine(Encoding.UTF8.GetString(mySocket.Buffer));
         startGettingData(mySocket);
      }
      private static void disconnect() {
         Console.WriteLine("disconnect");
      }
   }
   public class SocketContainer
   {
      public byte[] Buffer { get; set; }
      public Socket Socket { get; set; }
   }
}