using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet
{
    public class SimpleLinkedList
    {
        public static void Print()
        {
            string[] words =
            {
                "the",
                "fox",
                "jumps",
                "over",
                "the",
                "dog"
            };

            LinkedList<string> sentence = new(words);
            LoopValues(sentence, "The linked list values: ");

            sentence.AddFirst("today");
            LoopValues(sentence, "Test 1: Add 'today' to the beginning of the list: ");

            // move the first node to be the last node
            LinkedListNode<string> mark1 = sentence.First;
            sentence.RemoveFirst();
            sentence.AddLast(mark1);
            LoopValues(sentence, "Test 2: Move the first node to be the last:");

            // change last to 'yesterday'
            sentence.RemoveLast();
            sentence.AddLast("yesterday");
            LoopValues(sentence, "Test 3: Change last node to 'yesterday'");

            // move last node to the first
            mark1 = sentence.Last;
            sentence.RemoveLast();
            sentence.AddFirst(mark1);
            LoopValues(sentence, "Test 4: Move last node to be the first: ");

            // indicate the last occurence of 'the':
            sentence.RemoveFirst();
            LinkedListNode<string> current = sentence.FindLast("the");
            IndicateNode(current, "Test 5: Indicate last occurance of 'the'");

            sentence.AddAfter(current, "old");
            sentence.AddAfter(current, "lazy");
            IndicateNode(current, "Test 6: Add 'lazy' and 'old' after 'the'");

            // indicate 'fox' node
            current = sentence.Find("fox");
            IndicateNode(current, "Test 7: Indicate node 'fox'");


            sentence.AddBefore(current, "quick");
            sentence.AddBefore(current, "brown");
            IndicateNode(current, "Test 8: Add 'quick' and 'brown' before 'fox' node");

            mark1 = current;
            LinkedListNode<string> mark2 = current.Previous;
            current = sentence.Find("dog");
            IndicateNode(current, "Test 9: Indicate the 'dog' node");

            Console.WriteLine("Test 10: Throw Exception by adding node (fox)");
            try
            {
                sentence.AddBefore(current, mark1);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Exception Message: {0}", ex.Message);

            }
            Console.WriteLine();


            sentence.Remove(mark1);
            sentence.AddBefore(current, mark1);
            IndicateNode(current, "Test 11: Move a referenced node (fox) before the current node (dog)");

            sentence.Remove(current);
            IndicateNode(current, "Test 12: Remove current node (dog) and attempt to indicate it:");


            sentence.AddAfter(mark2, current);
            IndicateNode(current, "Test 13: Add node removed in test 11 after a referenced node (brown): ");

            sentence.Remove("old");
            LoopValues(sentence, "Test 14: Remove node that has the value 'old':");

            sentence.RemoveLast();
            ICollection<string> icoll = sentence;
            icoll.Add("rhinoceros");
            LoopValues(sentence, "Test 15: Remove last node, cast to ICollection and add 'rhinoceros':");

            Console.WriteLine("Test 16: Copy the list to an array");

            string[] sArray = new string[sentence.Count];
            sentence.CopyTo(sArray, 0); // should be here 0?

            foreach (string s in sArray)
            {
                Console.WriteLine(s);

            }
            sentence.Clear();

            Console.WriteLine();

            Console.WriteLine("Test 17: Clear all linked list and check sentence contains 'jumps' = {0}", sentence.Contains("jumps"));

            Console.ReadLine();





        }

        static void LoopValues(IEnumerable loop, string label)
        {
            Console.WriteLine(label);
            foreach (object item in loop)
            {
                Console.Write("    {0}", item);
            }
            Console.WriteLine("\n");
        }

        static void IndicateNode(LinkedListNode<string> node, string label)
        {
            Console.WriteLine(label);

            if (node.List == null)
            {
                Console.WriteLine("Node '{0}' is not in the list. \n");
                return;
            }

            StringBuilder result = new("(" + node.Value + ")");
            LinkedListNode<string> pNode = node.Previous;

            while (pNode != null)
            {
                result.Insert(0, pNode.Value + " ");
                pNode = pNode.Previous;
            }

            node = node.Next;
            while (node != null)
            {
                result.Append(" " + node.Value);
                node = node.Next;
            }

            Console.WriteLine(result);
            Console.WriteLine();
        }
    }
}
