using System;
using System.Collections.Generic;
using System.Text;

namespace Singulink.Numerics
{
    /// <summary>
    /// Specifies how mathematical rounding should process a number.
    /// </summary>
    public enum RoundingMode
    {
        // Existing modes which map directly to System.MidpointRounding:

        /// <summary>
        /// Round to nearest mode: when a number is halfway between two others, it is rounded toward the nearest even number.
        /// </summary>
        MidpointToEven,

        /// <summary>
        /// Round to nearest mode: when a number is halfway between two others, it is rounded toward the nearest number that is away from zero.
        /// </summary>
        MidpointAwayFromZero,

        /// <summary>
        /// Directed mode: the number is rounded toward zero, with the result closest to and no greater in magnitude than the infinitely precise result.
        /// </summary>
        ToZero,

        /// <summary>
        /// Directed mode: the number is rounded down, with the result closest to and no greater than the infinitely precise result.
        /// </summary>
        ToNegativeInfinity,

        /// <summary>
        /// Directed mode: the number is rounded up, with the result closest to and no less than the infinitely precise result.
        /// </summary>
        ToPositiveInfinity,

        // New modes:

        /// <summary>
        /// Directed mode: the number is rounded toward the nearest number that is away from zero.
        /// </summary>
        AwayFromZero,

        /// <summary>
        /// Round to nearest mode: when a number is halfway between two others, it is rounded toward zero, with the result closest to and no greater in
        /// magnitude than the infinitely precise result.
        /// </summary>
        MidpointToZero,

        /// <summary>
        /// Round to nearest mode: when a number is halfway between two others, it is rounded down, with the result closest to and no greater than the
        /// infinitely precise result.
        /// </summary>
        MidpointToNegativeInfinity,

        /// <summary>
        /// Round to nearest mode: when a number is halfway between two others, the number is rounded up, with the result closest to and no less than the
        /// infinitely precise result.
        /// </summary>
        MidpointToPositiveInfinity,
    }
}