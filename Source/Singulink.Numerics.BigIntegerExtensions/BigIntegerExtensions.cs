using System;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Singulink.Numerics
{
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

        private static readonly FieldInfo? BitsField = GetBitsField();
        private static readonly BigIntegerPowCache Pow10 = BigIntegerPowCache.GetCache(10);

#if NET5_0_OR_GREATER
        private static readonly double Log2 = Math.Log10(2);
#endif

        /// <summary>
        /// Gets a value indicating whether optimizations that depend on internal BigInteger structure are enabled.
        /// </summary>
        /// <remarks>
        /// Optimizations for this version of the library are enabled as long as the .NET runtime version is less than or equal to .NET 7.x and the BigInteger
        /// field structure matches what is expected for the current runtime. A warning is emitted to the debug console and optimizations are disabled if this
        /// library runs on newer runtimes to prevent incorrect behavior if the implementation of BigInteger changes. A new version of the library will be
        /// released with each major .NET release to safely enable optimizations on newer runtimes.
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

                return LargeBitFieldCount(value);
            }

            // Optimizations are disabled so BigInteger internal implementation changed - avoid Abs() if it switched to slow allocating 2's complement.

            switch (value.Sign)
            {
                case > 0 when value <= UInt64Max:
                    return SmallCount((ulong)value);

                case < 0:
                    if (value >= Int64Min)
                        return SmallCount((ulong)(-(long)value));

                    if (value >= MinusUInt64Max)
                        return SmallCount((ulong)BigInteger.Abs(value));

                    break;
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

#if NET7_0_OR_GREATER
                    (mantissa, remainder) = Math.DivRem(mantissa, 10);
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

            static (int Digits, int TrailingZeros) LargeBitFieldCount(BigInteger value)
            {
                const int MaxStackAllocSize = 128;

                Debug.Assert(BitsField != null, "bits field optimizations not enabled");

                uint[] valueBits = (uint[])BitsField!.GetValueDirect(__makeref(value));

                Debug.Assert(valueBits != null, "value too small to have bits array set");

                // First convert to base 10^9.
                const uint kuBase = 1000000000; // 10^9
                const int kcchBase = 9;

                int cuSrc = valueBits!.Length;
                int cuMax = checked((cuSrc * 10 / 9) + 2);

                uint[] rentedArray = null;
                Span<uint> rguDst = cuMax <= MaxStackAllocSize ? stackalloc uint[MaxStackAllocSize] : (rentedArray = ArrayPool<uint>.Shared.Rent(cuMax));

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
                        {
                            trailingZeros++;
                        }
                        else
                        {
                            hitNonZero = true;
                        }
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

#if NET5_0_OR_GREATER
            // Improves CountDigits() perf by ~2.5x on NET5+
            // NOTE: this works without needing the Abs() call above, which will be useful to avoid Abs() when BigInteger moves to 2's complement respresentation.
            int base10Digits = (int)(value.GetBitLength() * Log2);
#else
            int base10Digits = (int)Math.Ceiling(BigInteger.Log10(value));
#endif
            var reference = Pow10.Get(base10Digits);

            if (value >= reference)
                base10Digits++;

            return base10Digits;
        }

#if !NET7_0_OR_GREATER

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
            if (Environment.Version.Major > 7)
            {
                Debug.WriteLine($"[BigIntegerExtensions] WARNING: Runtime dependent optimizations have been disabled for safety because detected .NET runtime version is greater than 7.x. Update the 'Singulink.Numerics.BigIntegerExtensions' package to enable optimizations on newer runtimes.");
                return null;
            }

            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            var fields = typeof(BigInteger)
                .GetFields(bindingFlags)
                .OrderBy(f => f.FieldType.Name)
                .ToArray();

            if (fields.Length != 2 || fields[0].FieldType != typeof(int) || fields[1].FieldType != typeof(uint[]))
            {
                Debug.WriteLine("[BigIntegerExtensions] WARNING: Optimizations disabled - unexpected BigInteger internal field layout.");
                return null;
            }

            return fields[1];
#endif
        }
    }
}