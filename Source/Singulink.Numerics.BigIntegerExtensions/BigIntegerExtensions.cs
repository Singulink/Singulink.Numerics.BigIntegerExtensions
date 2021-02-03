using System;
using System.Numerics;

namespace Singulink.Numerics
{
    /// <summary>
    /// Provides extension methods and helpers for <see cref="BigInteger"/> values.
    /// </summary>
    public static class BigIntegerExtensions
    {
        private static readonly BigIntegerPowCache BigIntegerPow10 = BigIntegerPowCache.GetCache(10);

        /// <summary>
        /// Calculates the total number of base 10 digits in the value.
        /// </summary>
        public static int CountDigits(this BigInteger value)
        {
            value = BigInteger.Abs(value);

            if (value.IsZero || value.IsOne)
                return 1;

            int exp = (int)Math.Ceiling(BigInteger.Log10(value));
            var test = BigIntegerPow10.Get(exp);

            return value >= test ? exp + 1 : exp;
        }

        /// <summary>
        /// Divides two <see cref="BigInteger"/> values and applies the specified rounding to any fractional component of the result.
        /// </summary>
        public static BigInteger Divide(this BigInteger dividend, BigInteger divisor, MidpointRounding mode = MidpointRounding.ToEven)
        {
            var result = BigInteger.DivRem(dividend, divisor, out var remainder);

            if (!remainder.IsZero) {
                int compareResult = (BigInteger.Abs(remainder) << 1).CompareTo(BigInteger.Abs(divisor));
                int sign = dividend.Sign * divisor.Sign;

                if (compareResult > 0) {
                    return RoundAwayFromZero(result, sign);
                }
                else if (compareResult == 0) {
                    switch (mode) {
                        case MidpointRounding.AwayFromZero:
                            return RoundAwayFromZero(result, sign);
                        case MidpointRounding.ToEven:
                            if (!result.IsEven)
                                return RoundAwayFromZero(result, sign);

                            return result;
                        default:
                            throw new ArgumentException($"Unsupported rounding mode '{mode}'.", nameof(mode));
                    }
                }
            }

            return result;

            static BigInteger RoundAwayFromZero(BigInteger value, int sign) => sign > 0 ? value + BigInteger.One : value - BigInteger.One;
        }
    }
}
