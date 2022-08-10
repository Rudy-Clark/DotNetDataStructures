using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public class BSTMapNode<TKey, TValue> : IComparable<BSTMapNode<TKey, TValue>> where TKey : IComparable<TKey>
    {
        public BSTMapNode() { }
        public BSTMapNode(TKey key) : this(key, default(TValue), 0, null, null, null) { }
        public BSTMapNode(TKey key, TValue value) : this(key, value, 0, null, null, null) { }
        public BSTMapNode(TKey key, TValue value, int subTreeSize, BSTMapNode<TKey, TValue> parent, BSTMapNode<TKey, TValue> left, BSTMapNode<TKey, TValue> right)
        {
            Key = key;
            Value = value;
            Parent = parent;
            LeftChild = left;
            RightChild = right;
        }

        public virtual TKey Key { get; set; }
        public virtual TValue Value { get; set; }
        public virtual BSTMapNode<TKey, TValue> Parent { get; set; }
        public virtual BSTMapNode<TKey, TValue> LeftChild { get; set; }
        public virtual BSTMapNode<TKey, TValue> RightChild { get; set; }

        public virtual bool HasChildren
        {
            get => ChildrenCount > 0;
        }

        public virtual bool HasLeftChild
        {
            get => LeftChild != null;
        }

        public virtual bool HasRightChild
        {
            get => RightChild != null;
        }

        public virtual bool IsLeftChild
        {
            get => Parent != null && Parent.LeftChild == this;
        }

        public virtual bool IsRightChild
        {
            get => Parent != null && Parent.RightChild == this;
        }

        public virtual bool IsLeafNode
        {
            get => ChildrenCount == 0;
        }

        public virtual int ChildrenCount
        {
            get
            {
                int count = 0;
                if (HasLeftChild)
                {
                    count++;
                }

                if (HasRightChild)
                {
                    count++;
                }

                return count;
            }
        }
        public int CompareTo(BSTMapNode<TKey, TValue> other)
        {
            if (other == null)
            {
                return -1;
            }

            return Key.CompareTo(other.Key);
        }
    }
}
