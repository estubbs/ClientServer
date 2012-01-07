using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ClientServer
{
   class Program
   {
      static Client _Client { get; set; }
      static Server _Server { get; set; }

      static void Main(string[] args) {

         if (args[0] == "c")
            startClient();
         if (args[0] == "s")
            startServer();

         char c;
         while (true)
         {
            c = (char)Console.Read();
            switch (c)
            {
               default: continue;
               case 'q': return;
               case 'd': _Client.SendData(); break;
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

   }
}
