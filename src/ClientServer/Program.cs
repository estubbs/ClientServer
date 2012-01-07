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

         char c;
         while (true)
         {
            c = (char)Console.Read();
            switch (c)
            {
               default: continue;
               case 'q': return;
               case 's': startServer(); break;
               case 'c': startClient(); break;
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
