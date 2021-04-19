using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace Singulink.Numerics.BigIntegerExtensions.Tests
{
    [TestClass]
    public class DivideTests
    {
        [TestMethod]

        public void RoundToEven_ZeroResult()
        {
            var r = new BigInteger(0).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(0, r);

            r = new BigInteger(0).Divide(-10, MidpointRounding.ToEven);
            Assert.AreEqual(0, r);
        }

        [TestMethod]
        public void RoundAwayFromZero_ZeroResult()
        {
            var r = new BigInteger(0).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(0, r);

            r = new BigInteger(0).Divide(-10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(0, r);
        }

        [TestMethod]
        public void RoundToEven_NonMidPoint()
        {
            var r = new BigInteger(2).Divide(3, MidpointRounding.ToEven);
            Assert.AreEqual(1, r);

            r = new BigInteger(-2).Divide(3, MidpointRounding.ToEven);
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void RoundAwayFromZero_NonMidPoint()
        {
            var r = new BigInteger(2).Divide(3, MidpointRounding.AwayFromZero);
            Assert.AreEqual(1, r);

            r = new BigInteger(-2).Divide(3, MidpointRounding.AwayFromZero);
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void RoundToEven_MidPoint()
        {
            var r = new BigInteger(2).Divide(2, MidpointRounding.ToEven);
            Assert.AreEqual(1, r);

            r = new BigInteger(-2).Divide(2, MidpointRounding.ToEven);
            Assert.AreEqual(-1, r);

            r = new BigInteger(15).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(2, r);

            r = new BigInteger(25).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(2, r);

            r = new BigInteger(35).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(4, r);

            r = new BigInteger(-15).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-25).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-35).Divide(10, MidpointRounding.ToEven);
            Assert.AreEqual(-4, r);
        }

        [TestMethod]
        public void RoundAwayFromZero_MidPoint()
        {
            var r = new BigInteger(15).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(25).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(3, r);

            r = new BigInteger(35).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(4, r);

            r = new BigInteger(-15).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-25).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(-3, r);

            r = new BigInteger(-35).Divide(10, MidpointRounding.AwayFromZero);
            Assert.AreEqual(-4, r);
        }

        [TestMethod]
        public void RoundToZero()
        {
            var r = new BigInteger(29).Divide(10, MidpointRounding.ToZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-29).Divide(10, MidpointRounding.ToZero);
            Assert.AreEqual(-2, r);
        }

        [TestMethod]
        public void RoundToNegativeInfinity()
        {
            var r = new BigInteger(29).Divide(10, MidpointRounding.ToNegativeInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-21).Divide(10, MidpointRounding.ToNegativeInfinity);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void RoundToPositiveInfinity()
        {
            var r = new BigInteger(21).Divide(10, MidpointRounding.ToPositiveInfinity);
            Assert.AreEqual(3, r);

            r = new BigInteger(-29).Divide(10, MidpointRounding.ToPositiveInfinity);
            Assert.AreEqual(-2, r);
        }
    }
}
