using System;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private readonly double[] array;
        private readonly int start;
        private readonly int length;

        public Indexer(double[] array, int start, int length)
        {
            ValidateRange(array, start, length);

            this.array = array;
            this.start = start;
            this.length = length;
        }

        public int Length => length;

        public double this[int index]
        {
            get
            {
                ValidateIndex(index);
                return array[start + index];
            }
            set
            {
                ValidateIndex(index);
                array[start + index] = value;
            }
        }

        private void ValidateRange(double[] array, int start, int length)
        {
            if (start < 0 || length < 0 || start + length > array.Length)
                throw new ArgumentException("Invalid range.");
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= length)
                throw new IndexOutOfRangeException("Index is out of range.");
        }
    }
}