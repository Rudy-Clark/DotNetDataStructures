using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataStructuresDotNet.Common;

namespace DataStructuresDotNet.Trees
{
    /// <summary>
    /// Implements a generic Binary Search Tree Map data structure.
    /// </summary>
    public class BinarySearchTreeMap<TKey, TValue> : IBinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Specifies the mode of travelling through the tree.
        /// </summary>
        public enum TraversalMode
        {
            InOrder = 0,
            PreOrder = 1,
            PostOrder = 2,
        }

        /// <summary>
        /// TREE INSTANCE VARIABLES
        /// </summary>
        protected int count;
        protected bool allowDuplicates;
        protected virtual BSTMapNode<TKey, TValue> root { get; set; }
        public virtual BSTMapNode<TKey, TValue> Root
        {
            get => root;
            internal set => root = value;
        }


        /// <summary>
        /// CONSTRUCTOR.
        /// Allows duplicates by default.
        /// </summary>
        public BinarySearchTreeMap() : this(true) { }

        /// <summary>
        /// CONSTRUCTOR.
        /// If allowDuplictes is set to false, no duplicate items will be inserted.
        /// </summary>
        public BinarySearchTreeMap(bool allowDuplicates)
        {
            count = 0;
            this.allowDuplicates = allowDuplicates;
            Root = null;
        }

        protected virtual int getTreeHeight(BSTMapNode<TKey, TValue> node)
        {
            if (node == null)
            {
                return 0;
            }

            if (node.IsLeafNode)
            {
                return 1;
            }
            // bot child
            if (node.ChildrenCount == 2)
            {
                return (1 + Math.Max(getTreeHeight(node.LeftChild), getTreeHeight(node.RightChild)));
            }

            // left child
            if (node.HasLeftChild)
            {
                return (1 + (getTreeHeight(node.LeftChild)));
            }
            // right child
            return (1 + getTreeHeight(node.RightChild));
        }

        /// <summary>
        /// Inserts a new node to the tree.
        /// </summary>
        protected virtual bool insertNode(BSTMapNode<TKey, TValue> newNode)
        {
            if (Root == null)
            {
                Root = newNode;
                count++;
                return true;
            }

            if (newNode.Parent == null)
            {
                newNode.Parent = Root;
            }

            if (!allowDuplicates && newNode.Parent.Key.IsEqualTo(newNode.Key))
                return false;

            // got Left
            if (newNode.Parent.Key.IsGreaterThan(newNode.Key)) // newNode < Parent
            {
                if (!newNode.Parent.HasLeftChild)
                {
                    newNode.Parent.LeftChild = newNode;
                    count++;
                    return true;
                }

                newNode.Parent = newNode.Parent.LeftChild;
                return insertNode(newNode);
            }

            // go right
            if (!newNode.Parent.HasRightChild)
            {
                newNode.Parent.RightChild = newNode;
                count++;
                return true;
            }

            newNode.Parent = newNode.Parent.RightChild;
            return insertNode(newNode);
        }

        /// <summary>
        /// Replaces the node's value from it's parent node object with the newValue.
        /// Used in the recusive _remove function.
        /// </summary>
        protected virtual void replaceNodeInParent(BSTMapNode<TKey, TValue> node, BSTMapNode<TKey, TValue> newNode = null)
        {
            if (node.Parent != null)
            {
                if (node.IsLeftChild)
                {
                    node.Parent.LeftChild = newNode;
                }
                else
                {
                    node.Parent.RightChild = newNode;
                }
            }

            if (newNode != null)
            {
                newNode.Parent = node.Parent;
            }
        }

        /// <summary>
        /// Remove the specified node.
        /// </summary>
        protected virtual bool remove(BSTMapNode<TKey, TValue> node)
        {
            if (node == null)
            {
                return false;
            }

            var parent = node.Parent;

            if (node.ChildrenCount == 2)
            {
                var successor = node.RightChild;
                node.Key = successor.Key;
                node.Value = successor.Value;
                return remove(successor);
            }

            if (node.HasLeftChild)
            {
                replaceNodeInParent(node, node.LeftChild);
                count--;
            }
            else if (node.HasRightChild)
            {
                replaceNodeInParent(node, node.RightChild);
                count--;
            }
            else
            {
                replaceNodeInParent(node);
                count--;
            }

            return true;
        }

        /// <summary>
        /// Finds a node inside another node's subtrees, given it's value.
        /// </summary>
        protected virtual BSTMapNode<TKey, TValue> findNode(BSTMapNode<TKey, TValue> currentNode, TKey key)
        {
            if (currentNode == null)
            {
                return null;
            }

            if (key.IsEqualTo(currentNode.Key))
            {
                return currentNode;
            }

            if (currentNode.HasLeftChild && key.IsLessThan(currentNode.Key))
            {
                return findNode(currentNode.LeftChild, key);
            }

            if (currentNode.HasRightChild && key.IsGreaterThan(currentNode.Key))
            {
                return findNode(currentNode.RightChild, key);
            }

            return null;
        }

