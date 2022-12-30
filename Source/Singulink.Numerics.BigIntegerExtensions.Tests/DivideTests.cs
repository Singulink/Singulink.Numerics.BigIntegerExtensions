extern alias BigIntegerExtensionsAssembly;

using System;
using System.Numerics;
using BigIntegerExtensionsAssembly::Singulink.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace Singulink.Numerics.BigIntegerExtensions.Tests
{
    [TestClass]
    public class DivideTests
    {
        [TestMethod]

        public void MidpointToEven_ZeroResult()
        {
            var r = new BigInteger(0).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(0, r);

            r = new BigInteger(0).Divide(-10, RoundingMode.MidpointToEven);
            Assert.AreEqual(0, r);
        }

        [TestMethod]
        public void MidpointToEven_NonMidpoint()
        {
            var r = new BigInteger(2).Divide(3, RoundingMode.MidpointToEven);
            Assert.AreEqual(1, r);

            r = new BigInteger(-2).Divide(3, RoundingMode.MidpointToEven);
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void MidpointToEven_Midpoint()
        {
            var r = new BigInteger(2).Divide(2, RoundingMode.MidpointToEven);
            Assert.AreEqual(1, r);

            r = new BigInteger(-2).Divide(2, RoundingMode.MidpointToEven);
            Assert.AreEqual(-1, r);

            r = new BigInteger(15).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(2, r);

            r = new BigInteger(25).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(2, r);

            r = new BigInteger(35).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(4, r);

            r = new BigInteger(-15).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-25).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-35).Divide(10, RoundingMode.MidpointToEven);
            Assert.AreEqual(-4, r);
        }

        [TestMethod]
        public void MidpointAwayFromZero_ZeroResult()
        {
            var r = new BigInteger(0).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(0, r);

            r = new BigInteger(0).Divide(-10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(0, r);
        }

        [TestMethod]
        public void MidpointAwayFromZero_NonMidpoint()
        {
            var r = new BigInteger(2).Divide(3, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(1, r);

            r = new BigInteger(-2).Divide(3, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void MidpointAwayFromZero_Midpoint()
        {
            var r = new BigInteger(15).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(25).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(3, r);

            r = new BigInteger(35).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(4, r);

            r = new BigInteger(-15).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-25).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(-3, r);

            r = new BigInteger(-35).Divide(10, RoundingMode.MidpointAwayFromZero);
            Assert.AreEqual(-4, r);
        }

        [TestMethod]
        public void ToZero()
        {
            var r = new BigInteger(29).Divide(10, RoundingMode.ToZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-29).Divide(10, RoundingMode.ToZero);
            Assert.AreEqual(-2, r);

            r = new BigInteger(20).Divide(10, RoundingMode.ToZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-30).Divide(10, RoundingMode.ToZero);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void ToNegativeInfinity()
        {
            var r = new BigInteger(29).Divide(10, RoundingMode.ToNegativeInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-21).Divide(10, RoundingMode.ToNegativeInfinity);
            Assert.AreEqual(-3, r);

            r = new BigInteger(20).Divide(10, RoundingMode.ToNegativeInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-30).Divide(10, RoundingMode.ToNegativeInfinity);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void ToPositiveInfinity()
        {
            var r = new BigInteger(21).Divide(10, RoundingMode.ToPositiveInfinity);
            Assert.AreEqual(3, r);

            r = new BigInteger(-29).Divide(10, RoundingMode.ToPositiveInfinity);
            Assert.AreEqual(-2, r);

            r = new BigInteger(20).Divide(10, RoundingMode.ToPositiveInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-30).Divide(10, RoundingMode.ToPositiveInfinity);
            Assert.AreEqual(-3, r);
        }

        // New modes:

        [TestMethod]
        public void MidpointToZero_Zero()
        {
            var r = new BigInteger(20).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-30).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void MidpointToZero_NonMidpoint()
        {
            var r = new BigInteger(29).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(3, r);

            r = new BigInteger(22).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-21).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-27).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void MidpointToZero_Midpoint()
        {
            var r = new BigInteger(25).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-25).Divide(10, RoundingMode.MidpointToZero);
            Assert.AreEqual(-2, r);
        }

        [TestMethod]
        public void MidpointToNegativeInfinity_Zero()
        {
            var r = new BigInteger(20).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-30).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void MidpointToNegativeInfinity_NonMidpoint()
        {
            var r = new BigInteger(29).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(3, r);

            r = new BigInteger(22).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-21).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-27).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void MidpointToNegativeInfinity_Midpoint()
        {
            var r = new BigInteger(25).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-25).Divide(10, RoundingMode.MidpointToNegativeInfinity);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void MidpointToPositiveInfinity_Midpoint()
        {
            var r = new BigInteger(25).Divide(10, RoundingMode.MidpointToPositiveInfinity);
            Assert.AreEqual(3, r);

            r = new BigInteger(-25).Divide(10, RoundingMode.MidpointToPositiveInfinity);
            Assert.AreEqual(-2, r);
        }

        [TestMethod]
        public void MidpointToPositiveInfinity_NonMidpoint()
        {
            var r = new BigInteger(29).Divide(10, RoundingMode.MidpointToPositiveInfinity);
            Assert.AreEqual(3, r);

            r = new BigInteger(22).Divide(10, RoundingMode.MidpointToPositiveInfinity);
            Assert.AreEqual(2, r);

            r = new BigInteger(-21).Divide(10, RoundingMode.MidpointToPositiveInfinity);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-27).Divide(10, RoundingMode.MidpointToPositiveInfinity);
            Assert.AreEqual(-3, r);
        }

        [TestMethod]
        public void AwayFromZero()
        {
            var r = new BigInteger(21).Divide(10, RoundingMode.AwayFromZero);
            Assert.AreEqual(3, r);

            r = new BigInteger(-21).Divide(10, RoundingMode.AwayFromZero);
            Assert.AreEqual(-3, r);

            r = new BigInteger(20).Divide(10, RoundingMode.AwayFromZero);
            Assert.AreEqual(2, r);

            r = new BigInteger(-30).Divide(10, RoundingMode.AwayFromZero);
            Assert.AreEqual(-3, r);
        }
    }
}