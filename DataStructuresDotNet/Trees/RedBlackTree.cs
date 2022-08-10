using System;
using System.Collections.Generic;
using DataStructuresDotNet.Common;

namespace DataStructuresDotNet.Trees
{
    public enum RedBlackTreeColors
    {
        Red = 0,
        Black = 1,
    }

    public class RedBlackTree<TKey> : BinarySearchTree<TKey> where TKey : IComparable<TKey>
    {
        public new RedBlackTreeNode<TKey> Root
        {
            get => (RedBlackTreeNode<TKey>)base.Root;
            internal set => base.Root = value;
        }

        private bool IsRoot(RedBlackTreeNode<TKey> node) => node == Root;

        public RedBlackTree() : base() { }
        public RedBlackTree(bool allowDuplicates) : base(allowDuplicates) { }

        /*************************************************************************************************/
        /***
         * Safety Checks/Getters/Setters.
         * 
         * The following are helper methods for safely checking, getting and updating possibly-null objects.
         * These helpers make the algorithms of adjusting the tree after insertion and removal more readable.
         */
        protected RedBlackTreeNode<TKey> safeGetGrandParent(RedBlackTreeNode<TKey> node)
        {
            if (node == null || node.Parent == null)
            {
                return null;
            }

            return node.GrandParent;
        }

        protected RedBlackTreeNode<TKey> safeGetParent(RedBlackTreeNode<TKey> node)
        {
            if (node == null || node.Parent == null)
            {
                return null;
            }

            return node.Parent;
        }

        protected RedBlackTreeNode<TKey> safeGetSibling(RedBlackTreeNode<TKey> node)
        {
            if (node == null || node.Parent == null)
            {
                return null;
            }

            return node.Sibling;
        }

        protected RedBlackTreeNode<TKey> safeGetLeftChild(RedBlackTreeNode<TKey> node)
        {
            if (node == null)
            {
                return null;
            }

            return node.LeftChild;
        }

        protected RedBlackTreeNode<TKey> safeGetRightChild(RedBlackTreeNode<TKey> node)
        {
            if (node == null)
            {
                return null;
            }

            return node.RightChild;
        }

        protected RedBlackTreeColors safeGetColor(RedBlackTreeNode<TKey> node)
        {
            if (node == null)
            {
                return RedBlackTreeColors.Black;
            }

            return node.Color;
        }

        protected virtual void safeUpdateColor(RedBlackTreeNode<TKey> node, RedBlackTreeColors color)
        {
            if (node == null)
            {
                return;
            }

            node.Color = color;
        }

        protected virtual bool safeCheckIsBlack(RedBlackTreeNode<TKey> node) => (node == null) || (node != null && node.IsBlack);

        protected virtual bool safeCheckIsRed(RedBlackTreeNode<TKey> node) => (node != null && node.IsRed);

        /*************************************************************************************************/
        /***
         * Tree Rotations and Adjustements.
         * 
         * The following are methods for rotating the tree (left/right) and for adjusting the 
         * ... tree after inserting or removing nodes.
         */

