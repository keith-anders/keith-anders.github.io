using FluentAssertions;
using NUnit.Framework;
using Shouldly;
using Fact = Xunit.FactAttribute;
using TestClass = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using XUnitAssert = Xunit.Assert;

/// <summary>
/// MSTest does not support any order comparisons in its asserts.
/// NUnit supports using a custom IComparer like so:
/// Is.GreaterThan(expectedValue).Using(myComparer)
/// Shouldly supports using a custom IComparer like so: actualValue.ShouldBeGreaterThan(expectedValue, myComparer, "Some context")
/// Of the assertions in this class,
/// XUnit only supports the range comparisons,
/// but it can take a custom IComparer in those methods.
/// </summary>
[TestClass]
[TestFixture]
public class OrderTests
{
    [TestMethod, Test, Fact]
    public void QuantityIsGreater()
    {
        int expectedValue = 4;
        int actualValue = 3;

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.GreaterThan(expectedValue), () => "Some context");
        // Some context
        //  Expected: greater than 4
        //  But was: 3

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().BeGreaterThan(expectedValue, "SOME REASONS");
        // Expected actualValue to be greater than 4 because SOME REASONS, but found 3.

        // Shouldly
        actualValue.ShouldBeGreaterThan(expectedValue, "Some context");
        // actualValue
        //   should be greater than
        // 4
        //   but was
        // 3
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void QuantityIsGreaterOrEqual()
    {
        int expectedValue = 4;
        int actualValue = 3;

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.GreaterThanOrEqualTo(expectedValue), () => "Some context");
        Assert.That(actualValue, Is.AtLeast(expectedValue), () => "Some context");
        // Some context
        //  Expected: greater than or equal to 4
        //  But was: 3

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().BeGreaterOrEqualTo(expectedValue, "SOME REASONS");
        // Expected actualValue to be greater or equal to 4 because SOME REASONS, but found 3.

        // Shouldly
        actualValue.ShouldBeGreaterThanOrEqualTo(expectedValue, "Some context");
        // actualValue
        //   should be greater than or equal to
        // 4
        //   but was
        // 3
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void QuantityIsLess()
    {
        int expectedValue = 2;
        int actualValue = 3;

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.LessThan(expectedValue), () => "Some context");
        // Some context
        //  Expected: less than 2
        //  But was: 3

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().BeLessThan(expectedValue, "SOME REASONS");
        // Expected actualValue to be less than 2 because SOME REASONS, but found 3.

        // Shouldly
        actualValue.ShouldBeLessThan(expectedValue, "Some context");
        // actualValue
        //   should be less than
        // 2
        //   but was
        // 3
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void QuantityIsLessOrEqual()
    {
        int expectedValue = 2;
        int actualValue = 3;

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.LessThanOrEqualTo(expectedValue), () => "Some context");
        Assert.That(actualValue, Is.AtMost(expectedValue), () => "Some context");
        // Some context
        //  Expected: less than or equal to 2
        //  But was: 3

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().BeLessOrEqualTo(expectedValue, "SOME REASONS");
        // Expected actualValue to be less or equal to 2 because SOME REASONS, but found 3.

        // Shouldly
        actualValue.ShouldBeLessThanOrEqualTo(expectedValue, "Some context");
        // actualValue
        //   should be less than or equal to
        // 2
        //   but was
        // 3
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void QuantityIsInRange()
    {
        int expectedLower = 1;
        int expectedUpper = 2;
        int actualValue = 3;

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.InRange(expectedLower, expectedUpper), () => "Some context");
        // Some context
        //  Expected: in range (1,2)
        //  But was: 3

        // XUnit
        XUnitAssert.InRange(actualValue, expectedLower, expectedUpper);
        //Assert.InRange() Failure
        //Range: (1 - 2)
        //Actual: 3

        // Fluent
        actualValue.Should().BeInRange(expectedLower, expectedUpper, "SOME REASONS");
        // Expected actualValue to be between 1 and 2 because SOME REASONS, but found 3.

        // Shouldly
        actualValue.ShouldBeInRange(expectedLower, expectedUpper, "Some context");
        // actualValue
        //   should be in range
        // { from = 1, to = 2 }
        //   but was
        // 3
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void QuantityIsNotInRange()
    {
        int expectedLower = 1;
        int expectedUpper = 3;
        int actualValue = 3;

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.Not.InRange(expectedLower, expectedUpper), () => "Some context");
        // Some context
        //  Expected: not in range (1,3)
        //  But was: 3

        // XUnit
        XUnitAssert.NotInRange(actualValue, expectedLower, expectedUpper); // Supports a custom IComparer<T>
        //Assert.NotInRange() Failure
        //Range: (1 - 3)
        //Actual: 3

        // Fluent
        actualValue.Should().NotBeInRange(expectedLower, expectedUpper, "SOME REASONS");
        // Expected actualValue to not be between 1 and 3 because SOME REASONS, but found 3.

        // Shouldly
        actualValue.ShouldNotBeInRange(expectedLower, expectedUpper, "Some context");
        // actualValue
        //   should not be in range
        // { from = 1, to = 3 }
        //   but was
        // 3
        //
        // Additional Info:
        //  Some context
    }
}