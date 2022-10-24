using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.OptionalTask.Task2.Server
{

    public class Server
    {
        private readonly MessageHistory _messageHistory;
        private readonly TcpListenerServer _tcpListener;
        private readonly ClientsHandler _clientsHandler;

        public Server()
        {
            _messageHistory = new MessageHistory();
            _tcpListener = TcpListenerServer.Instance;
            _clientsHandler = new ClientsHandler();
        }

        public void Work()
        {
            while (true)
            {
                var clientSocket = _tcpListener.GetTcpListener().AcceptSocket();
                if (clientSocket != null && clientSocket.Connected)
                    Task.Run(() => RunTask(clientSocket));
            }
        }
        private void RunTask(Socket clientSocket)
        {
            using (clientSocket)
            {
                using (var networkStream = new NetworkStream(clientSocket))
                using (var streamReader = new StreamReader(networkStream))
                using (var streamWriter = new StreamWriter(networkStream))
                {

                    _clientsHandler.AddNewClient(streamWriter);

                    var clientName = streamReader.ReadLine();
                    Thread.CurrentThread.Name = clientName;
                    Console.WriteLine($"\t{clientName} connected to server");

                    SendMessageHistoryToClient(clientName, streamWriter);

                    while (true)
                    {
                        try
                        {
                            ReceiveAndProcessMessage(clientName, streamReader);
                        }
                        catch (IOException)
                        {
                            HandleDisconnect(clientName, streamWriter);
                            break;
                        }
                    }
                }
            }
        }

        private void HandleDisconnect(string clientName, StreamWriter streamWriter)
        {
            Console.WriteLine($"{clientName} disconnected");
            _clientsHandler.DeleteClient(streamWriter);
        }

        private void ReceiveAndProcessMessage(string clientName, StreamReader streamReader)
        {
            var messageContent = streamReader.ReadLine();

            if (string.IsNullOrEmpty(messageContent))
                return;

            var message = $"{clientName}: {messageContent}";
            _messageHistory.AddMessageToHistory(message);
            Console.WriteLine(message);

            var errorCount = _clientsHandler.SendMessageToAllClients(message);
            if (errorCount > 0)
                Console.WriteLine($"Error while sending message to {errorCount} clients");
        }

        private void SendMessageHistoryToClient(string clientName, StreamWriter streamWriter)
        {
            try
            {
                foreach (var message in _messageHistory.GetMessageHistory())
                    streamWriter.WriteLine(message);
                streamWriter.Flush();
            }
            catch (IOException)
            {
                HandleDisconnect(clientName, streamWriter);
            }
        }
    }
}