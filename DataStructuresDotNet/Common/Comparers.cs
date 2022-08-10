using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresDotNet.Trees;

namespace DataStructuresDotNet.Common
{
    public static class Comparers
    {
        /// <summary>
        /// Determines if thisValue is less than the specified otherValue.
        /// </summary>
        /// <returns><c>true</c> if thisValue is less than the specified otherValue; otherwise, <c>false</c>.</returns>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        /// <typeparam name="T">The Type of values.</typeparam>
        public static bool IsLessThan<T>(this T firstValue, T secondValue) where T : IComparable<T>
        {
            return firstValue.CompareTo(secondValue) < 0;
        }

        public static bool IsEqualTo<T>(this T firstValue, T secondValue) where T : IComparable<T>
        {
            return firstValue.Equals(secondValue);
        }

        public static bool IsEqualTo<T>(this BSTNode<T> first, BSTNode<T> second) where T : IComparable<T>
        {
            return (HandleNullCases(first, second) && first.Value.CompareTo(second.Value) == 0);
        }

        public static bool IsGreaterThan<T>(this T firstValue, T secondValue) where T : IComparable<T>
        {
            return firstValue.CompareTo(secondValue) > 0;
        }


        // binary search tree methods
        private static bool HandleNullCases<T>(BSTNode<T> first, BSTNode<T> second) where T : IComparable<T>
        {
            if (first == null || second == null)
                return false;
            return true;
        }
    }
}
