using MessageQueue.Task1.Common;
using MessageQueue.Task1.Common.Infrastructure;
using MessageQueue.Task1.Common.Model;
using RabbitMQ.Client;

namespace MessageQueue.Task1.DataCaptureService
{
    public class RabbitMQClientSender : RabbitMQ<RabbitMQClientSender>
    {
        private bool _disposed;

        public async Task SendFileMessageChunks(string fileName, byte[] fileContent)
        {
            List<byte[]> chunks = fileContent.Chunk(ConfigHelper.GetChunkSize()).ToList();
            Guid fileId = Guid.NewGuid();
            for (int i = 0; i < chunks.Count; i++)
            {
                FileMessage fileMessage = new()
                {
                    FileId = fileId,
                    FileName = fileName,
                    Content = chunks[i],
                    ChunkNumber = i,
                    TotalChunksAmount = chunks.Count
                };
                byte[] body = await BinarySerializer.SerializeAsync(fileMessage);
                using IModel channel = Connection.CreateModel();
                channel.BasicPublish(string.Empty, QueueName, null, body);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
