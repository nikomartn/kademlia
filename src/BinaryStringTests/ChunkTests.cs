using BinaryStringLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ChunkTests
{
    public class NiceRepresentation
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FromHex()
        {
            string values = "0123456789ABCDEF";
            for (int i = 0; i < values.Length; i++)
            {
                Chunk c = new Chunk(values[i]);
                Assert.AreEqual(i, c.NumericValue);
            }
        }

        [Test]
        public void FromNumericValue()
        {
            string values = "0123456789ABCDEF";
            for (int i = 0; i < values.Length; i++)
            {
                Chunk c = new Chunk(i);
                Assert.AreEqual(values[i], c.Hex);
            }
        }

    }


    public class NiceAccess
    {
        [Test]
        public void Access15()
        {
            Chunk c = new Chunk(15);
            Assert.IsTrue(c[0] && c[1] && c[2] && c[3]);
        }

        [Test]
        public void Access0()
        {
            Chunk c = new Chunk(0);
            Assert.IsTrue(!c[0] && !c[1] && !c[2] && !c[3]);
        }

        [Test]
        public void Access5()
        {
            Chunk c = new Chunk(5);
            Assert.IsTrue(c[0] && !c[1] && c[2] && !c[3]);
        }

        [Test]
        public void Access10()
        {
            Chunk c = new Chunk(10);
            Assert.IsTrue(!c[0] && c[1] && !c[2] && c[3]);
        }
    }

    public class NiceComparations
    {
        [Test]
        public void Lesser()
        {
            Chunk a = new Chunk(1);
            Chunk b = new Chunk(2);
            Assert.IsTrue(a < b);
        }

        [Test]
        public void Greater()
        {
            Chunk a = new Chunk(1);
            Chunk b = new Chunk(2);
            Assert.IsTrue(b > a);
        }
    }

    public class IComparable
    {
        [Test]
        public void CanBeSorted()
        {
            List<Chunk> list = new List<Chunk>();
            Random r = new Random();
            for (int i = 0; i < 15; i++)
            {
                list.Add(new Chunk(r.Next(0, 15)));
            }

            list.Sort();

            Assert.IsTrue(list[0] < list[^1] || list[0].NumericValue == list[^1].NumericValue);
        }
    }

    public class UpperCaseOrLowerCaseIsValid
    {
        [Test]
        public void UpperCase()
        {
            string values = "0123456789ABCDEF";
            for (int i = 0; i < values.Length; i++)
            {
                Chunk c = new Chunk(values[i]);
                Assert.AreEqual(i, c.NumericValue);
            }
        }

        [Test]
        public void LowerCase()
        {
            string values = "0123456789abcdef";
            for (int i = 0; i < values.Length; i++)
            {
                Chunk c = new Chunk(values[i]);
                Assert.AreEqual(i, c.NumericValue);
            }
        }
    }

    public class Exceptions
    {
        [Test]
        public void IndexOutOfRange()
        {
            Chunk c = new Chunk(0);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { var x = c[16]; });
            Assert.Throws(typeof(IndexOutOfRangeException), () => { c[16] = false; });
            Assert.Throws(typeof(IndexOutOfRangeException), () => { var x = c[-1]; });
            Assert.Throws(typeof(IndexOutOfRangeException), () => { c[-1] = false; });
        }

        [Test]
        public void OverflowException()
        {
            Chunk c = new Chunk(0);
            Assert.Throws(typeof(OverflowException), () => { c.NumericValue = -1; });
            Assert.Throws(typeof(OverflowException), () => { c.NumericValue = 16; });

            Assert.Throws(typeof(OverflowException), () => { c.Hex = 'Z'; });
            Assert.Throws(typeof(OverflowException), () => { c.Hex = 'l'; });
        }
    }

    public class Xor
    {
        [Test]
        public void XorWorks()
        {
            string values = "0123456789ABCDEF";
            var list = new List<Chunk>();
            foreach (char c in values)
            {
                list.Add(new Chunk(c));
            }

            Assert.AreEqual(15, (list[0] ^ list[15]).NumericValue);

            Assert.AreEqual(0, (list[15] ^ list[15]).NumericValue);
            Assert.AreEqual(0, (list[0] ^ list[0]).NumericValue);

            Assert.AreEqual(15, (list[5] ^ list[10]).NumericValue);
        }
    }

}