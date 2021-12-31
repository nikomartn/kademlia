using BinaryStringLib;
using NUnit.Framework;
using System.Collections.Generic;
namespace BinaryStringTests
{
    class NiceRepresentation
    {
        [Test]
        public void FromHex()
        {
            BinaryString bs = new BinaryStringLib.BinaryString("E6");
            Assert.AreEqual(8, bs.BitSize);
            Assert.AreEqual("E6", bs.StringHex);

            bs = new BinaryStringLib.BinaryString("FFFFE6");
            Assert.AreEqual(24, bs.BitSize);
            Assert.AreEqual("FFFFE6", bs.StringHex);
        }

        [Test]
        public void FromDefault()
        {
            BinaryStringLib.BinaryString bs = new BinaryStringLib.BinaryString();
            Assert.AreEqual(1, bs.BitSize);
            Assert.AreEqual("0", bs.StringHex);
        }
    }

    public class NiceComparations
    {
        [Test]
        public void Greater()
        {
            BinaryStringLib.BinaryString a = new BinaryStringLib.BinaryString("FF");
            BinaryStringLib.BinaryString b = new BinaryStringLib.BinaryString("F5");

            //Assert.IsTrue(a > b);
        }

        [Test]
        public void Lesser()
        {
            BinaryStringLib.BinaryString a = new BinaryStringLib.BinaryString("FF");
            BinaryStringLib.BinaryString b = new BinaryStringLib.BinaryString("F5");

            //Assert.IsTrue(b < a);
        }
    }

    public class UpperCaseOrLowerCaseAreValid
    {
        [Test]
        public void UpperCase()
        {
            var bs = new BinaryStringLib.BinaryString("0123456789ABCDEF");
            Assert.AreEqual("123456789ABCDEF", bs.StringHex);
        }

        [Test]
        public void LowerCase()
        {
            var bs = new BinaryStringLib.BinaryString("0123456789abcdef");
            Assert.AreEqual("123456789ABCDEF", bs.StringHex);
        }
    }

    public class IComparable
    {
        [Test]
        public void Sort()
        {
            List<BinaryStringLib.BinaryString> list = new List<BinaryStringLib.BinaryString>();
            list.Add(new BinaryStringLib.BinaryString("00ff00"));
            list.Add(new BinaryStringLib.BinaryString("FFFFFF"));
            list.Add(new BinaryStringLib.BinaryString("0"));

            list.Sort();

            Assert.AreEqual("0", list[0].StringHex);
            Assert.AreEqual("FFFFFF", list[^1].StringHex);

        }
    }

    public class Xor
    {
        [Test]
        public void XorWorks()
        {
            var a = new BinaryStringLib.BinaryString("FFFF");
            var b = new BinaryStringLib.BinaryString("0000");
            Assert.AreEqual("FFFF", (a ^ b).ToString());
            //Assert.AreEqual("FFFF", (b ^ a).ToString());

            var c = new BinaryStringLib.BinaryString("0");
            //Assert.AreEqual("FFFF", (a ^ c).ToString());

            a = new BinaryStringLib.BinaryString("A");
            b = new BinaryStringLib.BinaryString("5");
            //Assert.AreEqual("F", (a ^ b).ToString());

            a = new BinaryStringLib.BinaryString("0F0F0F");
            b = new BinaryStringLib.BinaryString("f0f0f0");
            //Assert.AreEqual("FFFFFF", (a ^ b).ToString());
        }
    }
}
