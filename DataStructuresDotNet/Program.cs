using System;
using DataStructuresDotNet.Linear;
using DataStructuresDotNet.Trees;



namespace DataStructuresDotNet
{
    class Program
    {

        static void Main(string[] args)
        {
            // simple stack
            // StackExample.Print();

            // generic stack
            // GenericStack.Print();

            // simple queue
            // SimpleQueue.Print();

            // geneeric queue
            // GenericQueue.Print();

            // linkded list
            // SimpleLinkedList.Print();

            // hashtable implemented in built in Dictionary type
            // Hashtable.Print();

            // binary search implemented with the Array.BinarySearch
            // BinarySearch.Print();

            // SkipList test

            /*var skipList = new SkipList<int>();

            for (int i = 100; i >= 50; --i)
                skipList.Add(i);

            for (int i = 0; i <= 35; ++i)
                skipList.Add(i);

            for (int i = -15; i <= 0; ++i)
                skipList.Add(i);

            for (int i = -15; i >= -35; --i)
                skipList.Add(i);

            Console.WriteLine("[TEST]Count Should be 124");
            Console.WriteLine("Result: {0}", skipList.Count);

            skipList.Clear();

            for (int i = 100; i >= 0; --i)
                skipList.Add(i);

            Console.WriteLine("[TEST]Count Should be: 101");
            Console.WriteLine("Count: {0}", skipList.Count);

            int deletedValue;

            skipList.Remove(100, out deletedValue);

            Console.WriteLine("[TEST]Deleted 100 value! Result should be 100 and Count 100");
            Console.WriteLine("Remove result: {0} \nCount: {1}", deletedValue, skipList.Count);

            Console.WriteLine("Walk for values:");
            foreach (int number in skipList)
            {
                Console.WriteLine("\t" + number);
            }

            */

            // BTree Test

            BTree<int> bTree = new (4);
            Console.WriteLine("INSERT CASES! \n");
            Console.WriteLine(@"
            // CASE #1
            // Insert: 10, 30, 20, 50, 40, 60, 70
            // ROOT contains all values; no split.
            //
            /***************************************
             **
             **    [10 , 20 , 30 , 40, 50, 60, 70]
             **
             ***************************************
             */
            ");
            bTree.Insert(10);
            bTree.Insert(30);
            bTree.Insert(20);
            bTree.Insert(50);
            bTree.Insert(40);
            bTree.Insert(60);
            bTree.Insert(70);

            Console.WriteLine("Result should be 7! Count: {0}", bTree.Root.Keys.Count);

            Console.WriteLine(@"
            //
            // CASE #2
            // Insert to the previous tree: 35.
            // Split into multiple.
            //
            /***************************************
             **
             **                      [40]
             **                     /    \
             **                    /      \
             ** [10 , 20 , 30 , 35]        [50 , 60 , 70]
             **
             ***************************************
             */");

            bTree.Insert(35);
            Console.WriteLine("Root Keys First Element = 40, Result = {0}", bTree.Root.Keys[0]);
            Console.WriteLine("First child keys count = 4, Result = {0}", bTree.Root.Children[0].Keys.Count);
            Console.WriteLine("Second child keys count = 3, Result = {0}", bTree.Root.Children[1].Keys.Count);

            Console.WriteLine("\n");
            Console.WriteLine(@"
            // CASE #3
            // Insert to the previous tree: 5, 15, 25, 39.
            // Split leftmost child.
            //
            /***************************************
             **
             **                      [20 , 40]
             **                     /    |    \
             **                    /     |     \
             **         [5, 10, 15]      |      [50 , 60 , 70]
             **                          |
             **                   [25, 30, 35, 39]
             **
             ***************************************
             */");

            bTree.Insert(5);
            bTree.Insert(15);
            bTree.Insert(25);
            bTree.Insert(39);
            Console.WriteLine("Root First key value = 20, Result = {0}", bTree.Root.Keys[0]);
            Console.WriteLine("Root Second key value = 40, Result = {0}", bTree.Root.Keys[1]);
            Console.WriteLine("Count of the first child keys = 3, Result = {0}", bTree.Root.Children[0].Keys.Count);
            Console.WriteLine("Count of the second child keys = 4, Result = {0}", bTree.Root.Children[1].Keys.Count);
            Console.WriteLine("Count of th third child keys = 3, Result = {0}", bTree.Root.Children[2].Keys.Count);

            Console.WriteLine("SEARCH CASES!");

            bTree = new BTree<int>(4);
            bTree.Insert(10);
            bTree.Insert(30);
            bTree.Insert(20);
            bTree.Insert(50);
            bTree.Insert(40);
            bTree.Insert(60);
            bTree.Insert(70);
            bTree.Insert(35);
            bTree.Insert(5);
            bTree.Insert(15);
            bTree.Insert(25);
            bTree.Insert(39);

            Console.WriteLine(@"
            // The tree now looks like this:
            /***************************************
             **
             **                      [20 , 40]
             **                     /    |    \
             **                    /     |     \
             **         [5, 10, 15]      |      [50 , 60 , 70]
             **                          |
             **                   [25, 30, 35, 39]
             **
             ***************************************
             */");

            Console.WriteLine("Search for value 20 keys count equal = 2, Result = {0}", bTree.Search(20).Keys.Count);
            Console.WriteLine("Search for value 40 keys count equal = 2, Result = {0}", bTree.Search(20).Keys.Count);
            var found = bTree.Search(41);
            Console.WriteLine("Search for not existing value 41, Result = {0}", found != null ? "Not Empty" : "Empty");
            Console.WriteLine("Search for value 5 keys count equal = 3, Result = {0}", bTree.Search(5).Keys.Count);
            Console.WriteLine("Search for value 25 keys count equal = 4, Result = {0}", bTree.Search(25).Keys.Count);

            Console.WriteLine("DELETE CASE!");
            // Build a base tree
            bTree = new BTree<int>(4);
            bTree.Insert(10);
            bTree.Insert(30);
            bTree.Insert(20);
            bTree.Insert(50);
            bTree.Insert(40);
            bTree.Insert(60);
            bTree.Insert(70);
            bTree.Insert(35);
            bTree.Insert(5);
            bTree.Insert(15);
            bTree.Insert(25);
            bTree.Insert(39);

            Console.WriteLine(@"
            // The tree now looks like this:
            /***************************************
             **
             **                      [20 , 40]
             **                     /    |    \
             **                    /     |     \
             **         [5, 10, 15]      |      [50 , 60 , 70]
             **                          |
             **                   [25, 30, 35, 39]
             **
             ***************************************
             */");
            Console.WriteLine("Search for value 20 keys count equal = 2, Result = {0}", bTree.Search(20).Keys.Count);
            Console.WriteLine("Search for value 40 keys count equal = 2, Result = {0}", bTree.Search(20).Keys.Count);
            found = bTree.Search(41);
            Console.WriteLine("Search for not existing value 41, Result = {0}", found != null ? "Not Empty" : "Empty");
            Console.WriteLine("Search for value 5 keys count equal = 3, Result = {0}", bTree.Search(5).Keys.Count);
            Console.WriteLine("Search for value 25 keys count equal = 4, Result = {0}", bTree.Search(25).Keys.Count);

            Console.WriteLine("Removed value 5!");
            bTree.Remove(5);

            Console.WriteLine("Search for value 5, Result = {0}", bTree.Search(5) == null ? "Empty" : "Not Empty");
            Console.WriteLine("Count of the children, Result = {0}", bTree.Root.Children.Count);
            Console.WriteLine("Left-most children count = 7, Result = {0}", bTree.Root.Children[0].Keys.Count);
            Console.WriteLine("Right-most children count = 3, Result = {0}", bTree.Root.Children[1].Keys.Count);

            Console.WriteLine("Removed value 50!");
            bTree.Remove(50);
            Console.WriteLine(@"
            // The tree now looks like this:
            /***************************************
             **
             **                           [39]
             **                          /    \
             **                         /      \
             ** [10, 15, 20, 25, 30, 35]       [40, 60, 70]
             ** 
             ***************************************
             */");

            Console.WriteLine("Root First Key value = 39, Result = {0}", bTree.Root.Keys[0]);
            Console.WriteLine("Root First Children Keys count = 6, Result = {0}", bTree.Root.Children[0].Keys.Count);
            Console.WriteLine("Root Second Children Keys count = 3, Result = {0}", bTree.Root.Children[1].Keys.Count);

            Console.WriteLine("Remove Everything!");
            bTree.Remove(10);
            bTree.Remove(15);
            bTree.Remove(20);
            bTree.Remove(25);
            bTree.Remove(30);
            bTree.Remove(35);
            bTree.Remove(39);
            bTree.Remove(40);
            bTree.Remove(60);
            bTree.Remove(70);

            Console.WriteLine("Root is Empty, Result = {0}", bTree.Root == null ? "Empty" : "NotEmpty");

        }
    }
}
