using System;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Singulink.Numerics;

/// <summary>
/// Provides useful extension methods for <see cref="BigInteger"/> values.
/// </summary>
public static partial class BigIntegerExtensions
{
    private static readonly BigInteger Ten = 10;
    private static readonly BigInteger MinusTen = -10;
    private static readonly BigInteger UInt64Max = ulong.MaxValue;
    private static readonly BigInteger MinusUInt64Max = -(BigInteger)ulong.MaxValue;
    private static readonly BigInteger Int64Min = long.MinValue;

#if NET
    private static readonly BigInteger UInt128Max = UInt128.MaxValue;
    private static readonly BigInteger MinusUInt128Max = -(BigInteger)UInt128.MaxValue;
#endif

    private static readonly FieldInfo? BitsField = GetBitsField();
    private static readonly BigIntegerPowCache Pow10 = BigIntegerPowCache.GetCache(10);

#if NET
    private static readonly double Log2 = double.Log10(2);
#endif

    /// <summary>
    /// Gets a value indicating whether optimizations that depend on expected BigInteger structure internals are enabled.
    /// </summary>
    /// <remarks>
    /// Optimizations for this version of the library are enabled as long as BigInteger's internal representation matches expectations that the optimizations
    /// depend on, which at the time of release of this version of the library is true for all supported runtimes up to and including .NET 9. A warning is
    /// emitted to the debug console and optimizations are disabled if changes to the BigInteger internals are detected as there are plans to eventually
    /// reimplement the type to utilize a 2's complement internal representation instead of the current 1's complement representation. A new version of the
    /// library will be released if/when that happens to accommodate the new implementation.
    /// </remarks>
    public static bool OptimizationsEnabled => BitsField != null;

    /// <summary>
    /// Calculates the number of base 10 digits in the value. Zero values return 1.
    /// </summary>
    public static int CountDigits(this BigInteger value)
    {
        if (value.IsZero || (value < Ten && value > MinusTen))
            return 1;

        return CountDigitsImpl(value);
    }

    /// <summary>
    /// Calculates the number of base 10 digits and trailing zeros in the value. Zero values return 1 digit and 0 trailing zeros.
    /// </summary>
    public static (int Digits, int TrailingZeros) CountDigitsAndTrailingZeros(this BigInteger value)
    {
        if (value.IsZero || (value < Ten && value > MinusTen))
            return (1, 0);

        if (!value.IsEven)
            return (CountDigitsImpl(value), 0);

        if (OptimizationsEnabled)
        {
            value = BigInteger.Abs(value);

            if (value <= UInt64Max)
                return SmallCount((ulong)value);

#if NET
            if (value <= UInt128Max)
                return MediumCount((UInt128)value);
#endif

            return LargeBitFieldCount(value);
        }

        // Optimizations are disabled so BigInteger internal implementation changed. Avoid Abs() when possible since it probably switched to 2's complement
        // meaning Abs() is slow and allocates.

        if (value.Sign > 0)
        {
            if (value <= UInt64Max)
                return SmallCount((ulong)value);

#if NET
            if (value <= UInt128Max)
                return MediumCount((UInt128)value);
#endif
        }
        else
        {
            if (value >= Int64Min)
                return SmallCount((ulong)(-(long)value));

            if (value >= MinusUInt64Max)
                return SmallCount((ulong)BigInteger.Abs(value));

#if NET
            if (value >= MinusUInt128Max)
                return MediumCount((UInt128)BigInteger.Abs(value));
#endif
        }

        return LargeStringCount(value);

        // Local functions:

        static (int Digits, int TrailingZeros) SmallCount(ulong mantissa)
        {
            Debug.Assert(mantissa != 0, "unexpected zero value");

            int trailingZeros = 0;
            int digits = 1;

            while (true)
            {
                ulong remainder;

#if NET
                (mantissa, remainder) = ulong.DivRem(mantissa, 10);
#else
                (mantissa, remainder) = DivRem(mantissa, 10);
#endif
                if (remainder != 0)
                    break;

                trailingZeros++;
                digits++;
            }

            while (mantissa != 0)
            {
                mantissa /= 10;
                digits++;
            }

            return (digits, trailingZeros);
        }

#if NET
        static (int Digits, int TrailingZeros) MediumCount(UInt128 mantissa)
        {
            Debug.Assert(mantissa != 0, "unexpected zero value");

            int trailingZeros = 0;
            int digits = 1;

            while (true)
            {
                UInt128 remainder;
                (mantissa, remainder) = UInt128.DivRem(mantissa, 10ul);

                if (remainder != 0)
                    break;

                trailingZeros++;
                digits++;
            }

            while (mantissa != 0)
            {
                mantissa /= 10;
                digits++;
            }

            return (digits, trailingZeros);
        }
#endif

        static (int Digits, int TrailingZeros) LargeBitFieldCount(BigInteger value)
        {
            Debug.Assert(BitsField is not null, "bits field optimizations not enabled");

            uint[] valueBits = Unsafe.As<uint[]>(BitsField.GetValueDirect(__makeref(value)));

            Debug.Assert(valueBits is not null, "value too small to have bits array set");

            // First convert to base 10^9.
            const uint kuBase = 1000000000; // 10^9
            const int kcchBase = 9;

            int cuSrc = valueBits.Length;
            int cuMax = checked((cuSrc * 10 / 9) + 2);

            uint[] rentedArray = null;
            Span<uint> rguDst = cuMax <= 128 ? stackalloc uint[128] : (rentedArray = ArrayPool<uint>.Shared.Rent(cuMax));

            int cuDst = 0;

            for (int iuSrc = cuSrc; --iuSrc >= 0;)
            {
                uint uCarry = valueBits[iuSrc];

                for (int iuDst = 0; iuDst < cuDst; iuDst++)
                {
                    Debug.Assert(rguDst[iuDst] < kuBase, "base 10^9 overflow");
                    ulong uuRes = MakeUInt64(rguDst[iuDst], uCarry);
                    rguDst[iuDst] = (uint)(uuRes % kuBase);
                    uCarry = (uint)(uuRes / kuBase);
                }

                if (uCarry != 0)
                {
                    rguDst[cuDst++] = uCarry % kuBase;
                    uCarry /= kuBase;
                    if (uCarry != 0)
                        rguDst[cuDst++] = uCarry;
                }
            }

            cuDst--;

            int digits = cuDst * kcchBase;

            int trailingZeros = 0;
            bool hitNonZero = false;

            for (int iuDst = 0; iuDst < cuDst; iuDst++)
            {
                uint uDig = rguDst[iuDst];
                Debug.Assert(uDig < kuBase, "base 10^9 overflow");

                for (int cch = kcchBase; --cch >= 0;)
                {
                    if (uDig % 10 == 0)
                    {
                        trailingZeros++;
                    }
                    else
                    {
                        hitNonZero = true;
                        uDig /= 10;
                        goto HitNonZero;
                    }

                    uDig /= 10;
                }
            }

        HitNonZero:

            for (uint uDig = rguDst[cuDst]; uDig != 0;)
            {
                digits++;

                if (!hitNonZero)
                {
                    if (uDig % 10 == 0)
                        trailingZeros++;
                    else
                        hitNonZero = true;
                }

                uDig /= 10;
            }

            if (rentedArray != null)
                ArrayPool<uint>.Shared.Return(rentedArray);

            return (digits, trailingZeros);
        }

        static (int Digits, int TrailingZeros) LargeStringCount(BigInteger value)
        {
            Debug.Assert(value != 0, "unexpected zero value");

            string s = value.ToString(CultureInfo.InvariantCulture);
            int trailingZeros = 0;

            for (int i = s.Length - 1; s[i] == '0'; i--)
                trailingZeros++;

            return (value.Sign < 0 ? s.Length - 1 : s.Length, trailingZeros);
        }
    }

