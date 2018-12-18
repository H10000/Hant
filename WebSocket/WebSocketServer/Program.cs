using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocket socket = new WebSocket();
            socket.start(1818);
        }
    }
}