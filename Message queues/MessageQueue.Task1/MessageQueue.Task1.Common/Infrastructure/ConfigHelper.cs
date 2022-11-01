using Microsoft.Extensions.Configuration;

namespace MessageQueue.Task1.Common.Infrastructure
{
    public static class ConfigHelper
    {
        private static readonly IConfigurationRoot Configuration =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public static string GetMessageQueueEndpoint()
        {
            return Configuration["MessageQueueEndpoint"];
        }

        public static string GetQueueName()
        {
            return Configuration["QueueName"];
        }

        public static string GetFileFolder()
        {
            return Configuration["FileFolder"];
        }

        public static int GetChunkSize()
        {
            return int.Parse(Configuration["ChunkSize"]);
        }
    }
}
