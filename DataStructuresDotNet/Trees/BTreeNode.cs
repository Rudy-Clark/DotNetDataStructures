using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    public class BTreeNode<T> : IComparable<BTreeNode<T>> where T : IComparable<T>
    {
        private BTreeNode<T> _parent;
        private List<BTreeNode<T>> _children;
        private List<T> _keys;
        private int _degree;

        public BTreeNode(int degree)
        {
            _degree = degree;
            _children = new List<BTreeNode<T>>(degree * 2 - 1);
            _keys = new List<T>(degree * 2 - 1);
        }

        public BTreeNode<T> Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public List<BTreeNode<T>> Children
        {
            get => _children;
            set => _children = value;
        }

        public List<T> Keys
        {
            get => _keys;
            set => _keys = value;
        }

        public bool IsLeaf
        {
            get => _children.Count == 0;
        }

        /// <summary>
        /// A utility function that returns the index of the first key
        /// that is greater than or equal to k.
        /// </summary>
        public int FindKey(T value)
        {
            return _keys.FindLastIndex((searching) => value.CompareTo(searching) > 0) + 1;
        }

        public void Remove(T value)
        {
            var index = FindKey(value);

            if (index < Keys.Count && value.CompareTo(Keys[index]) == 0)
            {
                if (IsLeaf)
                {
                    RemoveFromLeaf(index);
                }
                else
                {
                    RemoveFromNonLeaf(index);
                }
            }
            else
            {
                if (IsLeaf)
                    return;

                var didMergeLast = index == Children.Count;

                if (Children[index].Keys.Count < _degree)
                {
                    Fill(index);
                }

                if (didMergeLast && index > Children.Count)
                {
                    Children[index - 1].Remove(value);

                }
                else
                {
                    Children[index].Remove(value);
                }
            }

        }

        /// <summary>
        /// Removes the key at index from this leaf node.
        /// </summary>
        public void RemoveFromLeaf(int index)
        {
            Keys.RemoveAt(index);
        }



        /// <summary>
        /// Removes the key at index from this non-leaf node.
        /// </summary>
        public void RemoveFromNonLeaf(int index)
        {
            // If the child that precedes our key has atleast this._degree keys, 
            // find the predecessor of our key in the subtree rooted at the child 
            // at index. Replace our key by it's pred. Recursively delete pred in
            // the list of children.
            if (Children[index].Keys.Count >= _degree)
            {
                var pred = GetPredecessor(index);
                Keys[index] = pred;
                Children[index].Remove(pred);
            }
            // If the child has less than this._degree keys, examine the child on 
            // the other side. If it has atleast this._degree keys, find the successor
            // of the key in the subtree rooted at our next child. Replace the key with
            // the successor. Recursively delete the successor in the next child.
            else if (Children[index + 1].Keys.Count >= _degree)
            {
                var succ = GetSuccessor(index);
                Keys[index] = succ;
                Children[index + 1].Remove(succ);
            }
            // If both the previous child and the next child has less than this._degree 
            // keys, merge our key and all of the next child into the previous child.
            // Now the previous child contains 2 * this._degree - 1 keys. Recursively 
            // delete our key from the previous child.
            else
            {
                Merge(index);
                Children[index].Remove(Keys[index]);
            }
        }

        public void Fill(int index)
        {
            if (index != 0 && Children[index - 1].Keys.Count >= _degree)
            {
                BorrowFromPrevious(index);
            }
            else if (index != Keys.Count && Children[index].Keys.Count >= _degree)
            {
                BorrowFromNext(index);
            }
            else
            {
                if (index != Children.Count - 1)
                {
                    Merge(index);

                }
                else
                {
                    Merge(index - 1);
                }

            }

        }

        /// <summary>
        /// Gets the lowest value in the tree rooted at the child at index+1.
        /// </summary>
        private T GetSuccessor(int index)
        {
            var node = Children[index + 1];
            while (!node.IsLeaf)
            {
                node = Children[0];
            }

            return node.Keys[0];
        }

        /// <summary>
        /// Gets the highest value in the tree rooted at the child at index.
        /// </summary>
        private T GetPredecessor(int index)
        {
            var node = Children[index];
            while (!node.IsLeaf)
            {
                node = node.Children[node.Children.Count - 1];
            }

            return node.Keys[node.Keys.Count - 1];
        }

        /// <summary>
        /// Merges the child at index with the child at index+1.
        /// </summary>
        private void Merge(int index)
        {
            var child = Children[index];
            var sibling = Children[index + 1];

            // Add our key and siblings keys to the child
            child.Keys.Insert(_degree - 1, Keys[index]);
            Keys.RemoveAt(index);
            child.Keys.AddRange(sibling.Keys);

            // Node move the children
            if (!child.IsLeaf)
            {
                child.Children.AddRange(sibling.Children);
            }

            Children.RemoveAt(index + 1);
        }

        /// <summary>
        /// Pulls a key from the previous sibling and inserts it in the child
        /// at index.
        /// </summary>
        private void BorrowFromPrevious(int index)
        {
            var child = Children[index];
            var sibling = Children[index - 1];

            child.Keys.Insert(0, Keys[index - 1]);
            Keys[index - 1] = sibling.Keys[sibling.Keys.Count - 1];
            sibling.Keys.RemoveAt(sibling.Keys.Count - 1);

            // Rotate children, if its not a leaf node
            if (!child.IsLeaf)
            {
                child.Children.Insert(0, sibling.Children[sibling.Keys.Count - 1]);
                sibling.Children.RemoveAt(sibling.Keys.Count - 1);
            }
        }

        /// <summary>
        /// Pulls a key from the next sibling and inserts it in the child
        /// at index.
        /// </summary>
        private void BorrowFromNext(int index)
        {
            var child = Children[index];
            var sibling = Children[index + 1];

            child.Keys.Add(Keys[index]);
            Keys[index] = sibling.Keys[0];
            sibling.Keys.RemoveAt(0);

            if (!child.IsLeaf)
            {
                child.Children.Add(sibling.Children[0]);
                sibling.Children.RemoveAt(0);

            }
        }

        /// <summary>
        /// Finds the Node that holds the given value.
        /// </summary>
        public BTreeNode<T> Search(T value)
        {
            var found = Keys.FindIndex(searching => value.CompareTo(searching) == 0);

            if (found != -1)
            {
                return this;
            }
            if (found == -1 && this.IsLeaf)
            {
                return null;
            }

            found = Keys.FindLastIndex(searching => value.CompareTo(searching) > 0) + 1;
            return Children[found].Search(value);

        }


        /// <summary>
        /// Assumes value can be inserted. Callers should verify this.Keys has
        /// enough space.
        /// </summary>
        public void InsertNonFull(T value)
        {
            if (IsLeaf)
            {
                var i = Keys.FindLastIndex(
                        delegate (T compare)
                        {
                            return value.CompareTo(compare) > 0;
                        }
                    ) + 1;
                Keys.Insert(i, value);

            } else
            {
                var i = Keys.FindLastIndex(compare => value.CompareTo(compare) > 0) + 1;
                if (Children[i].Keys.Count >= 2 * _degree - 1)
                {
                    SplitChild(i, Children[i]);

                }

                if (value.CompareTo(Keys[i]) > 0)
                {
                    i++;
                }

                Children[i].InsertNonFull(value);
            }
        }


        public void SplitChild(int i, BTreeNode<T> child)
        {
            var node = new BTreeNode<T>(child._degree);

            var mid = child.Keys[child._degree - 1];
            node.Keys = child.Keys.GetRange(child._degree, child._degree - 1);
            child.Keys = child.Keys.GetRange(0, child._degree - 1);

            if (!child.IsLeaf)
            {
                node.Children = child.Children.GetRange(child._degree, child._degree);
                child.Children = child.Children.GetRange(0, child._degree);

            }

            Children.Insert(i + 1, node);
            Keys.Insert(i, mid);
        }

        public virtual int CompareTo(BTreeNode<T> bTreeNode)
        {
            if (bTreeNode == null)
            {
                return -1;
            }

            if (Children.Count != bTreeNode.Children.Count)
            {
                return -1;
            }

            return 0;
        }
    }
}