        /// <summary>
        /// Returns the min-node in a subtree.
        /// Used in the recusive _remove function.
        /// </summary>
        protected virtual BSTMapNode<TKey, TValue> findMinNode(BSTMapNode<TKey, TValue> node)
        {
            if (node == null)
            {
                return node;
            }

            var currentNode = node;

            while (currentNode.HasLeftChild)
            {
                currentNode = currentNode.LeftChild;
            }

            return currentNode;
        }

        /// <summary>
        /// Returns the max-node in a subtree.
        /// Used in the recusive _remove function.
        /// </summary>
        protected virtual BSTMapNode<TKey, TValue> findMaxNode(BSTMapNode<TKey, TValue> node)
        {
            if (node == null)
            {
                return null;
            }

            var currentNode = node;

            while (currentNode.HasRightChild)
            {
                currentNode = currentNode.RightChild;
            }

            return currentNode;
        }

        /// <summary>
        /// Finds the next smaller node in value compared to the specified node.
        /// </summary>
        protected virtual BSTMapNode<TKey, TValue> findNextSmaller(BSTMapNode<TKey, TValue> node)
        {
            if (node == null)
            {
                return node;
            }

            if (node.HasLeftChild)
            {
                return findMaxNode(node.RightChild);
            }

            var currentNode = node;

            while (currentNode.Parent != null && currentNode.IsLeftChild)
            {
                currentNode = currentNode.Parent;
            }

            return currentNode.Parent;
        }

        /// <summary>
        /// Finds the next larger node in value compared to the specified node.
        /// </summary>
        protected virtual BSTMapNode<TKey, TValue> findNextLarger(BSTMapNode<TKey, TValue> node)
        {
            if (node == null)
            {
                return node;
            }

            if (node.HasRightChild)
            {
                return findMinNode(node.RightChild);

            }

            var currentNode = node;

            while (currentNode.Parent != null && currentNode.IsRightChild)
            {
                currentNode = currentNode.Parent;
            }

            return currentNode.Parent;
        }

        /// <summary>
        /// A recursive private method. Used in the public FindAll(predicate) functions.
        /// Implements in-order traversal to find all the matching elements in a subtree.
        /// </summary>
        protected virtual void findAll(BSTMapNode<TKey, TValue> currentNode, Predicate<TKey> match, ref List<KeyValuePair<TKey, TValue>> list)
        {
            if (currentNode == null)
            {
                return;
            }
            // call left child
            findAll(currentNode.LeftChild, match, ref list);

            if (match(currentNode.Key))
            {
                list.Add(new KeyValuePair<TKey, TValue>(currentNode.Key, currentNode.Value));
            }

            // call the right child
            findAll(currentNode.RightChild, match, ref list);

        }

        /// <summary>
        /// In-order traversal of the subtrees of a node. Returns every node it vists.
        /// </summary>
        protected virtual void inOrderTraverse(BSTMapNode<TKey, TValue> currentNode, ref List<KeyValuePair<TKey, TValue>> list)
        {
            if (currentNode == null)
            {
                return;
            }
            // call the left child
            inOrderTraverse(currentNode.LeftChild, ref list);

            list.Add(new KeyValuePair<TKey, TValue>(currentNode.Key, currentNode.Value));

            // call the right child
            inOrderTraverse(currentNode.RightChild, ref list);

        }

        public virtual int Count
        {
            get => count;
        }

        public virtual bool IsEmpty
        {
            get => count == 0;
        }

        /// <summary>
        /// Returns the height of the tree.
        /// Time-complexity: O(n), where n = number of nodes.
        /// </summary>
        public virtual int Height
        {
            get
            {
                if (IsEmpty)
                {
                    return 0;
                }
                var currentNode = Root;
                return getTreeHeight(currentNode);
            }
        }

        public virtual bool AllowDuplicates
        {
            get => allowDuplicates;
        }

        /// <summary>
        /// Inserts a key-value pair to the tree
        /// </summary>
        public virtual void Insert(TKey key, TValue value)
        {
            BSTMapNode<TKey, TValue> newNode = new(key, value);

            var success = insertNode(newNode);

            if (!success && !allowDuplicates)
            {
                throw new InvalidOperationException("Tree does not allow insert duplicate elements.");
            }
        }

        /// <summary>
        /// Inserts a key-value pair to the tree
        /// </summary>
        public virtual void Insert(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Insert(keyValuePair.Key, keyValuePair.Value);
        }

