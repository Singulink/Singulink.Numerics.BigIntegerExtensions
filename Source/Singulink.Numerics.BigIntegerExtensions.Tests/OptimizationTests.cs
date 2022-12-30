extern alias BigIntegerExtensionsAssembly;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Singulink.Numerics.BigIntegerExtensions.Tests;

using BigIntegerExtensions = BigIntegerExtensionsAssembly::Singulink.Numerics.BigIntegerExtensions;

[TestClass]
public class OptimizationTests
{
    [TestMethod]
    public void OptimizationsEnabled()
    {
#if DEBUG
        BigIntegerExtensions.OptimizationsEnabled.ShouldBe(false);
#else
        BigIntegerExtensions.OptimizationsEnabled.ShouldBe(true);
#endif
    }
}