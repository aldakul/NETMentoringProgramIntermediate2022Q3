/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static readonly int elementsCount = 10;
        private static readonly List<int> sharedList = new List<int>();
        private static readonly Random random = new Random();
        private static readonly EventWaitHandle eventWaitHandle1 = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static readonly EventWaitHandle eventWaitHandle2 = new EventWaitHandle(false, EventResetMode.AutoReset);
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Task.Factory.StartNew(AddElement);
            Task.Factory.StartNew(PrintAllElements);

            Console.ReadLine();
        }
        private static void AddElement()
        {
            for (int i = 0; i < elementsCount; i++)
            {
                var newElement = random.Next();

                sharedList.Add(newElement);

                Console.WriteLine($"{sharedList.Count}.New random element: {newElement}");

                eventWaitHandle1.Set();
                eventWaitHandle2.WaitOne();
            }
        }
        private static void PrintAllElements()
        {
            while (sharedList.Count <= elementsCount)
            {
                eventWaitHandle1.WaitOne();

                sharedList.Select(x => { Console.WriteLine(x); return 1; }).ToList();

                eventWaitHandle2.Set();
            }

        }
    }
}
