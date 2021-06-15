using System;
using System.Collections.Generic;
using System.Text;

namespace Singulink.Numerics
{
    /// <summary>
    /// Extension methods for <see cref="MidpointRounding"/> enumeration values.
    /// </summary>
    public static class MidpointRoundingExtensions
    {
        /// <summary>
        /// Converts a <see cref="MidpointRounding"/> enumeration into a <see cref="RoundingMode"/> enumeration.
        /// </summary>
        public static RoundingMode ToRoundingMode(this MidpointRounding mode)
        {
            if ((uint)mode > 4)
                throw new ArgumentException($"Unsupported rounding mode '{mode}'.", nameof(mode));

            return (RoundingMode)mode;
        }
    }
}
