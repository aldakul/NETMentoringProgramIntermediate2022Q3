using System.Runtime.Serialization;

namespace MessageQueue.Task1.Common.Infrastructure
{
    public static class BinarySerializer
    {
        public static async Task<byte[]> SerializeAsync<T>(T @object)
        {
            await using MemoryStream memoryStream = new();
            DataContractSerializer serializer = new(typeof(T));
            serializer.WriteObject(memoryStream, @object);
            return memoryStream.ToArray();
        }

        public static async Task<T> DeserializeAsync<T>(byte[] bytes)
        {
            await using MemoryStream memoryStream = new(bytes);
            DataContractSerializer serializer = new(typeof(T));
            return (T)serializer.ReadObject(memoryStream);
        }
    }
}
