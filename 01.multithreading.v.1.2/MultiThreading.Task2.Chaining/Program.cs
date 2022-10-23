/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            ExecuteTask();

            Console.ReadLine();
        }
        static void ExecuteTask()
        {
            var generateArray = Task.Run(() =>TenRandomInt());
            var chainOfTasks = generateArray.ContinueWith(x => MultiplyWithRandomInt(x.Result))
                .ContinueWith(x => SortArrayByAsc(x.Result))
                .ContinueWith(x => CalculateAvgValue(x.Result));

        }
        static int[] TenRandomInt()
        {
            //vars
            int[] array = new int[10];
            Random random = new Random();

            //create an array of 10 random integer 
            array = array.Select(x => x = random.Next(1, 10000)).ToArray();

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().DeclaringType}");
            Console.WriteLine($"create an array of 10 random integer:");
            array.Select(x => { Console.WriteLine(x); return 1; }).ToList();

            return array;
        }
        static int[] MultiplyWithRandomInt(int[] array)
        {
            //vars
            Random random = new Random();
            int newRandomNumber = random.Next(1, 10000);

            //multiplie array with another random integer
            array = array.Select(x => x*newRandomNumber).ToArray();

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().DeclaringType}");
            Console.WriteLine($"new random integer:{newRandomNumber}");
            Console.WriteLine($"multiplie array with another random integer:");
            array.Select(x => { Console.WriteLine(x); return 1; }).ToList();

            return array;
        }
        static int[] SortArrayByAsc(int[] array)
        {
            //sort array by ascending
            array = array.OrderBy(x => x).ToArray();

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().DeclaringType}");
            Console.WriteLine($"sort array by ascending:");
            array.Select(x => { Console.WriteLine(x); return 1; }).ToList();

            return array;
        }
        static double CalculateAvgValue(int[] array)
        {
            //calculate the average value
            var arrayAvg = array.Average(x => x);

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().DeclaringType}");
            Console.WriteLine($"The average value: {arrayAvg}");

            return arrayAvg;
        }
    }
}
