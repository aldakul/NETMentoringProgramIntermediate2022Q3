using MessageQueue.Task1.Common.Infrastructure;

namespace MessageQueue.Task1.DataCaptureService
{
    public class FolderWatcher : IDisposable
    {
        private readonly FileSystemWatcher _fileSystemWatcher;
        private bool _disposed;

        public FolderWatcher()
        {
            string path = ConfigHelper.GetFileFolder();
            Directory.CreateDirectory(path);

            _fileSystemWatcher = new FileSystemWatcher(path)
            {
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName
            };
        }

        public void StartWatching()
        {
            _fileSystemWatcher.Created += SendFile;
            _fileSystemWatcher.Renamed += SendFile;
            _fileSystemWatcher.Changed += SendFile;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _fileSystemWatcher.Dispose();
            }

            _disposed = true;
        }

        private static void SendFile(object sender, FileSystemEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                string? fileName = eventArgs.Name;
                for (int i = 0; i < 100; i++)
                    try
                    {
                        byte[] fileContent = await File.ReadAllBytesAsync(eventArgs.FullPath);
                        await RabbitMQClientSender.Instance.SendFileMessageChunks(fileName, fileContent);
                        Console.WriteLine($"{fileName} was sent.");
                        return;
                    }
                    catch (IOException)
                    {
                        await Task.Delay(100);
                    }

                Console.WriteLine($"{fileName} - failed to send.");
            });
        }
    }
}
