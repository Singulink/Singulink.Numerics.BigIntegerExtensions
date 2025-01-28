extern alias BigIntegerExtensionsAssembly;

using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using BigIntegerExtensionsAssembly::Singulink.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singulink.Numerics.BigIntegerExtensions.Tests;

[TestClass]
public class CacheTests
{
    private readonly FieldInfo _bitsField = typeof(BigInteger).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).First(f => f.FieldType.IsArray);

    [TestMethod]
    public void CacheBasisRange()
    {
        for (int i = 3; i <= 10; i++)
        {
            var cache = BigIntegerPowCache.GetCache(i);
            Assert.AreSame(cache, BigIntegerPowCache.GetCache(i));
        }

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigIntegerPowCache.GetCache(2));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigIntegerPowCache.GetCache(11));
    }

    [TestMethod]
    public void CachedValues()
    {
        var cache = BigIntegerPowCache.GetCache(3);
        var value1 = cache.Get(1023);
        var value2 = cache.Get(1023);

        Assert.AreEqual(BigInteger.Pow(3, 1023), value1);
        Assert.AreSame(_bitsField.GetValue(value1), _bitsField.GetValue(value2));

        value1 = cache.Get(1024);
        value2 = cache.Get(1024);

        Assert.AreEqual(BigInteger.Pow(3, 1024), value1);
        Assert.AreNotSame(_bitsField.GetValue(value1), _bitsField.GetValue(value2));

        BigIntegerPowCache.GetCache(3, 2000);
        value1 = cache.Get(1024);
        value2 = cache.Get(1024);

        Assert.AreEqual(BigInteger.Pow(3, 1024), value1);
        Assert.AreSame(_bitsField.GetValue(value1), _bitsField.GetValue(value2));

        value1 = cache.Get(1999);
        value2 = cache.Get(1999);

        Assert.AreEqual(BigInteger.Pow(3, 1999), value1);
        Assert.AreSame(_bitsField.GetValue(value1), _bitsField.GetValue(value2));

        value1 = cache.Get(2000);
        value2 = cache.Get(2000);

        Assert.AreEqual(BigInteger.Pow(3, 2000), value1);
        Assert.AreNotSame(_bitsField.GetValue(value1), _bitsField.GetValue(value2));
    }
}