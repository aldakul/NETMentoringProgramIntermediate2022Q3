using System;
using System.Net;
using System.Net.Sockets;

namespace MultiThreading.OptionalTask.Task2.Server
{

    public class TcpListenerServer
    {
        private static readonly TcpListener TcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
        private static readonly Lazy<TcpListenerServer> Lazy = new Lazy<TcpListenerServer>(() => new TcpListenerServer());

        public static TcpListenerServer Instance => Lazy.Value;

        public TcpListener GetTcpListener() => TcpListener;

        private TcpListenerServer() { }
    }
}