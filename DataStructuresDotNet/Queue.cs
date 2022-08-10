using System;
using System.Collections;


namespace DataStructuresDotNet
{
    public class SimpleQueue
    {
        public static void Print()
        {
            Queue myQueue = new Queue();
            myQueue.Enqueue("Hello");
            myQueue.Enqueue("World");
            myQueue.Enqueue("!");

            Console.WriteLine("Queue");
            Console.WriteLine("\tCount: {0}", myQueue.Count);
            Console.WriteLine("\tValues:");
            foreach(string item in myQueue)
            {
                Console.Write("    {0}", item);
            }


        }
    }
}
