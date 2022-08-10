using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet
{
    public class GenericQueue
    {
        public static void Print()
        {
            Queue<string> numbers = new Queue<string>();
            numbers.Enqueue("One");
            numbers.Enqueue("Two");
            numbers.Enqueue("Three");
            numbers.Enqueue("Four");
            numbers.Enqueue("Five");

            Console.WriteLine("Values: ");
            foreach(string number in numbers)
            {
                Console.WriteLine(number);
            }

            Console.WriteLine("\nDequeue '{0}'", numbers.Dequeue());
            Console.WriteLine("Peek at the next item to dequeue: {0}", numbers.Peek());
            Console.WriteLine("Dequeueing '{0}'", numbers.Dequeue());

            Queue<string> copyNumbers = new Queue<string>(numbers.ToArray());

            Console.WriteLine("\nContents of the first copy");
            foreach (string item in copyNumbers)
            {
                Console.WriteLine(item);

            }

            string[] array2 = new string[numbers.Count * 2];
            numbers.CopyTo(array2, numbers.Count);

            Queue<string> queueCopy2 = new Queue<string>(array2)
            {

            };

            Console.WriteLine("\nContents of the second copy, with duplicates: ");
            foreach (string item in queueCopy2)
            {
                Console.WriteLine(item);

            }
            Console.WriteLine("\nqueueCopy.Contains('four'): {0}", queueCopy2.Contains("Four"));
            Console.WriteLine("queueCopy.Clear()");
            queueCopy2.Clear();
            Console.WriteLine("queueCopy.Count: {0}", queueCopy2.Count);

        }
    }
}
