using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStringLib
{
    public class BinaryString : IComparable<BinaryString>
    {
        List<Chunk> _chunks = new List<Chunk>();

        #region Properties

        public bool this[int i]
        {
            get
            {
                if (i > (_chunks.Count * 4)) return false;
                return _chunks[i / 4][i % 4];
            }
            set
            {
                while (i > (_chunks.Count * 4))
                {
                    _chunks.Add(new Chunk(0));
                }
                _chunks[i / 4][i % 4] = value;
            }
        }

        public int BitSize
        {
            get
            {
                int t = (_chunks.Count - 1) * 4;
                int i;
                for (i = 3; i >= 0; i--)
                {
                    if (_chunks[_chunks.Count - 1][i] || i == 0)
                        break;
                }
                return (t + i) + 1;
            }
        }

        public string StringHex
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (Chunk chunk in _chunks)
                {
                    sb.Append(chunk.Hex);
                }
                return ReverseString(sb.ToString());
            }
            set
            {
                _chunks = new List<Chunk>();
                value = ReverseString(value); // Reverse the string, since we want LittleEndian [So the size is irrelevant to BinaryString]
                while (value[value.Length - 1] == '0' && value.Length > 1)  // Trim miningless 0's, that is 0x 00A0 -> 0x A0
                {
                    value = value.Remove(value.Length - 1, 1);
                }
                for (int i = 0; i < value.Length; i++)
                {
                    _chunks.Add(new Chunk(value[i]));
                }
            }
        }

        #endregion

        #region Ctor

        public BinaryString()
        {
            StringHex = "0";
        }

        public BinaryString(string stringHex)
        {
            StringHex = stringHex;
        }

        // TODO: I see trouble here with the endianess 0011_0001 -> 1000_11000 != 0001_0011
        // It is supposed that it is irrelevant at bit level, the endiannes only works at byte level, but i'm not so sure..
        public BinaryString(byte[] byteArray)
        {
            var hexString = BitConverter.ToString(byteArray);
            if (!BitConverter.IsLittleEndian)
                hexString = ReverseString(hexString);

            StringHex = new String(hexString.Where(ch => Chunk.HexValues.Contains(ch)).ToArray());
        }

        #endregion

        #region IComparable<BinaryString>

        /* Compare chunks from the Greaterwise chunk */
        public int CompareTo(BinaryString other)
        {
            int maxLength = _chunks.Count > other._chunks.Count ? _chunks.Count : other._chunks.Count;
            maxLength -= 1;

            Chunk zero = new Chunk(0);
            for (int i = maxLength; i >= 0; i--)
            {
                if (ChunkAt(i) > other.ChunkAt(i)) return 1;
                else if (ChunkAt(i) < other.ChunkAt(i)) return -1;
            }
            return 0;
        }

        #endregion

        #region Operators

        public static bool operator >(BinaryString a, BinaryString b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(BinaryString a, BinaryString b)
        {
            return a.CompareTo(b) < 0;
        }

        public static BinaryString operator ^(BinaryString a, BinaryString b)
        {
            BinaryString temp_bs = new BinaryString();
            int maxLength = a._chunks.Count > b._chunks.Count ? a._chunks.Count : b._chunks.Count;
            temp_bs._chunks = new List<Chunk>();
            for (int i = 0; i < maxLength; i++)
            {
                temp_bs._chunks.Add(a.ChunkAt(i) ^ b.ChunkAt(i));
            }

            return new BinaryString(temp_bs.StringHex);
        }

        #endregion

        #region Helpers

        private static string ReverseString(string toReverse)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = toReverse.Length - 1; i >= 0; i--)
            {
                sb.Append(toReverse[i]);
            }
            return sb.ToString();
        }

        internal Chunk ChunkAt(int index)
        {
            if (index > _chunks.Count - 1) //Unaccesible Chunk
                return new Chunk(0);
            else
                return _chunks[index];
        }

        #endregion

        public override string ToString()
        {
            return StringHex;
        }
    }
}
