using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public interface IBinarySearchTree<T> where T: IComparable<T>
    {
        // Returns a copy of the tree root;
        BSTNode<T> Root { get; }

        // Returns the numbers of elements in the tree
        int Count { get; }

        // Check if the tree empty
        bool IsEmpty { get; }
        
        // Returns the height of the Tree
        int Height { get; }
        
        // Returns true if tree allow inserting duplicates; otherwise false;
        bool AllowsDuplicates { get; }

        // Inserts elemnt to the tree
        void Insert(T value);

        // Inserts array of the elements
        void Insert(T[] collection);

        // Inserts list of the elements
        void Insert(List<T> collection);

        // Removes min value from tree
        void RemoveMin();


        // Removes max value from tree
        void RemoveMax();

        // Remove an element from tree
        void Remove(T item);

        // Check for the existence of an item
        bool Contains(T item);

        // Finds minimum value
        T FindMin();

        // Finds max value
        T FindMax();

        // Find an element from tree, return null if not find
        T Find(T item);

        // Finds all the elements in the tree that match the predicate
        IEnumerable<T> FindAll(System.Predicate<T> searchPredicate);

        // Returns array of the tree elements
        T[] ToArray();

        // Return list of the tree elements
        List<T> ToList();

        // Returns an enumerator that visits node in the order: parent, left child, right child
        IEnumerator<T> GetPreOrderEnumerator();

        // Returns an enumerator that visits node in the order: left child, parent, right child
        IEnumerator<T> GetInOrderEnumerator();

        // Return an enumerator that visits noe in the order: left child, right child, parent
        IEnumerator<T> GetPostOrderEnumerator();

        // Just clear tree
        void Clear();
    }

    /// <summary>
    ///  The itemed version of the Binary Search tree
    /// </summary>
    /// <typeparam name="TKey">Type of items</typeparam>
    /// <typeparam name="TValue">Type of records per node.</typeparam>
    public interface IBinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        BSTMapNode<TKey, TValue> Root { get; }
        int Count { get; }
        bool IsEmpty { get; }
        int Height { get; }
        bool AllowDuplicates { get; }
        void Insert(TKey key, TValue value);
        void Insert(KeyValuePair<TKey, TValue> keyValue);
        void Insert(KeyValuePair<TKey, TValue>[] collection);
        void Insert(List<KeyValuePair<TKey, TValue>> collection);
        void RemoveMin();
        void RemoveMax();
        void Remove(TKey key);
        bool Contains(TKey key);
        KeyValuePair<TKey, TValue> FindMin();
        KeyValuePair<TKey, TValue> FindMax();
        KeyValuePair<TKey, TValue> Find(TKey key);
        IEnumerable<KeyValuePair<TKey, TValue>> FindAll(System.Predicate<TKey> searchPredicate);
        KeyValuePair<TKey, TValue>[] ToArray();
        List<KeyValuePair<TKey, TValue>> ToList();
        void Clear();
    }
}
