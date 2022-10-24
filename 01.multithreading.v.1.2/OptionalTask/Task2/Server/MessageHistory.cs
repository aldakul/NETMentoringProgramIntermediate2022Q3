using System.Collections.Generic;
using System.Linq;

namespace MultiThreading.OptionalTask.Task2.Server
{

    public class MessageHistory
    {
        private static readonly LinkedList<string> Messages = new LinkedList<string>();
        private static readonly object LockObject = new object();
        private const int MAX_MESSAGE_COUNT = 10;

        public void AddMessageToHistory(string message)
        {
            lock (LockObject)
            {
                Messages.AddLast(message);
                if (Messages.Count > MAX_MESSAGE_COUNT)
                    Messages.RemoveFirst();
            }
        }

        public IEnumerable<string> GetMessageHistory()
        {
            lock (LockObject)
            {
                return Messages.ToArray();
            }
        }
    }
}