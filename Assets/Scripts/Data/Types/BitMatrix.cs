using System;
using System.Collections;

namespace Data.Types
{
    public class BitMatrix
    {
        public readonly int Dimensions;

        private BitArray[] _values;

        public BitMatrix(int length)
        {
            _values = new BitArray[length];

            for (int i = 0; i < _values.Length; ++i)
            {
                _values[i] = new BitArray(length);
            }

            Dimensions = length;
        }

        public bool GetValue(int row, int column)
        {
            return _values[row][column];
        }

        public bool TryGetValue(int row, int column, out bool value)
        {
            if (row > _values.Length || column > _values.Length)
            {
                value = false;

                return value;
            }

            value = GetValue(row, column);

            return value;
        }

        public void SetValue(int row, int column, bool value)
        {
            _values[row][column] = value;
        }

        public bool TrySetValue(int row, int column, bool value)
        {
            if(row > _values.Length || column > _values.Length)
            {
                return false;
            }

            SetValue(row, column, value);

            return true;
        }

        public void ClearAllRow(int row)
        {
            for (int i = 0; i < Dimensions; i++)
            {
                SetValue(row, i, false);
            }
        }

        public void ClearAllColumn(int column)
        {
            for (int i = 0; i < Dimensions; i++)
            {
                SetValue(i, column, false);
            }
        }

    }
}
