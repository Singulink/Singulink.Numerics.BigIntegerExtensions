using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Singulink.Numerics
{
    /// <summary>
    /// Provides caching of <see cref="BigInteger"/> powers for a given basis (from 3 to 10). Exponents that are bigger than the size of the cache are
    /// calculated each time they are requested.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Caching powers of 2 is not supported - use left/right shifting for a more memory and CPU efficient means of multiplying/dividing values by powers of
    /// 2.</para>
    /// <para>
    /// You can safely get a cache instance without risking using large amounts of unnecessary memory in the event that the cache is not needed since it is
    /// empty when it is first created. The cache is populated on the first request for a value. Only 1 instance of a cache exists for a given basis.</para>
    /// <para>
    /// When an exponent is requested that is larger than the current cache size, the cache is expanded by a minimum of 64 entries or as needed to ensure it
    /// includes the requested exponent. This involves creating a new array to hold the expanded cache, copying previously cached values over, and calculating
    /// all the rest of the missing values. Since caching results of an exponential operation is already inherently exponential, a linear approach is used to
    /// expand the size of the cache instead of doubling its size each time.</para>
    /// </remarks>
    public sealed class BigIntegerPowCache
    {
        private const int MinGrowthAmount = 64;

        #region Static Members

        private static readonly BigIntegerPowCache?[] _cacheLookup = new BigIntegerPowCache[7];

        /// <summary>
        /// If there is an existing cache for the given basis then get it and increase its maximum size to meet the specified required max size, otherwise
        /// create a new cache with the given parameters.
        /// </summary>
        /// <param name="basis">A value between 3 and 10 that specifies the basis of the exponent.</param>
        /// <param name="requiredMaxSize">The required maximum size of the cache. The maximum cached exponent is one less than this size. Caches never have a
        /// smaller max size than 1024.</param>
        /// <returns>A cache for the given basis that meets or exceeds the specified parameters.</returns>
        public static BigIntegerPowCache GetCache(int basis, int requiredMaxSize = 1024)
        {
            if (basis is < 3 or > 10)
                throw new ArgumentOutOfRangeException(nameof(basis));

            int cacheIndex = basis - 3;

            lock (_cacheLookup) {
                var cache = _cacheLookup[cacheIndex];

                if (cache == null)
                    _cacheLookup[cacheIndex] = cache = new BigIntegerPowCache(basis);

                cache.RequireMaxSize(requiredMaxSize);
                return cache;
            }
        }

        #endregion

        private BigInteger[] _cache;
        private readonly object _syncRoot = new object();

        /// <summary>
        /// Gets the base of the exponent for this cache.
        /// </summary>
        public BigInteger Basis { get; }

        /// <summary>
        /// Gets the maximum cache size. The largest cached exponent is one less than this value.
        /// </summary>
        public int MaxSize { get; private set; }

        private BigIntegerPowCache(int basis)
        {
            Basis = new BigInteger(basis);
            MaxSize = 1024;

            _cache = Array.Empty<BigInteger>();
        }

        /// <summary>
        /// Gets the cache's basis raised to the power of the given exponent value.
        /// </summary>
        public BigInteger Get(int exponent)
        {
            return exponent < _cache.Length ? _cache[exponent] : GetUncached(exponent);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private BigInteger GetUncached(int exponent)
        {
            if (exponent < MaxSize) {
                GrowTo(exponent);
                return _cache[exponent];
            }

            return BigInteger.Pow(Basis, exponent);
        }

        private void GrowTo(int exponent)
        {
            lock (_syncRoot) {
                if (exponent < _cache.Length)
                    return;

                // Expand to at least exponent + 10 to prevent another fast resize if the exponent is near the upper limit of MinGrowthAmount

                int expandedSize = Math.Max(exponent + 10, _cache.Length + MinGrowthAmount);
                expandedSize = Math.Min(expandedSize, MaxSize);
                Debug.Assert(expandedSize > _cache.Length, "cache did not grow");

                var newCache = new BigInteger[expandedSize];
                Array.Copy(_cache, newCache, _cache.Length);

                for (int i = _cache.Length; i < newCache.Length; i++)
                    newCache[i] = BigInteger.Pow(Basis, i);

                _cache = newCache;
            }
        }

        private void RequireMaxSize(int size)
        {
            lock (_syncRoot) {
                MaxSize = Math.Max(size, MaxSize);
            }
        }
    }
}