# Singulink.Numerics.BigIntegerExtensions

[![Join the chat](https://badges.gitter.im/Singulink/community.svg)](https://gitter.im/Singulink/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![View nuget packages](https://img.shields.io/nuget/v/Singulink.Numerics.BigIntegerExtensions.svg)](https://www.nuget.org/packages/Singulink.Numerics.BigIntegerExtensions/)
[![Build and Test](https://github.com/Singulink/Singulink.Numerics.BigIntegerExtensions/workflows/build%20and%20test/badge.svg)](https://github.com/Singulink/Singulink.Numerics.BigIntegerExtensions/actions?query=workflow%3A%22build+and+test%22)

**BigIntegerExtensions** provides some helper `BigInteger` extension methods and a super fast power cache that serves as a replacement for `BigInteger.Pow()` for bases between 3 and 10 (inclusive) to quickly get cached values instead of calculating them each time they are needed. The cache is used by [Singulink.Numerics.BigDecimal](https://github.com/Singulink/Singulink.Numerics.BigDecimal/) to greatly improve performance for many operations.

### About Singulink

*Shameless plug*: We are a small team of engineers and designers dedicated to building beautiful, functional and well-engineered software solutions. We offer very competitive rates as well as fixed-price contracts and welcome inquiries to discuss any custom development / project support needs you may have.

This package is part of our **Singulink Libraries** collection. Visit https://github.com/Singulink to see our full list of publicly available libraries and other open-source projects.

## Installation

The package is available on NuGet - simply install the `Singulink.Numerics.BigIntegerExtensions` package.

**Supported Runtimes**: Anywhere .NET Standard 2.0+ is supported, including:
- .NET Core 2.0+
- .NET Framework 4.6.1+
- Mono 5.4+
- Xamarin.iOS 10.14+
- Xamarin.Android 8.0+

## API

You can view the API on [FuGet](https://www.fuget.org/packages/Singulink.Numerics.BigIntegerExtensions). 
