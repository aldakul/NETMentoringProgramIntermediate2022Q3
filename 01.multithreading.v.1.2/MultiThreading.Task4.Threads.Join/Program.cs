/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Drawing;
using System.Reflection;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore semaphore = new Semaphore(100, 100);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            var threadCount = 10;
            ThreadClass(threadCount);
            ThreadPoolClass(threadCount);


            Console.ReadLine();
        }

        private static void ThreadClass(object threadCount)
        {
            var count = (int)threadCount;
            if (count < 1)
                return;
            Console.WriteLine($"a) Thread id: {Thread.CurrentThread.ManagedThreadId}. Number: {--count}");
            var thread = new Thread(ThreadClass);
            thread.Start(count);
            thread.Join();
            
        }

        private static void ThreadPoolClass(object threadCount)
        {
            var count = (int)threadCount;
            if (count < 1)
                return;
            semaphore.WaitOne();
            Console.WriteLine($"b) Thread id: {Thread.CurrentThread.ManagedThreadId}. Number: {--count}");
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolClass), count);
            semaphore.Release();
        }
    }
}
