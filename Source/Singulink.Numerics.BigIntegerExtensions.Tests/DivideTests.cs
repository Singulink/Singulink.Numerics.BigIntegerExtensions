using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singulink.Numerics.BigIntegerExtensions.Tests
{
    [TestClass]
    public class DivideTests
    {
        [TestMethod]
        public void RoundToEven()
        {
            var r = new BigInteger(15).Divide(10);
            Assert.AreEqual(2, r);

            r = new BigInteger(25).Divide(10);
            Assert.AreEqual(2, r);

            r = new BigInteger(35).Divide(10);
            Assert.AreEqual(4, r);

            r = new BigInteger(-15).Divide(10);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-25).Divide(10);
            Assert.AreEqual(-2, r);

            r = new BigInteger(-35).Divide(10);
            Assert.AreEqual(-4, r);
        }

        [TestMethod]
        public void RoundAwayFromZero()
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
    }
}
