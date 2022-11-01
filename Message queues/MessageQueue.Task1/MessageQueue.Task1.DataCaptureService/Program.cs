using MessageQueue.Task1.Common.Infrastructure;
using System.IO;

namespace MessageQueue.Task1.DataCaptureService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RabbitMQClientSender.Instance.QueueDeclare();
            using FolderWatcher watcher = new();
            watcher.StartWatching();

            Console.WriteLine($"The program ready to send Message \nPlease open:\n{Directory.GetCurrentDirectory()}\\{ConfigHelper.GetFileFolder()} \nDirectory and paste any file");
            

            Console.ReadLine();
        }
    }
}
