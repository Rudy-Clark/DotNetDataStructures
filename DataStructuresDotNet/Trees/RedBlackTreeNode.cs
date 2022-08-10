using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public class RedBlackTreeNode<TKey> : BSTNode<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public RedBlackTreeNode() : this(default(TKey), 0, null, null, null) { }
        public RedBlackTreeNode(TKey value) : this(value, 0, null, null, null) { }
        public RedBlackTreeNode(TKey value, int height, RedBlackTreeNode<TKey> parent, RedBlackTreeNode<TKey> left, RedBlackTreeNode<TKey> right)
        {
            base.Value = value;
            Color = RedBlackTreeColors.Red;
            Parent = parent;
            LeftChild = left;
            RightChild = right;
        }

        public virtual RedBlackTreeColors Color { get; set; }

        public new RedBlackTreeNode<TKey> Parent
        {
            get => (RedBlackTreeNode<TKey>)base.Parent;
            set => base.Parent = value;
        }

        public new RedBlackTreeNode<TKey> LeftChild
        {
            get => (RedBlackTreeNode<TKey>)base.LeftChild;
            set => base.LeftChild = value;
        }

        public new RedBlackTreeNode<TKey> RightChild
        {
            get => (RedBlackTreeNode<TKey>)base.RightChild;
            set => base.RightChild = value;
        }

        /******************************************************************************/

        /// <summary>
        /// Returns if this node colored red
        /// </summary>
        public virtual bool IsRed
        {
            get => Color == RedBlackTreeColors.Red;
        }

        public virtual bool IsBlack
        {
            get => Color == RedBlackTreeColors.Black;
        }

        /// <summary>
        /// Returns the sibling of this node.
        /// </summary>
        public virtual RedBlackTreeNode<TKey> Sibling
        {
            get => Parent == null ? null : (IsLeftChild ? Parent.RightChild : Parent.LeftChild);
        }

        public virtual RedBlackTreeNode<TKey> GrandParent
        {
            get => Parent?.Parent;
        }
    }
}
