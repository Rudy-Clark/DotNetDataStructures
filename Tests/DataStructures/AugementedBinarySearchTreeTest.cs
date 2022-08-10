using System;
using Xunit;

using DataStructuresDotNet.Trees;
using System.Collections.Generic;

namespace Tests.DataStructures
{
    public class AugementedBinarySearchTreeTest
    {
        /// <summary>
        /// FIRST TEST TREE WITH DUPLICATES ELEMENTS
        /// </summary>
        /// <param name="binarySearchTree"></param>
        [Fact]
        public static void AssertTreeWithDuplicateElements()
        {
            // New tree which doesn't allow duplicates
            // AugmentedBinarySearchTree
            var binarySearchTree = new AugementedBinarySearchTree<int>(allowDuplicates: true);

            int[] values = new int[21] { 15, 25, 5, 12, 1, 16, 20, 9, 9, 7, 7, 7, -1, 11, 19, 30, 8, 10, 13, 28, 39 };

            // Insert values with duplicates
            binarySearchTree.Insert(values);

            // ASSERT COUNT = 21 (allows duplicates)
            Assert.Equal(21, binarySearchTree.Count);

            // Test contains/find
            Assert.True(binarySearchTree.Contains(10), "Wrong element.");

            // Test find all
            var enumerableList = binarySearchTree.FindAll(element => element > 15);
            List<int> list = new();
            foreach (int item in enumerableList)
            {
                list.Add(item);
            }

            Assert.True(list.Count == 7, "Wrong FindAll result!");

            // test sort
            List<int> sortedList = binarySearchTree.ToList();
            for (int i = 1; i < sortedList.Count; ++i)
                Assert.True(sortedList[i - 1] <= sortedList[i], "BST sort is wrong!");

            // ASSERT MIN ITEM
            Assert.True(binarySearchTree.FindMin() == -1, "Min is wrong.");

            // ASSERT MAX ITEM
            Assert.True(binarySearchTree.FindMax() == 39, "Max is wrong.");

            // Remove min & max
            binarySearchTree.RemoveMin();
            binarySearchTree.RemoveMax();

            // ASSERT MIN AFTER REMOVE-MIN
            Assert.True(binarySearchTree.FindMin() == 1, "Min is wrong.");

            // ASSERT MAX AFTER REMOVE MAX
            Assert.True(binarySearchTree.FindMax() == 30, "Max is wrong.");

            // Remove min twice
            binarySearchTree.RemoveMin();
            binarySearchTree.RemoveMin();

            // ASSERT MIN
            Assert.True(binarySearchTree.FindMin() == 7, "Min is wrong.");

            // 7 STILL EXISTS BECAUSE IT WAS DUPLICATED
            binarySearchTree.RemoveMin();
            Assert.True(binarySearchTree.FindMin() == 7, "Min is wrong.");

            // Remove max thrice
            binarySearchTree.RemoveMax();
            binarySearchTree.RemoveMax();
            binarySearchTree.RemoveMax();

            // ASSERT MAX AFTER REMOVE-MAX 3 TIMES
            Assert.True(binarySearchTree.FindMax() == 20, "Max is wrong.");

            // Test removing an element with subtrees
            try
            {
                // doesn't exist!
                binarySearchTree.Remove(1000);
            }
            catch
            {
                // does exist!
                binarySearchTree.Remove(16);
            }

            var enumerator = binarySearchTree.GetInOrderEnumerator();
            enumerator.MoveNext();
            Assert.Equal(7, enumerator.Current);

            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.True(enumerator.Current == 8, "Wrong in-order enumeration.");
        }

    }
}