    private static int CountDigitsImpl(BigInteger value)
    {
        value = BigInteger.Abs(value);

#if NET
        // Improves CountDigits() perf by ~2.5x on .NET.

        // NOTE: this works without needing the Abs() call above, which will be useful to avoid Abs() when BigInteger moves to 2's complement representation,
        // but GetBitLength() is faster for non-negative values in the current 1's complement representation so we keep the Abs() call.

        int base10Digits = (int)(value.GetBitLength() * Log2);
#else
        int base10Digits = (int)Math.Ceiling(BigInteger.Log10(value));
#endif
        var reference = Pow10.Get(base10Digits);

        if (value >= reference)
            base10Digits++;

        return base10Digits;
    }

#if NETSTANDARD

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right)
    {
        ulong quotient = left / right;
        return (quotient, left - (quotient * right));
    }

#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong MakeUInt64(uint uHi, uint uLo)
    {
        const int KcbitUint = 32;
        return ((ulong)uHi << KcbitUint) | uLo;
    }

    private static FieldInfo? GetBitsField()
    {
#if DEBUG
        // Disable optimizations in debug mode so that those paths are also tested.
        Debug.WriteLine("[BigIntegerExtensions] WARNING: Optimizations disabled for debug build.");
        return null;
#else
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var fields = typeof(BigInteger).GetFields(bindingFlags).ToArray();

        var signField = fields.FirstOrDefault(f => f.FieldType == typeof(int));
        var bitsField = fields.FirstOrDefault(f => f.FieldType == typeof(uint[]));

        if (fields.Length is not 2 || bitsField is null || signField is null)
        {
            Trace.TraceWarning("[BigIntegerExtensions] Optimizations disabled - unexpected BigInteger internal field layout.");
            return null;
        }

        // Test a sample negative BigInteger value to ensure the field contains the expected data.

        var testValue = new BigInteger(int.MinValue) * 2;

        int testSign = (int)signField.GetValueDirect(__makeref(testValue))!;
        uint[] testBits = (uint[])bitsField.GetValueDirect(__makeref(testValue));

        if (testSign is -1 && testBits?.Length is 2 && testBits[0] is 0 && testBits[1] is 1)
            return bitsField;

        Trace.TraceWarning("[BigIntegerExtensions] Optimizations disabled - unexpected BigInteger internal bits representation.");
        return null;
#endif
    }
}