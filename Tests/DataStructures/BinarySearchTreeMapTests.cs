using DataStructuresDotNet.Trees;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.DataStructures
{
    public static class BinarySearchTreeMapTests
    {
        [Fact]
        public static void DoTest()
        {
            var bstMap = new BinarySearchTreeMap<int, string>(allowDuplicates: false);
            // Testing data
            KeyValuePair<int, string>[] values = new KeyValuePair<int, string>[10];

            for (int i = 1; i <= 10; i++)
            {
                var keyValuePair = new KeyValuePair<int, string>(i, String.Format("Integer: {0}", i));
                values[i - 1] = keyValuePair;
            }

            // Test singular insert
            for (int i = 0; i < 10; ++i)
                bstMap.Insert(values[i].Key, values[i].Value);

            /*Assert.True(bstMap.Count == values.Length, "Expected same numbers of items.");*/
            Assert.Equal(bstMap.Count, values.Length);

            bstMap.Clear();

            // Test collection insert
            bstMap.Insert(values);


            bool passed = true;
            // Test enumeration of key-value pairs is still in oreder
            var enumerator = bstMap.GetInOrderEnumerator();
            for (int i = 0; i < 10; ++i)
            {
                if (enumerator.MoveNext())
                {
                    var curr = enumerator.Current;
                    if (curr.Key != values[i].Key || curr.Value != values[i].Value)
                    {
                        passed = false;
                        break;
                    }
                }
            }
            Assert.True(passed);

            // Test against re-shuffled insertions (not like above order)
            bstMap = new BinarySearchTreeMap<int, string>(allowDuplicates: false);

            bstMap.Insert(4, "int4");
            bstMap.Insert(5, "int5");
            bstMap.Insert(7, "int7");
            bstMap.Insert(2, "int2");
            bstMap.Insert(1, "int1");
            bstMap.Insert(3, "int3");
            bstMap.Insert(6, "int6");
            bstMap.Insert(0, "int0");
            bstMap.Insert(8, "int8");
            bstMap.Insert(10, "int10");
            bstMap.Insert(9, "int9");

            Assert.True(bstMap.Count == values.Length + 1, "Expected the same number of items.");

            var insert_duplicate_passed = true;

            try
            {
                bstMap.Insert(2, "Int2");
                insert_duplicate_passed = true;
            }
            catch
            {
                insert_duplicate_passed = false;
            }

            Assert.False(insert_duplicate_passed, "Fail! The tree doesn't allow duplicates!");

            Assert.True(bstMap.Find(2).Key == 2, "Wrong find result!");
            Assert.True(bstMap.FindMin().Key == 0, "Wrong min find!");
            Assert.True(bstMap.FindMax().Key == 10, "Wrong max find!");

            bool keyNotFoundException = false;

            try
            {
                bstMap.Find(99999);
                keyNotFoundException = false;
            }
            catch (KeyNotFoundException)
            {
                keyNotFoundException = true;
            }

            Assert.True(keyNotFoundException, "Key shouldn't exist!");

            // check count
            Assert.Equal(11, bstMap.Count);

            // Assert existence and nonexistence of some items
            Assert.True(bstMap.Contains(1));
            Assert.True(bstMap.Contains(3));
            Assert.False(bstMap.Contains(999));

            // Do some deletions
            bstMap.Remove(7);
            bstMap.Remove(1);
            bstMap.Remove(3);

            // Assert count
            Assert.True(bstMap.Count == 8);

            // Assert nonexistence of previously existing items
            Assert.False(bstMap.Contains(1));
            Assert.False(bstMap.Contains(3));

            // Remove root key
            var oldRootKey = bstMap.Root.Key;
            bstMap.Remove(bstMap.Root.Key);

            // Assert count
            Assert.True(bstMap.Count == 7);

            // Assert nonexistence of old root's key
            Assert.False(bstMap.Contains(oldRootKey));


        }


    }
}
