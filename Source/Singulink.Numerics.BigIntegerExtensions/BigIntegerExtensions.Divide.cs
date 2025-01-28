using System;
using System.Numerics;

namespace Singulink.Numerics;

/// <content>
/// Enhanced division implementation for BigInteger.
/// </content>
public static partial class BigIntegerExtensions
{
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

        if (!remainder.IsZero)
        {
            switch (mode)
            {
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

                    if (compareResult > 0)
                    {
                        result = RoundAwayFromZero(result, sign);
                    }
                    else if (compareResult == 0)
                    {
                        switch (mode)
                        {
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