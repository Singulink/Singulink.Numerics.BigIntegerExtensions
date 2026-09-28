extern alias BigIntegerExtensionsAssembly;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using BigIntegerExtensionsAssembly::Singulink.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Singulink.Numerics.BigIntegerExtensions.Tests;

/// <summary>
/// Validates the digit counting methods against a string-based reference implementation for a wide range of values that cover all the internal size
/// thresholds and word boundaries.
/// </summary>
[TestClass]
public class CountReferenceTests
{
    [TestMethod]
    public void MatchesStringReference()
    {
        foreach (var value in GetValues())
        {
            string s = BigInteger.Abs(value).ToString(CultureInfo.InvariantCulture);
            int expectedDigits = s.Length;
            int expectedTrailingZeros = value.IsZero ? 0 : s.Length - s.TrimEnd('0').Length;

            value.CountDigits().ShouldBe(expectedDigits, $"value: {value}");
            value.CountDigitsAndTrailingZeros().ShouldBe((expectedDigits, expectedTrailingZeros), $"value: {value}");
        }
    }

    private static IEnumerable<BigInteger> GetValues()
    {
        yield return BigInteger.Zero;

        for (int k = 0; k <= 700; k++)
        {
            var pow2 = BigInteger.Pow(2, k);
            var pow10 = BigInteger.Pow(10, k);

            foreach (var value in new[] { pow2, pow2 - 1, pow2 + 1, pow2 * 10, pow2 * 1000, pow10, pow10 - 1, pow10 + 1, pow10 * 3, pow10 * 12345, pow2 * pow10 })
            {
                yield return value;
                yield return -value;
            }
        }
    }
}