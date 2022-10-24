using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace Server
{
    class Program
    {
        static void Main(string[] args) 
        {
            TcpListenerServer.Instance.GetTcpListener().Start();
            Console.WriteLine("Server started");

            new Server().Work();
        }
    }
}
