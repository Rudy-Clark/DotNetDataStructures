using System;
using System.Collections;
using System.Collections.Generic;

using DataStructuresDotNet.Common;

namespace DataStructuresDotNet.Trees
{
    public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// Specifies the mod of the travelling through th tree
        /// </summary>
        public enum TraversalMode
        {
            InOrder = 0,
            PreOrder = 1,
            PostOrder = 2,
        }

        protected int count;
        protected bool allowDuplicates;
        protected virtual BSTNode<T> root { get; set; }

        public virtual BSTNode<T> Root
        {
            get => root;
            set => root = value;
        }

        public virtual int Count
        {
            get => count;
        }

        public virtual bool AllowDuplicates
        {
            get => allowDuplicates;
        }

        public virtual bool IsEmpty
        {
            get => count == 0;
        }

        public BinarySearchTree() : this(true) { }
        public BinarySearchTree(bool duplicates)
        {
            count = 0;
            allowDuplicates = duplicates;
            Root = null;
        }

        /// <summary>
        /// Replaces the node's value from it's parent node object with the newValue.
        /// Used in the recusive _remove function.
        /// </summary>
        /// <param name="node">BST node.</param>
        /// <param name="newNode">New value.</param>
        protected virtual void replaceNodeInParent(BSTNode<T> node, BSTNode<T> newNode = null)
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
            else
            {
                Root = newNode;
            }

