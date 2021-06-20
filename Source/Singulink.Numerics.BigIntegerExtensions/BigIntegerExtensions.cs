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

#if !NETSTANDARD
        private static readonly double Log2 = Math.Log10(2);
#endif

        /// <summary>
        /// Calculates the total number of base 10 digits in the value.
        /// </summary>
        public static int CountDigits(this BigInteger value)
        {
            if (value.IsZero)
                return 1;

            value = BigInteger.Abs(value);

            if (value.IsOne)
                return 1;

#if !NETSTANDARD
            int base10Digits = (int)(value.GetBitLength() * Log2); // improves CountDigits() perf by ~2.5x on NET5+
#else
            int base10Digits = (int)Math.Ceiling(BigInteger.Log10(value));
#endif
            var reference = BigIntegerPow10.Get(base10Digits);

            if (value >= reference)
                base10Digits++;

            return base10Digits;
        }

        /// <summary>
        /// Divides two <see cref="BigInteger"/> values and uses the specified rounding mode for any fractional component of the result.
        /// </summary>
        public static BigInteger Divide(this BigInteger dividend, BigInteger divisor, RoundingMode mode = RoundingMode.MidpointToEven)
        {
            if ((uint)mode > 8)
                throw new ArgumentException($"Unsupported rounding mode '{mode}'.", nameof(mode));

            if (mode == RoundingMode.ToZero)
                return dividend / divisor;

            var result = BigInteger.DivRem(dividend, divisor, out var remainder);
            int sign = dividend.Sign * divisor.Sign;

            if (!remainder.IsZero) {
                switch (mode) {
                    case RoundingMode.ToNegativeInfinity:
                        if (sign < 0)
                            result -= BigInteger.One;

                        break;

                    case RoundingMode.ToPositiveInfinity:
                        if (sign > 0)
                            result += BigInteger.One;

                        break;

                    case RoundingMode.AwayFromZero:
                        result += sign > 0 ? BigInteger.One : BigInteger.MinusOne;
                        break;

                    default:
                        int compareResult = (BigInteger.Abs(remainder) << 1).CompareTo(BigInteger.Abs(divisor));

                        if (compareResult > 0) {
                            result = RoundAwayFromZero(result, sign);
                        }
                        else if (compareResult == 0) {
                            switch (mode) {
                                case RoundingMode.MidpointAwayFromZero:
                                    result = RoundAwayFromZero(result, sign);
                                    break;

                                case RoundingMode.MidpointToEven:
                                    if (!result.IsEven)
                                        result = RoundAwayFromZero(result, sign);

                                    break;
                                case RoundingMode.MidpointToNegativeInfinity:
                                    if (sign < 0)
                                        result -= BigInteger.One;

                                    break;

                                case RoundingMode.MidpointToPositiveInfinity:
                                    if (sign > 0)
                                        result += BigInteger.One;

                                    break;
                            }
                        }

                        break;
                }
            }

            return result;

            static BigInteger RoundAwayFromZero(BigInteger value, int sign) => sign > 0 ? value + BigInteger.One : value - BigInteger.One;
        }
    }
}
