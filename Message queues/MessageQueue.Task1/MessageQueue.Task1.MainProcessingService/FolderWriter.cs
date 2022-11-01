using MessageQueue.Task1.Common.Infrastructure;

namespace MessageQueue.Task1.MainProcessingService
{
    public class FolderWriter
    {
        private readonly string _directory;
        public FolderWriter()
        {
            _directory = ConfigHelper.GetFileFolder();
            Directory.CreateDirectory(_directory);
        }

        public async Task CreateFileAsync(string fileName, byte[] fileBytes)
        {
            await using FileStream writer = File.Create($"{_directory}/{fileName}");
            await writer.WriteAsync(fileBytes);
        }
    }
}