        /// <summary>
        /// Rotates a node to the left in the Red-Black Tree.
        /// </summary>
        protected virtual void rotateLeftAt(RedBlackTreeNode<TKey> currentNode)
        {
            if (currentNode == null || !currentNode.HasRightChild)
            {
                return;
            }

            RedBlackTreeNode<TKey> pivotNode = currentNode.RightChild;

            RedBlackTreeNode<TKey> parent = currentNode.Parent;

            bool isLeftChild = currentNode.IsLeftChild;

            bool isRootNode = currentNode == Root;

            currentNode.RightChild = pivotNode.LeftChild;
            pivotNode.LeftChild = currentNode;

            currentNode.Parent = pivotNode;
            pivotNode.Parent = parent;

            if (currentNode.HasRightChild)
            {
                currentNode.RightChild.Parent = currentNode;
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
        }

        /// <summary>
        /// Rotates a node to the right in the Red-Black Tree.
        /// </summary>
        protected virtual void rotateRightAt(RedBlackTreeNode<TKey> currentNode)
        {
            if (currentNode == null || !currentNode.HasLeftChild)
            {
                return;
            }

            RedBlackTreeNode<TKey> pivotNode = currentNode.LeftChild;

            RedBlackTreeNode<TKey> parent = currentNode.Parent;

            bool isLeftChild = currentNode.IsLeftChild;

            bool isRootNode = currentNode == Root;

            // perform rotation
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
        }

        /// <summary>
        /// After insertion tree-adjustement helper.
        /// </summary>
        protected virtual void adjustTreeAfterInsertion(RedBlackTreeNode<TKey> currentNode)
        {
            //
            // STEP 1:
            // Color the currentNode as red
            safeUpdateColor(currentNode, RedBlackTreeColors.Red);

            //
            // STEP 2:
            // Fix the double red-consecutive-nodes problems, if there exists any.
            if (currentNode != null && currentNode != Root && safeCheckIsRed(safeGetParent(currentNode)))
            {
                //
                // STEP 2.A:
                // This is the simplest step: Basically recolor, and bubble up to see if more work is needed.
                if (safeCheckIsRed(safeGetSibling(currentNode.Parent)))
                {
                    // If it has a sibling and it is black, then then it has a parent
                    currentNode.Parent.Color = RedBlackTreeColors.Black;

                    // Color sibling of parent as black
                    safeUpdateColor(safeGetSibling(currentNode.Parent), RedBlackTreeColors.Black);

                    // Color grandparent as red
                    safeUpdateColor(safeGetGrandParent(currentNode), RedBlackTreeColors.Red);

                    // Adjust on the grandparent of currentNode
                    adjustTreeAfterInsertion(safeGetGrandParent(currentNode));
                }

                //
                // STEP 2.B:
                // Restructure the tree if the parent of currentNode is a left child to the grandparent of currentNode
                // (parent is a left child to its own parent).
                // If currentNode is also a left child, then do a single right rotation; otherwise, a left-right rotation.
                //
                // using the safe methods to check: currentNode.Parent.IsLeftChild == true
                else if (safeGetParent(currentNode) == safeGetLeftChild(safeGetGrandParent(currentNode)))
                {
                    if (currentNode.IsRightChild)
                    {
                        currentNode = safeGetParent(currentNode);
                        rotateLeftAt(currentNode);
                    }

                    // Color parent as black
                    safeUpdateColor(safeGetParent(currentNode), RedBlackTreeColors.Black);

                    // Color grandparent as red
                    safeUpdateColor(safeGetGrandParent(currentNode), RedBlackTreeColors.Red);

                    // Right Rotate tree around the currentNode's grand parent
                    rotateRightAt(safeGetGrandParent(currentNode));
                }

                //
                // STEP 2.C: 
                // Restructure the tree if the parent of currentNode is a right child to the grandparent of currentNode
                // (parent is a right child to its own parent).
                // If currentNode is a right-child in it's parent, then do a single left rotation; otherwise a right-left rotation.
                //
                // using the safe methods to check: currentNode.Parent.IsRightChild == true
                else if (safeGetParent(currentNode) == safeGetRightChild(safeGetGrandParent(currentNode)))
                {
                    if (currentNode.IsLeftChild)
                    {
                        currentNode = safeGetParent(currentNode);
                        rotateRightAt(currentNode);
                    }

                    // Color parent as black
                    safeUpdateColor(safeGetParent(currentNode), RedBlackTreeColors.Black);

                    // Color grandparent as red
                    safeUpdateColor(safeGetGrandParent(currentNode), RedBlackTreeColors.Red);

                    // Left Rotate tree around the currentNode's grand parent
                    rotateLeftAt(safeGetGrandParent(currentNode));
                }
            }

            // STEP 3:
            // Color the root node as black
            safeUpdateColor(Root, RedBlackTreeColors.Black);
        }

        /// <summary>
        /// After removal tree-adjustement helper.
        /// </summary>
        protected virtual void adjustTreeAfterRemoval(RedBlackTreeNode<TKey> currentNode)
        {
            while (currentNode != null && currentNode != Root && currentNode.IsBlack)
            {
                if (currentNode.IsLeftChild)
                {
                    var sibling = safeGetRightChild(safeGetParent(currentNode));

                    // Safely check sibling.IsRed property
                    if (safeCheckIsRed(sibling))
                    {
                        safeUpdateColor(sibling, RedBlackTreeColors.Black);

                        // Color currentNode.Parent as red
                        safeUpdateColor(safeGetParent(currentNode), RedBlackTreeColors.Red);

                        // Left Rotate on currentNode's parent
                        rotateLeftAt(safeGetParent(currentNode));

                        // Update sibling reference
                        // Might end be being set to null
                        sibling = safeGetRightChild(safeGetParent(currentNode));
                    }


                    // Check if the left and right children of the sibling node are black
                    // Use the safe methods to check for: (sibling.LeftChild.IsBlack && sibling.RightChild.IsBlack)
                    if (safeCheckIsBlack(safeGetLeftChild(sibling)) && safeCheckIsBlack(safeGetRightChild(sibling)))
                    {
                        // Color currentNode.Sibling as red
                        safeUpdateColor(sibling, RedBlackTreeColors.Red);
                        // Assign currentNode.Parent to currentNode 
                        currentNode = safeGetParent(currentNode);
                    }
                    else
                    {
                        if (safeCheckIsBlack(safeGetRightChild(sibling)))
                        {
                            // Color currentNode.Sibling.LeftChild as black
                            safeUpdateColor(safeGetLeftChild(sibling), RedBlackTreeColors.Black);

                            // Color currentNode.Sibling as red
                            safeUpdateColor(sibling, RedBlackTreeColors.Red);

                            // Right Rotate on sibling
                            rotateRightAt(sibling);

                            // Update sibling reference
                            // Might end be being set to null
                            sibling = safeGetRightChild(safeGetParent(currentNode));
                        }

                        // Color the Sibling node as currentNode.Parent.Color
                        safeUpdateColor(sibling, safeGetColor(safeGetParent(currentNode)));

                        // Color currentNode.Parent as black
                        safeUpdateColor(safeGetParent(currentNode), RedBlackTreeColors.Black);

                        // Color Sibling.RightChild as black
                        safeUpdateColor(safeGetRightChild(sibling), RedBlackTreeColors.Black);

                        // Rotate on currentNode's parent
                        rotateLeftAt(safeGetParent(currentNode));

                        currentNode = Root;

                    }
                }
                else
                {
                    var sibling = safeGetLeftChild(safeGetParent(currentNode));

                    if (safeCheckIsRed(sibling))
                    {
                        // Color currentNode.Sibling as black
                        safeUpdateColor(sibling, RedBlackTreeColors.Black);

                        // Color currentNode.Parent as red
                        safeUpdateColor(safeGetParent(currentNode), RedBlackTreeColors.Red);

                        // Right Rotate tree around the parent of currentNode
                        rotateRightAt(safeGetParent(currentNode));

                        // Update sibling reference
                        // Might end be being set to null
                        sibling = safeGetLeftChild(safeGetParent(currentNode));

                    }

                    // Check if the left and right children of the sibling node are black
                    // Use the safe methods to check for: (sibling.LeftChild.IsBlack && sibling.RightChild.IsBlack)
                    if (safeCheckIsBlack(safeGetLeftChild(sibling)) && safeCheckIsBlack(safeGetRightChild(sibling)))
                    {
                        safeUpdateColor(sibling, RedBlackTreeColors.Red);

                        // Assign currentNode.Parent to currentNode 
                        currentNode = safeGetParent(currentNode);

                    }
                    else
                    {
                        // Check if sibling.LeftChild.IsBlack == true
                        if (safeCheckIsBlack(safeGetLeftChild(sibling)))
                        {
                            // Color currentNode.Sibling.RightChild as black
                            safeUpdateColor(safeGetRightChild(sibling), RedBlackTreeColors.Black);

                            // Color currentNode.Sibling as red
                            safeUpdateColor(sibling, RedBlackTreeColors.Red);

                            // Left rotate on sibling
                            rotateLeftAt(sibling);

                            // Update sibling reference
                            // Might end be being set to null
                            sibling = safeGetLeftChild(safeGetParent(currentNode));
                        }

                        // Color the Sibling node as currentNode.Parent.Color
                        safeUpdateColor(sibling, safeGetColor(safeGetParent(currentNode)));

                        // Color currentNode.Parent as black
                        safeUpdateColor(safeGetParent(currentNode), RedBlackTreeColors.Black);

                        // Color Sibling.RightChild as black
                        safeUpdateColor(safeGetLeftChild(sibling), RedBlackTreeColors.Black);

                        rotateRightAt(safeGetParent(currentNode));

                        currentNode = Root;
                    }
                }
            }

            // Color currentNode as black
            safeUpdateColor(currentNode, RedBlackTreeColors.Black);
        }

        /// <summary>
        /// Remove node helpers.
        /// </summary>
        protected override bool remove(BSTNode<TKey> node)
        {
            return remove((RedBlackTreeNode<TKey>)node);
        }

        /// <summary>
        ///     The internal remove helper.
        ///     Separated from the overriden version to avoid casting the objects from BSTNode to RedBlackTreeNode.
        ///     This is called from the overriden _remove(BSTNode nodeToDelete) helper.
        /// </summary>
        protected virtual bool remove(RedBlackTreeNode<TKey> nodeToDelete)
        {
            if (nodeToDelete == null)
            {
                return false;
            }

            if (IsRoot(nodeToDelete) && !nodeToDelete.HasChildren)
            {
                Root = null;
            }
            else
            {
                RedBlackTreeNode<TKey> x;
                if (!nodeToDelete.HasChildren)
                {
                    x = nodeToDelete;
                    Transplant(nodeToDelete, null);
                }
                else if (nodeToDelete.HasOnlyRightChild)
                {
                    x = nodeToDelete.RightChild;
                    Transplant(nodeToDelete, nodeToDelete.RightChild);

                }
                else if (nodeToDelete.HasOnlyLeftChild)
                {
                    x = nodeToDelete.LeftChild;
                    Transplant(nodeToDelete, nodeToDelete.LeftChild);
                }
                else
                {
                    // Y is the node we will replace with the X in the tree once we move it to the nodeToDelete position.
                    var y = (RedBlackTreeNode<TKey>)findMinNode(nodeToDelete.RightChild);
                    x = y.RightChild;

                    if (y.Parent == nodeToDelete)
                    {
                        if (x != null)
                        {
                            x.Parent = y;
                        }
                    }
                    else
                    {
                        Transplant(y, y.RightChild);
                        y.RightChild = nodeToDelete.RightChild;
                        y.RightChild.Parent = y;
                    }

                    Transplant(nodeToDelete, y);
                    y.LeftChild = nodeToDelete.LeftChild;
                    y.LeftChild.Parent = y;
                    y.Color = nodeToDelete.Color;

                    if (Root == nodeToDelete)
                    {
                        Root = y;
                        Root.Parent = null;
                    }
                }

                if (nodeToDelete.Color == RedBlackTreeColors.Black)
                {
                    adjustTreeAfterRemoval(x);
                }
            }

            base.count--;
            return true;
        }

        /// <summary>
        ///     Insert one subtree in the place of the other in his parent.
        /// </summary>
        /// <param name="replaced">Subtree of node will be replaced by <param name="replacement">.</param></param>
        /// <param name="replacement">Subtree replaces <param name="replaced">.</param></para
        private void Transplant(RedBlackTreeNode<TKey> replaced, RedBlackTreeNode<TKey> replacement)
        {
            if (replaced.Parent == null)
            {
                Root = replacement;
            }
            else if (replaced == replaced.Parent.LeftChild)
            {
                replaced.Parent.LeftChild = replacement;
            }
            else
            {
                replaced.Parent.RightChild = replacement;
            }

            if (replacement != null)
            {
                replacement.Parent = replaced.Parent;
            }
        }

        /// <summary>
        /// Insert data item to tree
        /// </summary>
        public override void Insert(TKey item)
        {
            var newNode = new RedBlackTreeNode<TKey>(item);

            // Invoke the super BST insert node method.
            // This insert node recursively starting from the root and checks for success status (related to allowDuplicates flag).
            // The functions increments count on its own.
            var success = base.insertNode(newNode);

            if (!success && !allowDuplicates)
            {
                throw new InvalidOperationException("Tree does not allow inserting duplicate elements.");
            }

            if (!newNode.IsEqualTo(Root))
            {
                if (newNode.Parent.Color != RedBlackTreeColors.Black)
                {
                    adjustTreeAfterInsertion(newNode);
                }
            }

            Root.Color = RedBlackTreeColors.Black;
        }

        /// <summary>
        /// Inserts a collection of elements to the tree.
        /// </summary>
        public override void Insert(List<TKey> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            if (collection.Count > 0)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    Insert(collection[i]);
                }
            }
        }

        /// <summary>
        /// Inserts an array of elements to the tree.
        /// </summary>
        public override void Insert(TKey[] collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (collection.Length > 0)
                for (int i = 0; i < collection.Length; ++i)
                    this.Insert(collection[i]);
        }

        /// <summary>
        /// Removes an item from the tree.
        /// </summary>
        public override void Remove(TKey item)
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = (RedBlackTreeNode<TKey>)findNode(Root, item);

            bool status = remove(node);

            if (!status)
            {
                throw new Exception("Item was not found!");
            }
        }

        /// <summary>
        /// Removes the min value from tree.
        /// </summary>
        public override void RemoveMin()
        {
            if (IsEmpty)
            {
                throw new Exception("Thre tree is empty!");
            }

            var node = (RedBlackTreeNode<TKey>)findMinNode(Root);

            remove(node);
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

            var node = (RedBlackTreeNode<TKey>)findMaxNode(Root);

            remove(node);
        }
    }
}
