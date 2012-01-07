using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ClientServer
{
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
