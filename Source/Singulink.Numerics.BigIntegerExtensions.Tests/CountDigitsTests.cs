using System;
using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singulink.Numerics.BigIntegerExtensions.Tests
{
    [TestClass]
    public class CountDigitsTests
    {
        [TestMethod]
        public void Count()
        {
            var value = new BigInteger(9999_9999);
            Assert.AreEqual(8, value.CountDigits());

            value = new BigInteger(1_0000_0000);
            Assert.AreEqual(9, value.CountDigits());

            value = new BigInteger(1_0000_0001);
            Assert.AreEqual(9, value.CountDigits());
        }

        [TestMethod]
        public void CountBig()
        {
            var value = BigInteger.Parse(new string('9', 100_000), CultureInfo.InvariantCulture);
            Assert.AreEqual(100_000, value.CountDigits());

            value = BigInteger.Parse("1" + new string('0', 100_000), CultureInfo.InvariantCulture);
            Assert.AreEqual(100_001, value.CountDigits());
        }
    }
}
