using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Linear
{
    public class SkipListNode<T> : IComparable<SkipListNode<T>> where T : IComparable<T>
    {
        private T _value;
        private SkipListNode<T>[] _forwards;

        public SkipListNode(T value, int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException("Invalid value for level.");

            Forwards = new SkipListNode<T>[level];
            Value = value;
        }

        public virtual SkipListNode<T>[] Forwards
        {
            get => _forwards;
            private set => _forwards = value;
        }

        public virtual T Value
        {
            get => _value;
            private set => _value = value;
        }

        public virtual int Level
        {
            get => Forwards.Length;
        }

        public int CompareTo(SkipListNode<T> other)
        {
            if (other == null)
            {
                return -1;
            }

            return Value.CompareTo(other.Value);
        }
    }
}
