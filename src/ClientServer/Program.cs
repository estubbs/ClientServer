using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ClientServer
{
   class Program
   {
      static Client _Client { get; set; }
      static Server _Server { get; set; }

      static void Main(string[] args) {

         if (args.Length == 1)
         {
            if (args[0] == "c")
               startClient();
            if (args[0] == "s")
               startServer();
         }

         char c;
         while (true)
         {
            c = (char)Console.Read();
            switch (c)
            {
               default: continue;
               case 'q': return;
               case 'd': _Client.SendData(); break;
               case 'c': startClient(); break;
               case 's': startServer(); break;
               case 'r': sendBufferFromServer(); break;
            }
         }
      }

      private static void startServer() {
         _Server = new Server("127.0.0.1", 9657);
         _Server.Start();
      }

      private static void startClient() {
         _Client = new Client("127.0.0.1", 9657);
         _Client.Connect();
      }
      private static void sendBufferFromServer() {
         byte[] buffer = new byte[8192];
         int currentPosition = 0;
         string s;
         while ((s = Console.ReadLine()) != "." && currentPosition < buffer.Length - 1)
         {
            int bytesWriten = 0;
            while (bytesWriten < s.Length)
            {
               bytesWriten = Encoding.UTF8.GetBytes(s, 0, s.Length, buffer, currentPosition);
               currentPosition += bytesWriten;
            }
            _Server.SendData(ref buffer, currentPosition);
            currentPosition = 0;
         }
      }
   }
}
