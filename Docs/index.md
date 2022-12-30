<div class="article">

# Singulink.Numerics.BigIntegerExtensions

## Overview

**BigIntegerExtensions** provides some helper `BigInteger` extension methods and a super fast power cache that serves as a replacement for `BigInteger.Pow()` for bases between 3 and 10 (inclusive) to quickly get cached values instead of calculating them each time they are needed. This library is used by [Singulink.Numerics.BigDecimal](https://www.singulink.com/Docs/Singulink.Numerics.BigDecimal/) to improve performance and support its operations.

**Singulink.Numerics.BigIntegerExtensions** is part of the **Singulink Libraries** collection. Visit https://github.com/Singulink/ to see the full list of libraries available.

## Features

- `BigIntegerPowCache` for cached lookups of powers.
- `CountDigits()` and `CountDigitsAndTrailingZeros()` extension methods using highly optimized algorithms.
- `Divide()` extension method that supports a wide range of rounding modes instead of just truncating the result like the standard division operator.

## Information and Links

Here are some additonal links to get you started:

- [API Documentation](api/index.md) - Browse the fully documented API here.
- [Chat on Discord](https://discord.gg/EkQhJFsBu6) - Have questions or want to discuss the library? This is the place for all Singulink project discussions.
- [Github Repo](https://github.com/Singulink/Singulink.Numerics.BigIntegerExtensions) - File issues, contribute pull requests or check out the code for yourself!

</div>