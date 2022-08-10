using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public class BSTRankedNode<T> : BSTNode<T> where T : IComparable<T>
    {
        private int subTreeSize = 0;

        public BSTRankedNode() : this(default(T), 0, null, null, null) { }
        public BSTRankedNode(T value) : this(value, 0, null, null, null) { }
        public BSTRankedNode(T value, int subTreeSize, BSTRankedNode<T> parent, BSTRankedNode<T> left, BSTRankedNode<T> right)
        {
            base.Value = value;
            this.subTreeSize = subTreeSize;
            Parent = parent;
            LeftChild = left;
            RightChild = right;
        }

        public virtual int SubTreeSize
        {
            get => subTreeSize;
            set => subTreeSize = value;
        }

        public new BSTRankedNode<T> Parent
        {
            get { return (BSTRankedNode<T>)base.Parent; }
            set { base.Parent = value; }
        }

        public new BSTRankedNode<T> LeftChild
        {
            get { return (BSTRankedNode<T>)base.LeftChild; }
            set => base.LeftChild = value;
        }

        public new BSTRankedNode<T> RightChild
        {
            get { return (BSTRankedNode<T>)base.RightChild; }
            set => base.RightChild = value;
        }
    }

    /******************************************************************************/


    /// <summary>
    /// Binary Search Tree Data Structure.
    /// This is teh augmented version of BST. It is augmented to keep track of the nodes subtrees-sizes.
    /// </summary>
    public class AugementedBinarySearchTree<T> : BinarySearchTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// Override the Root node accessors.
        /// </summary>
        public new BSTRankedNode<T> Root
        {
            get { return (BSTRankedNode<T>)base.Root; }
            set => base.Root = value;
        }

        /// <summary>
        /// CONSTRUCTOR.
        /// Allows duplicates by default.
        /// </summary>
        public AugementedBinarySearchTree() : base() { }

        /// <summary>
        /// CONSTRUCTOR.
        /// If allowDuplictes is set to false, no duplicate items will be inserted.
        /// </summary>
        public AugementedBinarySearchTree(bool allowDuplicates) : base(allowDuplicates) { }

        /// <summary>
        /// Returns the height of the tree.
        /// </summary>
        /// <returns>Hight</returns>
        public override int Height
        {
            get
            {
                if (IsEmpty)
                {
                    return 0;
                }

                var currentNode = this.Root;
                return _getTreeHeight(currentNode);
            }
        }

        /// <summary>
        /// Returns the Subtrees size for a tree node if node exists; otherwise 0 (left and right nodes of leafs).
        /// This is used in the recursive function UpdateSubtreeSize.
        /// </summary>
        /// <returns>The size.</returns>
        /// <param name="node">BST Node.</param>
        protected int _subTreeSize(BSTRankedNode<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            return node.SubTreeSize;
        }

        /// <summary>
        /// Updates the Subtree Size of a tree node.
        /// Used in recusively calculating the Subtrees Sizes of nodes.
        /// </summary>
        /// <param name="node">BST Node.</param>
        protected void _upadteSubTreeSize(BSTRankedNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            node.SubTreeSize = _subTreeSize(node.LeftChild) + _subTreeSize(node.RightChild) + 1;
            _upadteSubTreeSize(node.Parent);
        }

        /// <summary>
        /// Remove the specified node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>>True if removed successfully; false if node wasn't found.</returns>
        protected bool remove(BSTRankedNode<T> node)
        {
            if (node == null)
            {
                return false;
            }

            var parent = node.Parent;
            if (node.ChildrenCount == 2)
            {
                var successor = findNextLarger(node);
                node.Value = successor.Value;
                return remove(successor);
            }

            if (node.HasLeftChild)
            {
                base.replaceNodeInParent(node, node.LeftChild);
                _upadteSubTreeSize(parent);
                count--;
            }
            else if (node.HasRightChild)
            {
                base.replaceNodeInParent(node, node.RightChild);
                _upadteSubTreeSize(parent);
                count--;
            }
            else
            {
                base.replaceNodeInParent(node, null);
                _upadteSubTreeSize(parent);
                count--;
            }

            return true;
        }

        protected int _getTreeHeight(BSTRankedNode<T> node)
        {
            if (node == null || !node.HasChildren)
            {
                return 0;
            }

            if (node.ChildrenCount > 2)
            {
                if (node.LeftChild.SubTreeSize > node.RightChild.SubTreeSize)
                {
                    return (1 + _getTreeHeight(node.LeftChild));
                }
                return (1 + _getTreeHeight(node.RightChild));
            }

            if (node.HasLeftChild)
            {
                return (1 + _getTreeHeight(node.LeftChild));

            }

            if (node.HasRightChild)
            {
                return (1 + _getTreeHeight(node.RightChild));

            }

            return 0;
        }

        public override void Insert(T item)
        {
            BSTRankedNode<T> newNode = new(item);

            var success = base.insertNode(newNode);
            if (!success && !allowDuplicates)
            {
                throw new InvalidOperationException("Tree doesn't allow insert duplicate elements.");
            }

            _upadteSubTreeSize(newNode.Parent);
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

        public override void Remove(T item)
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is empty!");
            }

            var node = (BSTRankedNode<T>)base.findNode(Root, item);
            bool status = remove(node);
            _upadteSubTreeSize(node.Parent);

            if (!status)
            {
                throw new Exception("Item was not found!");
            }
        }

        public override void RemoveMin()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is epmty!");
            }
            var minNode = (BSTRankedNode<T>)findMinNode(Root);
            var parent = minNode.Parent;
            remove(minNode);

            _upadteSubTreeSize(parent);

        }

        public override void RemoveMax()
        {
            if (IsEmpty)
            {
                throw new Exception("The tree is epmty!");
            }
            var maxNode = (BSTRankedNode<T>)findMaxNode(Root);
            var parent = maxNode.Parent;
            remove(maxNode);

            _upadteSubTreeSize(parent);
        }

        /// <summary>
        /// Returns the rank of the specified element
        /// </summary>
        /// <param name="item">Tree element</param>
        /// <returns>Rank(item) if found; otherwise throws an exception.</returns>
        public virtual int Rank(T item)
        {
            var node = (BSTRankedNode<T>)findNode(Root, item);
            if (node == null)
            {
                throw new Exception("Item was not found!");
            }

            return (_subTreeSize(node.LeftChild) + 1);
        }
    }
}
