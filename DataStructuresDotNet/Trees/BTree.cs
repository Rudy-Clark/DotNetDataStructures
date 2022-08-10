using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public class BTree<T> where T : IComparable<T>
    {
        private int _count;
        private BTreeNode<T> _root;
        private int _minDegree;

        public BTreeNode<T> Root
        {
            get => _root;
        }

        public BTree(int degree)
        {
            _minDegree = degree;
            _count = 0;
        }

        /// <summary>
        /// Inserts an item to the tree.
        /// </summary>
        public void Insert(T value)
        {
            if (_root == null)
            {
                _root = new BTreeNode<T>(_minDegree);
                _root.Keys.Add(value);
            }
            else
            {
                if (_root.Keys.Count >= 2 * _minDegree - 1)
                {
                    var newRoot = new BTreeNode<T>(_minDegree);
                    newRoot.Children.Add(_root);
                    newRoot.SplitChild(0, _root);

                    var i = 0;
                    if (value.CompareTo(newRoot.Keys[0]) > 0)
                    {
                        i++;
                    }

                    newRoot.Children[i].InsertNonFull(value);
                    _root = newRoot;
                }
                else
                {
                    _root.InsertNonFull(value);
                }
            }
        }

        /// <summary>
        /// Finds the Node that holds the given value.
        /// </summary>
        public BTreeNode<T> Search(T value)
        {
            if (_root == null)
            {
                Console.WriteLine("The B Tree is empty");
                return null;
            }

            return _root.Search(value);
        }

        /// <summary>
        /// Removes an item from the tree
        /// </summary>
        public void Remove(T value)
        {
            if (_root == null)
            {
                Console.WriteLine("The B Tree is empty");

            }

            _root.Remove(value);
            if (_root.Keys.Count == 0)
            {
                if (_root.IsLeaf)
                {
                    _root = null;
                }
                else
                {
                    _root = _root.Children[0];

                }
            }
        }
    }
}
