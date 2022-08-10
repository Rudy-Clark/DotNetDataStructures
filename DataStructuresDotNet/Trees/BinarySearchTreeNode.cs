using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public class BSTNode<T> : IComparable<BSTNode<T>> where T : IComparable<T>
    {
        private T _value;
        private BSTNode<T> _parent;
        private BSTNode<T> _left;
        private BSTNode<T> _right;

        public BSTNode() : this(default(T), 0, null, null, null) { }
        public BSTNode(T value) : this(value, 0, null, null, null) { }
        public BSTNode(T value, int subTreeSize, BSTNode<T> parent, BSTNode<T> left, BSTNode<T> right)
        {
            Value = value;
            Parent = parent;
            LeftChild = left;
            RightChild = right;
        }

        public virtual T Value
        {
            get => _value;
            set => _value = value;
        }

        public virtual BSTNode<T> Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public virtual BSTNode<T> LeftChild
        {
            get => _left;
            set => _left = value;
        }

        public virtual BSTNode<T> RightChild
        {
            get => _right;
            set => _right = value;
        }

        public virtual bool HasChildren
        {
            get => ChildrenCount > 0;
        }

        public virtual bool HasLeftChild
        {
            get => LeftChild != null;
        }

        public virtual bool HasOnlyRightChild => !HasLeftChild && HasRightChild;

        public virtual bool HasRightChild
        {
            get => RightChild != null;
        }

        public virtual bool HasOnlyLeftChild => !HasRightChild && HasLeftChild;

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
                    count++;
                if (HasRightChild)
                    count++;

                return count;
            }
        }
        int IComparable<BSTNode<T>>.CompareTo(BSTNode<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
