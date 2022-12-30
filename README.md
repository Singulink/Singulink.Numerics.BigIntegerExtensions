# Singulink.Numerics.BigIntegerExtensions

[![Chat on Discord](https://img.shields.io/discord/906246067773923490)](https://discord.gg/EkQhJFsBu6)
[![View nuget packages](https://img.shields.io/nuget/v/Singulink.Numerics.BigIntegerExtensions.svg)](https://www.nuget.org/packages/Singulink.Numerics.BigIntegerExtensions/)
[![Build and Test](https://github.com/Singulink/Singulink.Numerics.BigIntegerExtensions/workflows/build%20and%20test/badge.svg)](https://github.com/Singulink/Singulink.Numerics.BigIntegerExtensions/actions?query=workflow%3A%22build+and+test%22)

**BigIntegerExtensions** provides some helper `BigInteger` extension methods and a super fast power cache that serves as a replacement for `BigInteger.Pow()` for bases between 3 and 10 (inclusive) to quickly get cached values instead of calculating them each time they are needed. This library is used by [Singulink.Numerics.BigDecimal](https://github.com/Singulink/Singulink.Numerics.BigDecimal/) to improve performance and support its operations.

### About Singulink

We are a small team of engineers and designers dedicated to building beautiful, functional and well-engineered software solutions. We offer very competitive rates as well as fixed-price contracts and welcome inquiries to discuss any custom development / project support needs you may have.

This package is part of our **Singulink Libraries** collection. Visit https://github.com/Singulink to see our full list of publicly available libraries and other open-source projects.

## Installation

The package is available on NuGet - simply install the `Singulink.Numerics.BigIntegerExtensions` package.

**Supported Runtimes**: Anywhere .NET Standard 2.0+ is supported, including:
- .NET Core 2.0+
- .NET Framework 4.6.1+
- Mono 5.4+
- Xamarin.iOS 10.14+
- Xamarin.Android 8.0+

The package multitargets .NET 5 and .NET 7 with additional optimizations.

## Features

- `BigIntegerPowCache` for cached lookups of powers of bases 3 to 10.
- `CountDigits()` and `CountDigitsAndTrailingZeros()` extension methods using highly optimized algorithms.
- `Divide()` extension method that supports a wide range of rounding modes instead of just truncating the result like the standard division operator.

## Further Reading

You can view the fully documented API on the [project documentation site](https://www.singulink.com/Docs/Singulink.Numerics.BigIntegerExtensions/api/Singulink.Numerics.html).
