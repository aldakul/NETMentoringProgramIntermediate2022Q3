using System;

namespace MultiThreading.OptionalTask.Task2.Server
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