        /// <summary>
        /// Inserts an array of elements to the tree.
        /// </summary>
        public virtual void Insert(TKey[] collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (collection.Length > 0)
            {
                for (int i = 0; i < collection.Length; ++i)
                {
                    Insert(collection[i], default);
                }
            }
        }

        /// <summary>
        /// Inserts an array of key-value pairs to the tree.
        /// </summary>
        public virtual void Insert(KeyValuePair<TKey, TValue>[] keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException();
            }

            if (keyValuePairs.Length > 0)
            {
                for (int i = 0; i < keyValuePairs.Length; i++)
                {
                    Insert(keyValuePairs[i]);
                }
            }
        }

        /// <summary>
        /// Inserts a list of elements to the tree.
        /// </summary>
        public virtual void Insert(List<TKey> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (collection.Count > 0)
            {
                for (int i = 0; i < collection.Count; ++i)
                {
                    this.Insert(collection[i], default);
                }
            }
        }

        /// <summary>
        /// Inserts a list of elements to the tree.
        /// </summary>
        public virtual void Insert(List<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                throw new InvalidOperationException();
            }

            if (keyValuePairs.Count > 0)
            {
                for (int i = 0; i < keyValuePairs.Count; i++)
                {
                    Insert(keyValuePairs[i]);
                }
            }
        }

        /// <summary>
        /// Updates the node of a specific key with a new value.
        /// </summary>
        public virtual void Update(TKey key, TValue newValue)
        {
            if (IsEmpty)
            {
                throw new Exception("The Tree is Empty!");
            }

            var node = findNode(Root, key);

            if (node == null)
            {
                throw new KeyNotFoundException("Key doesn't exist in tree!");
            }

            node.Value = newValue;
        }

        /// <summary>
        /// Deletes an element from the tree with a specified key.
        /// </summary>
        public virtual void Remove(TKey key)
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = findNode(Root, key);

            if (node == null)
            {
                throw new KeyNotFoundException("Key doesn't exist in tree!");
            }

            remove(node);

        }

        /// <summary>
        /// Removes the min value from tree.
        /// </summary>
        public virtual void RemoveMin()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = findMinNode(Root);

            remove(node);
        }

        /// <summary>
        /// Removes the max value from tree.
        /// </summary>
        public virtual void RemoveMax()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = findMaxNode(Root);
            remove(node);
        }

        /// <summary>
        /// Clears all elements from tree.
        /// </summary>
        public virtual void Clear()
        {
            Root = null;
            count = 0;
        }

        /// <summary>
        /// Checks for the existence of an item
        /// </summary>
        public virtual bool Contains(TKey key)
        {
            return findNode(Root, key) != null;
        }

        /// <summary>
        /// Finds the minimum in tree 
        /// </summary>
        /// <returns>Min</returns>
        public virtual KeyValuePair<TKey, TValue> FindMin()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var minNode = findMinNode(Root);
            return new KeyValuePair<TKey, TValue>(minNode.Key, minNode.Value);
        }

        /// <summary>
        /// Finds the next smaller element in tree, compared to the specified item.
        /// </summary>
        public virtual KeyValuePair<TKey, TValue> FindNextSmaller(TKey key)
        {
            var node = findNode(Root, key);
            var nextSmaller = findNextSmaller(node);

            if (nextSmaller == null)
            {
                throw new Exception("Item was not found!");
            }

            return new KeyValuePair<TKey, TValue>(nextSmaller.Key, nextSmaller.Value);
        }

        /// <summary>
        /// Finds the next larger element in tree, compared to the specified item.
        /// </summary>
        public virtual KeyValuePair<TKey, TValue> FindNextLarger(TKey key)
        {
            var node = findNode(Root, key);
            var nextLarger = findNextLarger(node);

            if (nextLarger == null)
            {
                throw new Exception("Item was not found!");
            }

            return new KeyValuePair<TKey, TValue>(nextLarger.Key, nextLarger.Value);
        }

        /// <summary>
        /// Finds the maximum in tree 
        /// </summary>
        /// <returns>Max</returns>
        public virtual KeyValuePair<TKey, TValue> FindMax()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var maxNode = findMaxNode(Root);
            return new KeyValuePair<TKey, TValue>(maxNode.Key, maxNode.Value);
        }

        /// <summary>
        /// Find the item in the tree. Throws an exception if not found.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>Item.</returns>
        public virtual KeyValuePair<TKey, TValue> Find(TKey key)
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = findNode(Root, key);

            if (node != null)
            {
                return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            }
            throw new KeyNotFoundException("Item was not found!");
        }

        /// <summary>
        /// Given a predicate function, find all the elements that match it.
        /// </summary>
        /// <param name="searchPredicate">The search predicate</param>
        /// <returns>ArrayList<T> of elements.</returns>
        public virtual IEnumerable<KeyValuePair<TKey, TValue>> FindAll(Predicate<TKey> predicate)
        {
            var list = new List<KeyValuePair<TKey, TValue>>();
            findAll(Root, predicate, ref list);
            return list;
        }

        /// <summary>
        /// Returns an array of nodes' values.
        /// </summary>
        /// <returns>The array.</returns>
        public virtual KeyValuePair<TKey, TValue>[] ToArray()
        {
            return ToList().ToArray();
        }

        /// <summary>
        /// Returns a list of the nodes' value.
        /// </summary>
        public virtual List<KeyValuePair<TKey, TValue>> ToList()
        {
            var list = new List<KeyValuePair<TKey, TValue>>();
            inOrderTraverse(Root, ref list);
            return list;
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: parent, left child, right child
        /// </summary>
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetPreOrderEnumerator()
        {
            return new BinarySearchTreePreOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: left child, parent, right child
        /// </summary>
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetInOrderEnumerator()
        {
            return new BinarySearchTreeInOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: left child, right child, parent
        /// </summary>
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetPostOrderEnumerator()
        {
            return new BinarySearchTreePostOrderEnumerator(this);
        }

        /***************************************************************/

        /// <summary>
        /// Returns an preorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinarySearchTreePreOrderEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private BSTMapNode<TKey, TValue> current;
            private BinarySearchTreeMap<TKey, TValue> tree;
            internal Queue<BSTMapNode<TKey, TValue>> traverseQueue;

            public BinarySearchTreePreOrderEnumerator(BinarySearchTreeMap<TKey, TValue> treeMap)
            {
                tree = treeMap;
                // Build queue;
                traverseQueue = new Queue<BSTMapNode<TKey, TValue>>();
                visitNode(tree.Root);
            }

            private void visitNode(BSTMapNode<TKey, TValue> node)
            {
                if (node == null)
                {
                    return;
                }

                traverseQueue.Enqueue(node);
                visitNode(node.LeftChild);
                visitNode(node.RightChild);
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get => new(current.Key, current.Value);
            }

            object IEnumerator.Current => Current;

            /*object IEnumerator<KeyValuePair<TKey, TValue>>.Current
            {
                get => Current;
            }*/

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                {
                    current = traverseQueue.Dequeue();
                }
                else
                {
                    current = null;
                }

                return (current != null);
            }
        }

        internal class BinarySearchTreeInOrderEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private BSTMapNode<TKey, TValue> current;
            private BinarySearchTreeMap<TKey, TValue> tree;
            internal Queue<BSTMapNode<TKey, TValue>> traverseQueue;

            public BinarySearchTreeInOrderEnumerator(BinarySearchTreeMap<TKey, TValue> treeMap)
            {
                tree = treeMap;
                // Build queue;
                traverseQueue = new Queue<BSTMapNode<TKey, TValue>>();
                visitNode(tree.Root);
            }

            private void visitNode(BSTMapNode<TKey, TValue> node)
            {
                if (node == null)
                {
                    return;
                }

                visitNode(node.LeftChild);
                traverseQueue.Enqueue(node);
                visitNode(node.RightChild);
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get => new(current.Key, current.Value);
            }

            object IEnumerator.Current => Current;

            /*object IEnumerator<KeyValuePair<TKey, TValue>>.Current
            {
                get => Current;
            }*/

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                {
                    current = traverseQueue.Dequeue();
                }
                else
                {
                    current = null;
                }

                return (current != null);
            }
        }

        internal class BinarySearchTreePostOrderEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private BSTMapNode<TKey, TValue> current;
            private BinarySearchTreeMap<TKey, TValue> tree;
            internal Queue<BSTMapNode<TKey, TValue>> traverseQueue;

            public BinarySearchTreePostOrderEnumerator(BinarySearchTreeMap<TKey, TValue> treeMap)
            {
                tree = treeMap;
                // Build queue;
                traverseQueue = new Queue<BSTMapNode<TKey, TValue>>();
                visitNode(tree.Root);
            }

            private void visitNode(BSTMapNode<TKey, TValue> node)
            {
                if (node == null)
                {
                    return;
                }

                visitNode(node.LeftChild);
                visitNode(node.RightChild);
                traverseQueue.Enqueue(node);
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get => new(current.Key, current.Value);
            }

            object IEnumerator.Current => Current;

            /*object IEnumerator<KeyValuePair<TKey, TValue>>.Current
            {
                get => Current;
            }*/

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                {
                    current = traverseQueue.Dequeue();
                }
                else
                {
                    current = null;
                }

                return (current != null);
            }
        }

    }
}
