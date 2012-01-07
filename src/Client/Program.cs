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


         Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         IPAddress myAddress = IPAddress.Parse("127.0.0.1");
         IPEndPoint endPoint = new IPEndPoint(myAddress, 9567);

         mySocket.Connect(endPoint);

         SendData(mySocket);
         mySocket.Disconnect(false);
         mySocket.Close();
      }
      
      public static void SendData(Socket myScoket) {
         myScoket.Send(Encoding.UTF8.GetBytes("Hello, World!"));
      }

   }
}
