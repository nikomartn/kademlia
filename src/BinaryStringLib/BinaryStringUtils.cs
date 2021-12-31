using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinaryStringLib.Utils
{
    public static class BinaryStringUtils
    {
        public static BinaryString GetNearestBinaryString(BinaryString key, IEnumerable<BinaryString> subjects)
        {
            if (key == null || subjects == null)
                return null;

            BinaryString temp = subjects.First();
            BinaryString distance_temp = (key ^ subjects.First());
            foreach (BinaryString subject in subjects)
            {
                if ((subject ^ key) < distance_temp)
                {
                    temp = subject;
                    distance_temp = (subject ^ key);
                }
            }
            return temp;
        }

        public static BinaryString GenerateRandomBinaryString(HashAlgorithm hashAlgorithm)
        {
            if (hashAlgorithm == null)
                return null;

            const string hex = "0123456789ABCDEF";
            StringBuilder noiseString = new StringBuilder();
            Random r = new Random();
            foreach (int _ in Enumerable.Range(0, (hashAlgorithm.HashSize) / 4))
            {
                noiseString.Append(hex[r.Next(0, 16)]);
            }
            return new BinaryString(noiseString.ToString());
        }

    }
}
