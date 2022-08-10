using System;

namespace DataStructuresDotNet.Trees
{
    public class AVLTreeNode<T> : BSTNode<T> where T : IComparable<T>
    {
        private int height;

        public AVLTreeNode() : this(default(T), 0, null, null, null) { }
        public AVLTreeNode(T value) : this(value, 0, null, null, null) { }
        public AVLTreeNode(T value, int height, AVLTreeNode<T> parent, AVLTreeNode<T> left, AVLTreeNode<T> right) 
        {
            base.Value = value;
            Height = height;
        }

        public virtual int Height
        {
            get => height;
            set => height = value;
        }

        public new virtual AVLTreeNode<T> Parent
        {
            get => (AVLTreeNode<T>)base.Parent;
            set => base.Parent = value;
        }

        public new virtual AVLTreeNode<T> LeftChild
        {
            get => (AVLTreeNode<T>)base.LeftChild;
            set => base.LeftChild = value;
        }

        public new virtual AVLTreeNode<T> RightChild
        {
            get => (AVLTreeNode<T>)base.RightChild;
            set => base.RightChild = value;
        }
    }
}
