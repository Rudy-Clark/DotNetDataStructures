using System;
using System.Collections;

namespace DataStructuresDotNet
{
    public class StackExample
    {
        public static void Print()
        {
            Console.WriteLine("Stack Example!");
            Stack myStack = new Stack();

            myStack.Push("Hello");
            myStack.Push(" World");
            myStack.Push("!");

            Console.WriteLine("\tCount: {0}", myStack.Count);
            Console.WriteLine("\tValues: ");

            foreach (string stack in myStack)
            {
                Console.Write("    {0}", stack);
            }
        }
    }
}
