using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet
{
    public class BinarySearch
    {
        public static void Print()
        {
            int[] intArray = new int[10] { 1, 3, 5, 7, 11, 13, 17, 19, 23, 31 };
            int target = 17;
            int pos = Array.BinarySearch(intArray, target);
            if (pos >= 0)
            {
                Console.WriteLine($"ITem {intArray[pos].ToString()} found at position {pos + 1}");

            }
            else
            {
                Console.WriteLine("Item not found");

            }
            Console.ReadKey();

        }
    }
}
