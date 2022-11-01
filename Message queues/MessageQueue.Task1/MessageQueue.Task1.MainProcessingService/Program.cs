namespace MessageQueue.Task1.MainProcessingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RabbitMQServerConsumer receiver = RabbitMQServerConsumer.Instance;
            receiver.QueueDeclare();
            receiver.StartReceivingMessages();
            Console.WriteLine("The program ready to receive files");
            Console.ReadLine();
        }
    }
}
