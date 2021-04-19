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
            if (value.IsZero)
                return 1;

            value = BigInteger.Abs(value);

            if (value.IsOne)
                return 1;

            int exp = (int)Math.Ceiling(BigInteger.Log10(value));
            var test = BigIntegerPow10.Get(exp);

            if (value >= test)
                exp++;

            return exp;
        }

        /// <summary>
        /// Divides two <see cref="BigInteger"/> values and applies the specified rounding to any fractional component of the result.
        /// </summary>
        public static BigInteger Divide(this BigInteger dividend, BigInteger divisor, MidpointRounding mode = MidpointRounding.ToEven)
        {
            if ((uint)mode > 4)
                throw new ArgumentException($"Unsupported rounding mode '{mode}'.", nameof(mode));

            if (mode == (MidpointRounding)2) // ToZero
                return dividend / divisor;

            var result = BigInteger.DivRem(dividend, divisor, out var remainder);

            if (!remainder.IsZero) {
                switch (mode) {
                    case (MidpointRounding)3: // ToNegativeInfinity
                        if (dividend.Sign * divisor.Sign < 0)
                            result -= BigInteger.One;

                        break;

                    case (MidpointRounding)4: // ToPositiveInfinity
                        if (dividend.Sign * divisor.Sign > 0)
                            result += BigInteger.One;

                        break;

                    default:
                        int compareResult = (BigInteger.Abs(remainder) << 1).CompareTo(BigInteger.Abs(divisor));

                        if (compareResult > 0) {
                            result = RoundAwayFromZero(result, dividend.Sign * divisor.Sign);
                        }
                        else if (compareResult == 0) {
                            switch (mode) {
                                case MidpointRounding.AwayFromZero:
                                    result = RoundAwayFromZero(result, dividend.Sign * divisor.Sign);
                                    break;

                                case MidpointRounding.ToEven:
                                    if (!result.IsEven)
                                        result = RoundAwayFromZero(result, dividend.Sign * divisor.Sign);

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
