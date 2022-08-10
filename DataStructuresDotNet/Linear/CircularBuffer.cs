using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Linear
{
    public class CircularBuffer<T> : IEnumerable<T>, ICollection<T> where T : IComparable<T>
    {
        private T[] circularBuffer;
        private int end;
        private int start;
        private static readonly int defaultBuffertLength = 10;


        /// <summary>
        /// Returns the length of the buffer
        /// </summary>
        public int Length { get => circularBuffer.Length - 1; }

        /// <summary>
        ///  Checks if no element is inserted into the buffer
        /// </summary>
        public bool IsEmpty { get => count == 0; }

        /// <summary>
        /// Checks if the buffer is filled up
        /// </summary>
        public bool IsFilledUp
        {
            get => ((end + 1) % circularBuffer.Length == start) && !circularBuffer[start].Equals(circularBuffer[end]);
        }

        /// <summary>
        /// Controls whether data should be overridden when it is continously inserted without reading
        /// </summary>
        public bool CanOverride { get; private set; }

        /// <summary>
        /// Initializes a circular buffer with initial length of 10
        /// </summary>
        public CircularBuffer(bool canOverride = true) : this(defaultBuffertLength, canOverride) { }

        public CircularBuffer(int length, bool canOverride = true)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException("Length can not negative or zero!");
            }

            circularBuffer = new T[length - 1];
            end = 0;
            start = 0;
            CanOverride = canOverride;
        }

        /// <summary>
        /// Writes value to the back of the buffer
        /// </summary>
        /// <typeparam name="value">value to be added to the buffer</typeparam>
        public void Add(T value)
        {
            if (!CanOverride && IsFilledUp)
            {
                throw new CirculaBufferFullException($"Circullar buffer is filled up. {value} can not be inserted");
            }

            innerInsert(value);
        }

        public void innerInsert(T value)
        {

        }

    }

    public class CirculaBufferFullException : Exception
    {
        public CirculaBufferFullException(string message) : base(message)
        {

        }
    }
}
