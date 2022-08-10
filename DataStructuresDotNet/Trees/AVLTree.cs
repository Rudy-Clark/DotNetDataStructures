using System;
using System.Collections.Generic;

namespace DataStructuresDotNet.Trees
{
    public class AVLTree<T> : BinarySearchTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// Override the Root node accessors
        /// </summary>
        public new AVLTreeNode<T> Root
        {
            get => (AVLTreeNode<T>)base.Root;
            internal set => base.Root = value;
        }

        public AVLTree() : base() { }
        public AVLTree(bool allowDuplicates) : base(allowDuplicates) { }

        /// <summary>
        /// returns node height
        /// </summary>
        private int getNodeHeight(AVLTreeNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }

            return node.Height;
        }

        /// <summary>
        /// update node heights
        /// </summary>
        private void updateNodeHeight(AVLTreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            node.Height = 1 + Math.Max(getNodeHeight(node.LeftChild), getNodeHeight(node.RightChild));
        }

        /// <summary>
        /// pdates the height of a node and it's parents' recursivley up to the root of the tree
        /// </summary>
        private void updateHeightRecrusive(AVLTreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            node.Height = 1 + Math.Max(getNodeHeight(node.LeftChild), getNodeHeight(node.RightChild));

            updateNodeHeight(node.Parent);
        }

        /// <summary>
        /// Returns the AVL balance factor for a node.
        /// </summary>
        private int getBalanceFactor(AVLTreeNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }

            return (getNodeHeight(node.RightChild) - getNodeHeight(node.LeftChild));
        }

        /// <summary>
        /// Rotates a node to the left in the AVL tree.
        /// </summary>
        private void rotateLeftAt(AVLTreeNode<T> currentNode)
        {
            if (currentNode == null || !currentNode.HasRightChild)
            {
                return;
            }

            AVLTreeNode<T> pivotNode = currentNode.RightChild;

            AVLTreeNode<T> parent = currentNode.Parent;

            bool isLeftChild = currentNode.IsLeftChild;

            bool isRoot = currentNode == Root;

            // Perform rotation
            currentNode.RightChild = pivotNode.LeftChild;
            pivotNode.LeftChild = currentNode;

            currentNode.Parent = pivotNode;
            pivotNode.Parent = parent;

            if (currentNode.HasRightChild)
            {
                currentNode.RightChild.Parent = currentNode;
            }

            if (isRoot)
            {
                Root = pivotNode;
            }

            if (isLeftChild)
            {
                parent.LeftChild = pivotNode;
            }
            else if (parent != null)
            {
                parent.RightChild = pivotNode;
            }

            updateHeightRecrusive(currentNode);
        }

        /// <summary>
        /// Rotates a node to the right in the AVL tree.
        /// </summary>
        private void rotateRightAt(AVLTreeNode<T> currentNode)
        {
            if (currentNode == null || !currentNode.HasLeftChild)
            {
                return;
            }

            AVLTreeNode<T> pivotNode = currentNode.LeftChild;

            AVLTreeNode<T> parent = currentNode.Parent;

            bool isLeftChild = currentNode.IsLeftChild;

            bool isRootNode = currentNode == Root;

            currentNode.LeftChild = pivotNode.RightChild;
            pivotNode.RightChild = currentNode;

            currentNode.Parent = pivotNode;
            pivotNode.Parent = parent;

            if (currentNode.HasLeftChild)
            {
                currentNode.LeftChild.Parent = currentNode;
            }

            if (isRootNode)
            {
                Root = pivotNode;
            }

            if (isLeftChild)
            {
                parent.LeftChild = pivotNode;
            }
            else if (parent != null)
            {
                parent.RightChild = pivotNode;
            }

            updateHeightRecrusive(currentNode);
        }

        /// <summary>
        /// Rebalances the tree around a node.
        /// </summary>
        private void rebalanceSubtreeTreeAt(AVLTreeNode<T> currentNode)
        {
            if (currentNode == null)
            {
                return;
            }

            int balance = getBalanceFactor(currentNode);

            // Balance the tree only if the balance factor was less than -1 or greater than +1.
            if (Math.Abs(balance) >= 2)
            {
                // if balance is positive number
                // right subtee outweighs
                if (balance > 0)
                {
                    int rightSubtreeBalance = getBalanceFactor(currentNode.RightChild);

                    if (rightSubtreeBalance == 0 || rightSubtreeBalance == 1)
                    {
                        // Rotate *LEFT* on current node
                        rotateLeftAt(currentNode);
                    }
                    else if (rightSubtreeBalance == -1)
                    {
                        // Rotate *RIGHT* on right child
                        rotateRightAt(currentNode.RightChild);

                        // Rotate *LEFT* on current node
                        rotateLeftAt(currentNode);
                    }
                }
                // if balance is negative number -2, -3, -4 ... etc
                // left subtree outweighs
                else
                {
                    int leftSubtreeBalance = getBalanceFactor(currentNode.LeftChild);

                    if (leftSubtreeBalance == 0 || leftSubtreeBalance == 1)
                    {
                        // rotate *RIGHT* on current node
                        rotateRightAt(currentNode);
                    }
                    else if (leftSubtreeBalance == -1)
                    {
                        // rotate *LEFT* on left child
                        rotateLeftAt(currentNode.LeftChild);

                        // rotate *RIGHT* on current node
                        rotateRightAt(currentNode);
                    }
                }
            }
        }

        /// <summary>
        /// Rebalances the whole tree around a node.
        /// </summary>
        private void rebalanceTreeAt(AVLTreeNode<T> node)
        {
            var currentNode = node;
            while (currentNode != null)
            {
                // update this node height value
                updateNodeHeight(currentNode);

                var left = currentNode.LeftChild;
                var right = currentNode.RightChild;

                if (getNodeHeight(left) >= 2 + getNodeHeight(right))
                {
                    if (currentNode.HasLeftChild && getNodeHeight(left.LeftChild) >= getNodeHeight(left.RightChild))
                    {
                        rotateRightAt(currentNode);
                    }
                    else
                    {
                        rotateLeftAt(currentNode.LeftChild);
                        rotateRightAt(currentNode);
                    }
                }
                else if (getNodeHeight(right) >= 2 + getNodeHeight(left))
                {
                    if (currentNode.HasRightChild && getNodeHeight(right.RightChild) >= getNodeHeight(right.LeftChild))
                    {
                        rotateLeftAt(currentNode);
                    }
                    else
                    {
                        rotateRightAt(currentNode.RightChild);
                        rotateLeftAt(currentNode);
                    }
                }

                currentNode = currentNode.Parent;
            }
        }

        public override void Insert(T item)
        {
            AVLTreeNode<T> newNode = new() { Value = item };

            // Invoke the super BST insert node method.
            // This insert node recursively starting from the root and checks for success status (related to allowDuplicates flag).
            // The functions increments count on its own.
            var success = base.insertNode(newNode);

            if (!success && !allowDuplicates)
            {
                throw new InvalidOperationException("Tree does not allow insert duplicate elements!");
            }

            rebalanceTreeAt(newNode);
        }

        /// <summary>
        /// Inserts an array of elements to the tree.
        /// </summary>
        public override void Insert(T[] collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (collection.Length > 0)
                for (int i = 0; i < collection.Length; ++i)
                    Insert(collection[i]);
        }

        /// <summary>
        /// Inserts a list of elements to the tree.
        /// </summary>
        public override void Insert(List<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (collection.Count > 0)
                for (int i = 0; i < collection.Count; ++i)
                    Insert(collection[i]);
        }

        /// <summary>
        /// Removes an item fromt he tree
        /// </summary>
        public override void Remove(T item)
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = (AVLTreeNode<T>)base.findNode(Root, item);

            bool success = base.remove(node);

            if (success)
            {
                // Rebalance the tree
                // node.parent is actually the old parent of the node,
                // which is the first potentially out-of-balance node.
                rebalanceTreeAt(node);
            }
            else
            {
                throw new Exception("Item was not found.");
            }

        }

        /// <summary>
        /// Removes the min value from tree.
        /// </summary>
        public override void RemoveMin()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = (AVLTreeNode<T>)findMinNode(Root);

            base.remove(node);

            // Rebalance the tree
            // node.parent is actually the old parent of the node,
            // which is the first potentially out-of-balance node.
            rebalanceTreeAt(node);
        }

        /// <summary>
        /// Removes the max value from tree.
        /// </summary>
        public override void RemoveMax()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = (AVLTreeNode<T>)findMaxNode(Root);

            base.remove(node);

            // Rebalance the tree
            // node.parent is actually the old parent of the node,
            // which is the first potentially out-of-balance node.
            rebalanceTreeAt(node);
        }
    }
}
