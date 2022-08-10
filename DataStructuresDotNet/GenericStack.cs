using System;
using System.Collections.Generic;

namespace DataStructuresDotNet
{
    public class GenericStack
    {
        public static void Print()
        {
            Console.WriteLine("Generic Stack example!");

            Stack<string> numbers = new Stack<string>();
            numbers.Push("One");
            numbers.Push("Two");
            numbers.Push("Three");
            numbers.Push("Four");
            numbers.Push("Five");

            foreach (string number in numbers)
            {
                Console.WriteLine(number);
            }

            Console.WriteLine("numbers.Pop(): {0}", numbers.Pop());
            Console.WriteLine("Peek at the next item to destack: {0}", numbers.Peek());
            Console.WriteLine("numbers.Pop(): {0}", numbers.Pop());
            
            Console.WriteLine("\n");

            Stack<string> stack2 = new Stack<string>(numbers.ToArray());

            Console.WriteLine("Contents of the first copy: ");
            
            foreach(string number in stack2)
            {
                Console.WriteLine(number);
            }

            string[] array2 = new string[numbers.Count * 2];
            numbers.CopyTo(array2, numbers.Count);

            Stack<string> stack3 = new Stack<string>(array2);

            Console.WriteLine("\n");
            Console.WriteLine("Content of the second copy, with duplicates: ");
            foreach(string number in stack3)
            {
                Console.WriteLine(number);
            }

            Console.WriteLine("stack2 contains 'four'? Answer: {0}", stack2.Contains("four") ? "Yes" : "No");

            Console.WriteLine("Clear stack stack2.Clear()");
            stack2.Clear();
            Console.WriteLine("Count of the stack2: {0}", stack2.Count);

        }
    }
}
