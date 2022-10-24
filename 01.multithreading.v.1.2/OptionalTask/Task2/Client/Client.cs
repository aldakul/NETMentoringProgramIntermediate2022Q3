using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MultiThreading.OptionalTask.Task2.Client
{

    public class Client
    {
        private readonly Random _random;

        public Client()
        {
            _random = new Random();
        }

        public void Work()
        {
            try
            {
                while (true)
                {
                    using (var client = new TcpClient("127.0.0.1", 8080))
                    using (var networkStream = client.GetStream())
                    using (var streamWriter = new StreamWriter(networkStream))
                    {
                        var randomNumber = _random.Next(100);
                        var clientNumber = $"Client{randomNumber}";
                        var clientMessage = $"Texted {randomNumber}";

                        //Connect to server
                        SendMessage(streamWriter, clientNumber);
                        Console.WriteLine($"\t{clientNumber} is connected to the server");

                        //Send Message
                        SendMessage(streamWriter, clientMessage);
                        Thread.Sleep(_random.Next(2000));
                        Console.WriteLine("\tMessage is sended");

                        //Get Message
                        ReadMessages(networkStream);
                        Console.WriteLine("\tMessage is received");

                        //Disconnect
                        Console.WriteLine($"\t{clientNumber} is disconnected from the server\n\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void SendMessage(StreamWriter streamWriter, string message)
        {
            streamWriter.WriteLine(message);
            streamWriter.Flush();
        }

        private static void ReadMessages(NetworkStream networkStream)
        {
            using (StreamReader streamReader = new StreamReader(networkStream, Encoding.UTF8, false, 1024, leaveOpen: true))
            {
                while (streamReader.Peek() != -1)
                    Console.WriteLine(streamReader.ReadLine());
            }
        }
    }
}