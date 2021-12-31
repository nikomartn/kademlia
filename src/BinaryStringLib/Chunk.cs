using System;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("BinaryStringTests")]

namespace BinaryStringLib
{
    internal class Chunk : IComparable<Chunk>
    {
        public static string HexValues = "0123456789ABCDEF";

        protected bool[] _bits = { false, false, false, false };

        #region Properties

        public bool this[int i]
        {
            get
            {
                if (i < 0 || i > 3)
                {
                    throw new IndexOutOfRangeException($"Accepted range for Chunk[i] is [0,3], received value: {i}");
                }
                return _bits[i];
            }
            set
            {
                if (i < 0 || i > 3)
                {
                    throw new IndexOutOfRangeException($"Accepted range for Chunk[i] is [0,3], received value: {i}");
                }
                _bits[i] = value;
            }
        }

        public int NumericValue
        {
            get
            {
                return (_bits[0] ? 1 : 0) + (_bits[1] ? 2 : 0) + (_bits[2] ? 4 : 0) + (_bits[3] ? 8 : 0);
            }
            set
            {
                if (value > 15 || value < 0)
                {
                    throw new OverflowException($"Accepted range for Chunk.NumericValue is [0,15], received value : {value}");
                }

                _bits = new bool[] { false, false, false, false };
                int i = 0;
                while (value > 0)
                {
                    if (value == 1)
                    {
                        _bits[i] = true;
                        return;
                    }

                    _bits[i++] = (value % 2 == 1);

                    if (value == 0) return;
                    value /= 2;
                }
            }
        }

        public char Hex
        {
            get
            {
                return HexValues[NumericValue];
            }
            set
            {
                value = ToUpper(value);
                for (int i = 0; i < HexValues.Length; i++)
                {
                    if (value == HexValues[i])
                    {
                        NumericValue = i;
                        return;
                    }
                }
                throw new OverflowException($"Accepted range for Chunk.Hex is [{HexValues}], received value : {value}");

            }
        }

        #endregion

        #region Ctor

        public Chunk(int value)
        {
            this.NumericValue = value;
        }

        public Chunk(char hexChar)
        {
            this.Hex = hexChar;
        }

        #endregion

        #region IComparable<Chunk>

        public int CompareTo(Chunk other)
        {
            for (int i = 3; i >= 0; i--)
            {
                if (this[i] && !other[i]) return 1;
                else if (!this[i] && other[i]) return -1;
            }
            return 0;
        }

        #endregion

        #region Operators

        public static bool operator >(Chunk a, Chunk b)
        {
            return (a.CompareTo(b) > 0);
        }

        public static bool operator <(Chunk a, Chunk b)
        {
            return (a.CompareTo(b) < 0);
        }

        public static Chunk operator ^(Chunk a, Chunk b)
        {
            Chunk ret = new Chunk(0);
            ret[0] = a[0] ^ b[0];
            ret[1] = a[1] ^ b[1];
            ret[2] = a[2] ^ b[2];
            ret[3] = a[3] ^ b[3];
            return ret;
        }

        #endregion

        #region Helpers

        private char ToUpper(char value)
        {
            switch (value)
            {
                case 'a': return 'A';
                case 'b': return 'B';
                case 'c': return 'C';
                case 'd': return 'D';
                case 'e': return 'E';
                case 'f': return 'F';
                default: return value;
            }
        }

        #endregion
    }
}

