using MessageQueue.Task1.Common;
using MessageQueue.Task1.Common.Infrastructure;
using MessageQueue.Task1.Common.Model;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace MessageQueue.Task1.MainProcessingService
{
    public class RabbitMQServerConsumer : RabbitMQ<RabbitMQServerConsumer>
    {
        private readonly EventingBasicConsumer _consumer;
        private readonly IModel _channel;
        private readonly Dictionary<Guid, List<(ulong, FileMessage)>> _receivedChunks = new();
        private bool _disposed;

        private RabbitMQServerConsumer()
        {
            _channel = Connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ReceivedMessage;
        }

        public void StartReceivingMessages()
        {
            _channel.BasicConsume(QueueName, false, _consumer);
        }

        private async void ReceivedMessage(object sender, BasicDeliverEventArgs eventArgs)
        {
            byte[] body = eventArgs.Body.ToArray();
            FileMessage fileMessage = await BinarySerializer.DeserializeAsync<FileMessage>(body);
            (ulong DeliveryTag, FileMessage fileMessage) receivedMessage = (eventArgs.DeliveryTag, fileMessage);

            TryAddMessageToDictionary(fileMessage, receivedMessage);
            List<(ulong, FileMessage)> currentFileChunks = _receivedChunks[fileMessage.FileId];

            if (ReceivedAllFileChunks(currentFileChunks, fileMessage))
            {
                await CreateNewFile(fileMessage.FileName, currentFileChunks);
                AcknowledgeChunkMessages(currentFileChunks);

                Console.WriteLine($"{fileMessage.FileName} was received.");
                _receivedChunks.Remove(fileMessage.FileId);
            }
        }

        private static bool ReceivedAllFileChunks(List<(ulong, FileMessage)> chunkMessages,
            FileMessage fileMessage)
            => chunkMessages.Count == fileMessage.TotalChunksAmount;


        private void TryAddMessageToDictionary(FileMessage fileMessage, (ulong, FileMessage) receivedMessage)
        {
            if (!_receivedChunks.TryAdd(fileMessage.FileId, new List<(ulong, FileMessage)> { receivedMessage }))
                _receivedChunks[fileMessage.FileId].Add(receivedMessage);
        }

        private void AcknowledgeChunkMessages(List<(ulong DeliveryTag, FileMessage)> chunkMessages)
        {
            foreach ((ulong DeliveryTag, FileMessage) chunk in chunkMessages)
                _channel.BasicAck(chunk.DeliveryTag, false);
        }

        private static async Task CreateNewFile(string fileName, List<(ulong, FileMessage FileMessage)> chunkMessages)
        {
            byte[] fileBytes = chunkMessages.OrderBy(c => c.FileMessage.ChunkNumber)
                .SelectMany(c => c.FileMessage.Content)
                .ToArray();

            FolderWriter directoryWriter = new();
            await directoryWriter.CreateFileAsync(fileName, fileBytes);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _channel.Dispose();

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
