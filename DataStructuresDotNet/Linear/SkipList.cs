using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataStructuresDotNet.Common;

namespace DataStructuresDotNet.Linear
{
    public class SkipList<T> : ICollection<T>, IEnumerable<T> where T : IComparable<T>
    {
        private int _count;
        private int _currentMaxLevel;
        private Random _randomizer;

        private SkipListNode<T> _firstNode;

        private readonly int MaxLevel = 32;
        private readonly double Probability = 0.5;

        private int _getNextLevel()
        {
            int lvl = 0;
            
            while (_randomizer.NextDouble() < Probability && lvl <= _currentMaxLevel && lvl < MaxLevel)
            {
                ++lvl;
            }

            return lvl;
        }

        public SkipList()
        {
            _count = 0;
            _currentMaxLevel = 1;
            _randomizer = new Random();
            _firstNode = new SkipListNode<T>(default, MaxLevel);

            for (int i = 0; i < MaxLevel; ++i)
            {
                _firstNode.Forwards[i] = _firstNode;
            }
        }

        public SkipListNode<T> Root { get => _firstNode; }

        public int Count { get => _count; }

        public bool IsReadOnly => false;

        public bool IsEmpty
        {
            get => _count == 0;
        }

        public int Level
        {
            get => _currentMaxLevel;
        }

        /// <summary>
        /// Adds item to the list
        /// </summary>
        public void Add(T item)
        {
            var current = _firstNode;
            var toBeUpdated = new SkipListNode<T>[MaxLevel];

            for (int i = _currentMaxLevel - 1; i >= 0; --i)
            {
                while (current.Forwards[i] != _firstNode && current.Forwards[i].Value.IsLessThan(item))
                {
                    current = current.Forwards[i];
                }

                toBeUpdated[i] = current;
            }

            current = current.Forwards[0];

            int lvl = _getNextLevel();
            if (lvl > _currentMaxLevel)
            {
                for (int i = _currentMaxLevel; i < lvl; ++i)
                {
                    toBeUpdated[i] = _firstNode;
                }

                _currentMaxLevel = lvl;
            }

            var newNode = new SkipListNode<T>(item, lvl);

            for (int i = 0; i < lvl; ++i)
            {
                newNode.Forwards[i] = toBeUpdated[i].Forwards[i];
                toBeUpdated[i].Forwards[i] = newNode;
            }

            ++_count;
        }

        /// <summary>
        /// Remove item from list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            T deleted;
            return Remove(item, out deleted);
        }

        public bool Remove(T item, out T deleted)
        {
            var current = _firstNode;
            var toBeUpdated = new SkipListNode<T>[MaxLevel];

            for (int i = _currentMaxLevel - 1; i >= 0; --i)
            {
                while (current.Forwards[i] != _firstNode && current.Forwards[i].Value.IsLessThan(item))
                {
                    current = current.Forwards[i];
                }

                toBeUpdated[i] = current;
            }

            current = current.Forwards[0];

            if (current.Value.IsEqualTo(item) == false)
            {
                deleted = default(T);
                return false;
            }

            for (int i = 0; i < _currentMaxLevel; ++i)
            {
                if (toBeUpdated[i].Forwards[i] == current)
                {
                    toBeUpdated[i].Forwards[i] = current.Forwards[i];
                }
            }

            // Decrement count;
            --_count;

            while (_currentMaxLevel > 1 && _firstNode.Forwards[_currentMaxLevel - 1] == _firstNode)
            {
                --_currentMaxLevel;
            }

            deleted = current.Value;
            return false;


        }

        public bool Contains(T item)
        {
            T found;
            return Find(item, out found);
        }

        public bool Find(T item, out T found)
        {
            var current = _firstNode;

            for (int i = _currentMaxLevel - 1; i >= 0; --i)
            {
                while (current.Forwards[i] != _firstNode && current.Forwards[i].Value.IsLessThan(item))
                {
                    current = current.Forwards[i];
                }
            }

            current = current.Forwards[0];

            if (current.Value.IsEqualTo(item))
            {
                found = current.Value;
                return true;
            }

            found = default(T);
            return false;

        }

        public void Clear()
        {
            _count = 0;
            _currentMaxLevel = 1;
            _randomizer = new Random();
            _firstNode = new SkipListNode<T>(default(T), MaxLevel);

            for (int i = 0; i < MaxLevel; ++i)
            {
                _firstNode.Forwards[i] = _firstNode;
            }
        }

        public T DeleteMin()
        {
            T deleted;
            if (!TryDeleteMin(out deleted))
            {
                throw new InvalidOperationException("SkipList is Empty!");
            }

            return deleted;
        }

        public bool TryDeleteMin(out T delete)
        {
            if (IsEmpty)
            {
                delete = default(T);
                return false;
            }

            return Remove(_firstNode.Forwards[0].Value, out delete);
        }

        public T Peek()
        {
            T peek;
            if (!TryPeek(out peek))
            {
                throw new InvalidOperationException("SkipList is Empty!");
            }

            return peek;

        }

        public bool TryPeek(out T peek)
        {
            if (IsEmpty)
            {
                peek = default(T);
                return false;
            }

            peek = _firstNode.Forwards[0].Value;
            return true;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (array.Length == 0 || arrayIndex >= array.Length || arrayIndex < 0)
            {
                throw new IndexOutOfRangeException();
            }

            var enumarator = this.GetEnumerator();

            for (int i = arrayIndex; i < array.Length; ++i)
            {
                if (enumarator.MoveNext())
                {
                    array[i] = enumarator.Current;
                } else
                {
                    break;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = _firstNode;
            while (node.Forwards[0] != null && node.Forwards[0] != _firstNode)
            {
                node = node.Forwards[0];
                yield return node.Value;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