            if (newNode != null)
            {
                newNode.Parent = node.Parent;
            }
        }

        /// <summary>
        /// Remove the specified node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>>True if removed successfully; false if node wasn't found.</returns>
        protected virtual bool remove(BSTNode<T> node)
        {
            if (node == null)
            {
                return false;
            }

            if (node.ChildrenCount == 2)
            {
                var successor = findNextLarger(node);
                node.Value = successor.Value;
                return (true && remove(successor));
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
                replaceNodeInParent(node, null);
                count--;
            }

            return true;
        }

        /// <summary>
        /// Inserts a new node to the tree.
        /// </summary>
        /// <param name="currentNode">Current node to insert afters.</param>
        /// <param name="newNode">New node to be inserted.</param>
        protected virtual bool insertNode(BSTNode<T> newNode)
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

            if (!allowDuplicates && newNode.Parent.Value.IsEqualTo(newNode.Value))
            {
                return false;
            }

            // Go Left
            if (newNode.Parent.Value.IsGreaterThan(newNode.Value))
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
            // Go Right
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
        /// Calculates the tree height from a specific node, recursively.
        /// Time-complexity: O(n), where n = number of nodes.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Height of node's longest subtree</returns>
        protected virtual int getTreeHeight(BSTNode<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            if (node.IsLeafNode)
            {
                return 1;
            }

            if (node.ChildrenCount == 2)
            {
                return (1 + Math.Max(getTreeHeight(node.LeftChild), getTreeHeight(node.RightChild)));
            }

            if (node.HasLeftChild)
            {
                return (1 + getTreeHeight(node.LeftChild));
            }

            // has only right child
            return (1 + getTreeHeight(node.RightChild));
        }

        /// <summary>
        /// Finds a node inside another node's subtrees, given it's value.
        /// </summary>
        /// <param name="currentNode">Node to start search from.</param>
        /// <param name="item">Search value</param>
        /// <returns>Node if found; otherwise null</returns>
        protected virtual BSTNode<T> findNode(BSTNode<T> currentNode, T item)
        {
            if (currentNode == null)
            {
                return null;
            }

            if (item.IsEqualTo(currentNode.Value))
            {
                return currentNode;
            }

            if (currentNode.HasLeftChild && item.IsLessThan(currentNode.Value))
            {
                return findNode(currentNode.LeftChild, item);
            }

            if (currentNode.HasRightChild && item.IsGreaterThan(currentNode.Value))
            {
                return findNode(currentNode.RightChild, item);
            }

            return null;
        }

        /// <summary>
        /// Returns the min-node in a subtree.
        /// Used in the recusive _remove function.
        /// </summary>
        /// <returns>The minimum-valued tree node.</returns>
        /// <param name="node">The tree node with subtree(s).</param>
        protected virtual BSTNode<T> findMinNode(BSTNode<T> node)
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
        /// <returns>The maximum-valued tree node.</returns>
        /// <param name="node">The tree node with subtree(s).</param>
        protected virtual BSTNode<T> findMaxNode(BSTNode<T> node)
        {
            if (node == null)
            {
                return node;
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
        protected virtual BSTNode<T> findNextSmaller(BSTNode<T> node)
        {
            if (node == null)
            {
                return node;
            }

            if (node.HasLeftChild)
            {
                return findMaxNode(node.LeftChild);
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
        protected virtual BSTNode<T> findNextLarger(BSTNode<T> node)
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
        /// <param name="currentNode">Node to start searching from.</param>
        /// <param name="match"></param>
        protected virtual void findAll(BSTNode<T> currentNode, Predicate<T> match, ref List<T> list)
        {
            if (currentNode == null)
            {
                return;
            }

            // call the left child
            findAll(currentNode.LeftChild, match, ref list);

            if (match(currentNode.Value))
            {
                list.Add(currentNode.Value);
            }

            // call the right child
            findAll(currentNode.RightChild, match, ref list);
        }

        /// <summary>
        /// In-order traversal of the subtrees of a node. Returns every node it vists.
        /// </summary>
        /// <param name="currentNode">Node to traverse the tree from.</param>
        /// <param name="list">List to add elements to.</param>
        protected virtual void inOrderTraverse(BSTNode<T> currentNode, ref List<T> list)
        {
            if (currentNode == null)
            {
                return;
            }

            inOrderTraverse(currentNode.LeftChild, ref list);

            list.Add(currentNode.Value);

            inOrderTraverse(currentNode.RightChild, ref list);
        }

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

        public bool AllowsDuplicates => throw new NotImplementedException();

        /// <summary>
        /// Inserts an element to the tree
        /// </summary>
        /// <param name="item">Item to insert</param>
        public virtual void Insert(T item)
        {
            var newNode = new BSTNode<T>(item);

            var success = insertNode(newNode);

            if (!success && !allowDuplicates)
            {
                throw new InvalidOperationException("Tree does not allow inserting duplicate elements.");
            }
        }

        /// <summary>
        /// Inserts an array of elements to the tree.
        /// </summary>
        public virtual void Insert(T[] collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            if (collection.Length > 0)
            {
                for (int i = 0; i < collection.Length; i++)
                {
                    Insert(collection[i]);
                }
            }
        }

        /// <summary>
        /// Inserts a list of elements to the tree.
        /// </summary>
        public virtual void Insert(List<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (collection.Count > 0)
            {
                for (int i = 0; i < collection.Count; ++i)
                {
                    Insert(collection[i]);
                }
            }
        }

        /// <summary>
        /// Deletes an element from the tree
        /// </summary>
        /// <param name="item">item to remove.</param>
        public virtual void Remove(T item)
        {
            if (IsEmpty)
            {
                throw new Exception("Tree is empty!");
            }

            var node = findNode(Root, item);
            var success = remove(node);

            if (!success)
            {
                throw new Exception("It was not found!");
            }
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
        public virtual bool Contains(T item)
        {
            return findNode(Root, item) != null;
        }

        /// <summary>
        /// Finds the minimum in tree 
        /// </summary>
        /// <returns>Min</returns>
        public virtual T FindMin()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            return findMinNode(Root).Value;
        }

        /// <summary>
        /// Finds the next smaller element in tree, compared to the specified item.
        /// </summary>
        public virtual T FindNextSmaller(T item)
        {
            var node = findNode(Root, item);
            var nextSmaller = findNextSmaller(node);

            if (nextSmaller == null)
            {
                throw new Exception("It was not found!");
            }

            return nextSmaller.Value;
        }

        /// <summary>
        /// Finds the next larger element in tree, compared to the specified item.
        /// </summary>
        public virtual T FindNextLarger(T item)
        {
            var node = findNode(Root, item);
            var nextLarger = findNextLarger(node);

            if (node == null)
            {
                throw new Exception("It was not found!");
            }

            return nextLarger.Value;
        }

        /// <summary>
        /// Finds the maximum in tree 
        /// </summary>
        /// <returns>Max</returns>
        public virtual T FindMax()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            return findMaxNode(Root).Value;
        }

        /// <summary>
        /// Find the item in the tree. Throws an exception if not found.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>Item.</returns>
        public virtual T Find(T item)
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = findNode(Root, item);
            if (node != null)
            {
                return node.Value;
            }

            throw new Exception("Item was not found!");
        }

        public virtual IEnumerable<T> FindAll(Predicate<T> predicate)
        {
            List<T> list = new();
            findAll(Root, predicate, ref list);
            return list;
        }


        /// <summary>
        /// Returns an array of nodes' values.
        /// </summary>
        /// <returns>The array.</returns>
        public virtual T[] ToArray()
        {
            return ToList().ToArray();
        }

        /// <summary>
        /// Returns a list of the nodes' value.
        /// </summary>
        public virtual List<T> ToList()
        {
            List<T> list = new();
            inOrderTraverse(Root, ref list);
            return list;
        }

        /*********************************************************************/


        /// <summary>
        /// Returns an enumerator that visits node in the order: parent, left child, right child
        /// </summary>
        public virtual IEnumerator<T> GetPreOrderEnumerator()
        {
            return new BinarySearchTreePreOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: left child, parent, right child
        /// </summary>
        public virtual IEnumerator<T> GetInOrderEnumerator()
        {
            return new BinarySearchTreeInOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: left child, right child, parent
        /// </summary>
        public virtual IEnumerator<T> GetPostOrderEnumerator()
        {
            return new BinarySearchTreePostOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an preorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinarySearchTreePreOrderEnumerator : IEnumerator<T>
        {
            private BSTNode<T> current;
            private BinarySearchTree<T> tree;
            private Queue<BSTNode<T>> traverseQueue;

            public BinarySearchTreePreOrderEnumerator(BinarySearchTree<T> tree)
            {
                traverseQueue = new Queue<BSTNode<T>>();
                this.tree = tree;
                visitNode(this.tree.Root);
            }

            private void visitNode(BSTNode<T> node)
            {
                if (node == null)
                {
                    return;
                }
                traverseQueue.Enqueue(node);
                visitNode(node.LeftChild);
                visitNode(node.RightChild);
            }

            public T Current
            {
                get => current.Value;
            }

            object IEnumerator.Current
            {
                get => Current;
            }



            public void Dispose()
            {
                current = null;
                tree = null;
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

                return current != null;
            }

            public void Reset()
            {
                current = null;
            }
        }

        /// <summary>
        /// Returns an inorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinarySearchTreeInOrderEnumerator : IEnumerator<T>
        {
            private BSTNode<T> current;
            private Queue<BSTNode<T>> traverseQueue;
            private BinarySearchTree<T> tree;

            public BinarySearchTreeInOrderEnumerator(BinarySearchTree<T> tree)
            {
                this.tree = tree;
                traverseQueue = new Queue<BSTNode<T>>();
                visitNode(this.tree.Root);
            }

            private void visitNode(BSTNode<T> node)
            {
                if (node == null)
                {
                    return;
                }
                visitNode(node.LeftChild);
                traverseQueue.Enqueue(node);
                visitNode(node.RightChild);
            }

            public T Current
            {
                get => current.Value;
            }
            object IEnumerator.Current
            {
                get => Current;
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

                return current != null;
            }

            public void Reset()
            {
                current = null;
            }


            public void Dispose()
            {
                current = null;
                tree = null;
            }
        }

        /// <summary>
        /// Returns a postorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinarySearchTreePostOrderEnumerator : IEnumerator<T>
        {
            private BSTNode<T> current;
            private BinarySearchTree<T> tree;
            private Queue<BSTNode<T>> traverseQueue;

            public BinarySearchTreePostOrderEnumerator(BinarySearchTree<T> tree)
            {
                this.tree = tree;
                traverseQueue = new Queue<BSTNode<T>>();
                visitNode(this.tree.Root);
            }

            public T Current
            {
                get => current.Value;
            }
            object IEnumerator.Current => Current;

            public void Dispose()
            {
                tree = null;
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

                return current != null;
            }

            public void Reset()
            {
                current = null;
            }

            private void visitNode(BSTNode<T> node)
            {
                if (node == null)
                {
                    return;
                }

                visitNode(node.LeftChild);
                visitNode(node.RightChild);
                traverseQueue.Enqueue(node);
            }
        }
    }
}
