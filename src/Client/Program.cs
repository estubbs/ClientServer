using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Client
{
   class Program
   {
      static void Main(string[] args) {
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

         IPAddress ip = IPAddress.Parse("127.0.0.1");
         IPEndPoint ep = new IPEndPoint(ip, 9657);
         Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         IAsyncResult result = mySocket.BeginConnect(ep, new AsyncCallback(BeginConnectCallback), mySocket);
      }

      private static void BeginConnectCallback(IAsyncResult result) {
         Socket soc = (Socket)result.AsyncState;
         try
         {
            soc.EndConnect(result);
         }
         catch (SocketException ex)
         {
            Console.WriteLine(ex.Message);
         }
         if (soc.Connected)
         {
            soc.Send(Encoding.UTF8.GetBytes("Hello, World!"));
         }
         else
         {
            Console.WriteLine("retry");
            connect();
         }

      }

      private static void disconnect() {
         Console.WriteLine("disconnect");

      }
   }
}
