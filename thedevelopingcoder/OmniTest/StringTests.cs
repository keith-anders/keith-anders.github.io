using FluentAssertions;
using NUnit.Framework;
using Shouldly;
using System;
using System.Text.RegularExpressions;
using Fact = Xunit.FactAttribute;
using MSTestStringAssert = Microsoft.VisualStudio.TestTools.UnitTesting.StringAssert;
using TestClass = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using XUnitAssert = Xunit.Assert;

/// <summary>
/// Note that the default comparison is case-sensitive for all frameworks except Shouldly
/// (and even on Shouldly case-sensitive is the default for ShouldBe).
/// MSTest supports most of the positive assertions here, but very few negative assertions.
/// (I.e. it supports "string contains" but not "string does not contain") MSTest also does
/// not support case-insensitive assertions for the most part.
/// XUnit also supports most positive assertions, and also DoesNotContain, but
/// it supports none of the other negative assertions.
/// Shouldly's errors do not mention the default case insensitive comparison on ShouldStartWith,
/// ShouldNotStartWith, ShouldEndWith, and ShouldNotEndWith.
/// </summary>
[TestClass]
[TestFixture]
public class StringTests
{
    [TestMethod, Test, Fact]
    public void StringContains()
    {
        string expectedValue = "XXX";
        string actualValue = "xxx";

        // MSTest
        MSTestStringAssert.Contains(actualValue, expectedValue, "Some context");
        // StringAssert.Contains failed. String 'xxx' does not contain string 'XXX'. Some context

        // NUnit
        Assert.That(actualValue, Does.Contain(expectedValue), () => "Some context");
        // Some context
        //  Expected: String containing "XXX"
        //  But was: "xxx"

        // XUnit
        XUnitAssert.Contains(expectedValue, actualValue);
        // Assert.Contains() Failure
        // Not found: XXX
        // In value: xxx

        // Fluent
        actualValue.Should().Contain(expectedValue, "SOME REASONS");
        // Expected actualValue "xxx" to contain "XXX" because SOME REASONS.

        // Shouldly
        actualValue.ShouldContain(expectedValue, "Some context", Case.Sensitive);
        // actualValue
        //   should contain
        // "XXX"
        //   but was actually
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotContain()
    {
        string expectedValue = "xx";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.Contain(expectedValue), () => "Some context");
        // Some context
        //  Expected: not String containing "xx"
        //  But was: "xxx"

        // XUnit
        XUnitAssert.DoesNotContain(expectedValue, actualValue);
        // Assert.DoesNotContain() Failure
        // Found: xx
        // In value: xxx

        // Fluent
        actualValue.Should().NotContain(expectedValue, "SOME REASONS");
        // Did not expect actualValue "xxx" to contain "xx" because SOME REASONS.

        // Shouldly
        actualValue.ShouldNotContain(expectedValue, "Some context", Case.Sensitive);
        // actualValue
        //   should not contain
        // "xx"
        //   but was actually
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringStartsWith()
    {
        string expectedValue = "XX";
        string actualValue = "xxx";

        // MSTest
        MSTestStringAssert.StartsWith(actualValue, expectedValue, "Some context");
        // StringAssert.StartsWith failed. String 'xxx' does not start with string 'XX'. Some context

        // NUnit
        Assert.That(actualValue, Does.StartWith(expectedValue), () => "Some context");
        // Some context
        //  Expected: String starting with "XX"
        //  But was: "xxx"

        // XUnit
        XUnitAssert.StartsWith(expectedValue, actualValue);
        // Assert.StartsWith() Failure
        // Not found: XX
        // In value: xx...

        // Fluent
        actualValue.Should().StartWith(expectedValue, "SOME REASONS");
        // Expected actualValue to start with "XX" because SOME REASONS, but "xxx" differs near "xxx" (index 0).

        // Shouldly
        actualValue.ShouldStartWith(expectedValue, "Some context", Case.Sensitive);
        // actualValue
        //   should start with
        // "XX"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotStartWith()
    {
        string expectedValue = "xx";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.StartWith(expectedValue), () => "Some context");
        // Some context
        //  Expected: not String starting with "xx"
        //  But was: "xxx"

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().NotStartWith(expectedValue, "SOME REASONS");
        // Expected actualValue that does not start with "xx" because SOME REASONS, but found "xxx".

        // Shouldly
        actualValue.ShouldNotStartWith(expectedValue, "Some context", Case.Sensitive);
        // actualValue
        //   should not start with
        // "xx"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringEndsWith()
    {
        string expectedValue = "XX";
        string actualValue = "xxx";

        // MSTest
        MSTestStringAssert.EndsWith(actualValue, expectedValue, "Some context");
        // StringAssert.EndsWith failed. String 'xxx' does not end with string 'XX'. Some context

        // NUnit
        Assert.That(actualValue, Does.EndWith(expectedValue), () => "Some context");
        // Some context
        //  Expected: String ending with "XX"
        //  But was: "xxx"

        // XUnit
        XUnitAssert.EndsWith(expectedValue, actualValue);
        // Assert.EndsWith() Failure
        // Expected: XX
        // Actual: ***xx

        // Fluent
        actualValue.Should().EndWith(expectedValue, "SOME REASONS");
        // Expected actualValue "xxx" to end with "XX" because SOME REASONS.

        // Shouldly
        actualValue.ShouldEndWith(expectedValue, "Some context", Case.Sensitive);
        // actualValue
        //   should end with
        // "XX"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotEndWith()
    {
        string expectedValue = "xx";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.EndWith(expectedValue), () => "Some context");
        // Some context
        //  Expected: not String ending with "xx"
        //  But was: "xxx"

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().NotEndWith(expectedValue, "SOME REASONS");
        // Expected actualValue "xxx" not to end with "xx" because SOME REASONS.

        // Shouldly
        actualValue.ShouldNotEndWith(expectedValue, "Some context", Case.Sensitive);
        // actualValue
        //   should not end with
        // "xx"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void StringMatchesRegexPattern()
    {
        string pattern = "....";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Match(pattern), () => "Some context");
        // Some context
        //  Expected: String matching "...."
        //  But was: "xxx"

        // XUnit
        XUnitAssert.Matches(pattern, actualValue);
        // Assert.Matches() Failure
        // Regex: ....
        // Value: xxx

        // Fluent
        actualValue.Should().MatchRegex(pattern, "SOME REASONS");
        // Expected actualValue to match regex "...." because SOME REASONS, but "xxx" does not match.

        // Shouldly
        actualValue.ShouldMatch(pattern, "Some context");
        // actualValue
        //   should match
        // "...."
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotMatchRegexPattern()
    {
        string pattern = "...";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.Match(pattern), () => "Some context");
        // Some context
        //  Expected: not String matching "..."
        //  But was: "xxx"

        // XUnit
        XUnitAssert.DoesNotMatch(pattern, actualValue);
        // Assert.DoesNotMatch() Failure
        // Regex: ...
        // Value: xxx

        // Fluent
        actualValue.Should().NotMatchRegex(pattern, "SOME REASONS");
        // Did not expect actualValue to match regex "..." because SOME REASONS, but "xxx" matches.

        // Shouldly
        actualValue.ShouldNotMatch(pattern, "Some context");
        // actualValue
        //   should not match "..." but did
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringMatchesRegexObject()
    {
        Regex pattern = new Regex("....");
        string actualValue = "xxx";

        // MSTest
        MSTestStringAssert.Matches(actualValue, pattern, "Some context");
        // StringAssert.Matches failed. String 'xxx' does not match pattern '....'. Some context.

        // NUnit does not support this case.

        // XUnit
        XUnitAssert.Matches(pattern, actualValue);
        // Assert.Matches() Failure
        // Regex: ....
        // Value: xxx

        // Fluent does not support this case.
        
        // Shouldly does not support this case.
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotMatchRegexObject()
    {
        Regex pattern = new Regex("...");
        string actualValue = "xxx";

        // MSTest
        MSTestStringAssert.DoesNotMatch(actualValue, pattern, "Some context");
        // StringAssert.DoesNotMatch failed. String 'xxx' matches pattern '...'. Some context.

        // NUnit does not support this case.

        // XUnit
        XUnitAssert.DoesNotMatch(pattern, actualValue);
        // Assert.DoesNotMatch() Failure:
        // Regex: ...
        // Value: xxx

        // Fluent does not support this case.

        // Shouldly does not support this case.
    }

    [TestMethod, Test, Fact]
    public void StringContainsCaseInsensitive()
    {
        string expectedValue = "XA";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Contain(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: String containing "XA", ignoring case
        //  But was: "xxx"

        // XUnit
        XUnitAssert.Contains(expectedValue, actualValue, StringComparison.CurrentCultureIgnoreCase);
        // Assert.Contains() Failure
        // Not found: XA
        // In value: xxx

        // Fluent
        actualValue.Should().ContainEquivalentOf(expectedValue, "SOME REASONS");
        // Expected actualValue to contain equivalent of "XA" because SOME REASONS but found "xxx".

        // Shouldly
        actualValue.ShouldContain(expectedValue, "Some context");
        // actualValue
        //   should contain (case insensitive comparison)
        // "XA"
        //   but was actually
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotContainCaseInsensitive()
    {
        string expectedValue = "XX";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.Contain(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: not String containing "XX", ignoring case
        //  But was: "xxx"

        // XUnit
        XUnitAssert.DoesNotContain(expectedValue, actualValue, StringComparison.CurrentCultureIgnoreCase);
        // Assert.DoesNotContain() Failure
        // Found: XX
        // In value: xxx

        // Fluent
        actualValue.Should().NotContainEquivalentOf(expectedValue, "SOME REASONS");
        // Did not expect actualValue to contain equivalent of "XX" because SOME REASONS but found "xxx".

        // Shouldly
        actualValue.ShouldNotContain(expectedValue, "Some context");
        // actualValue
        //   should not contain (case insensitive comparison)
        // "XX"
        //   but was actually
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringStartsWithCaseInsensitive()
    {
        string expectedValue = "XA";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.StartWith(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: String starting with "XA", ignoring case
        //  But was: "xxx"

        // XUnit
        XUnitAssert.StartsWith(expectedValue, actualValue, StringComparison.CurrentCultureIgnoreCase);
        // Assert.StartsWith() Failure
        // Not found: XA
        // In value: xx...

        // Fluent
        actualValue.Should().StartWithEquivalent(expectedValue, "SOME REASONS");
        // Expected actualValue to start with equivalent of "XA" because SOME REASONS, but "xxx" differs near "xx" (index 1).

        // Shouldly
        actualValue.ShouldStartWith(expectedValue, "Some context");
        // actualValue
        //   should start with
        // "XA"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotStartWithCaseInsensitive()
    {
        string expectedValue = "XX";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.StartWith(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: not String starting with "XX", ignoring case
        //  But was: "xxx"

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().NotStartWithEquivalentOf(expectedValue, "SOME REASONS");
        // Expected actualValue that does not start with equivalent of "XX" because SOME REASONS, but found "xxx".

        // Shouldly
        actualValue.ShouldNotStartWith(expectedValue, "Some context");
        // actualValue
        //   should not start with
        // "XX"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringEndsWithCaseInsensitive()
    {
        string expectedValue = "YX";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.EndWith(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: String ending with "YX", ignoring case
        //  But was: "xxx"

        // XUnit
        XUnitAssert.EndsWith(expectedValue, actualValue, StringComparison.CurrentCultureIgnoreCase);
        // Assert.EndsWith() Failure
        // Expected: YX
        // Actual: ***xx

        // Fluent
        actualValue.Should().EndWithEquivalent(expectedValue, "SOME REASONS");
        // Expected actualValue that ends with equivalent of "YX" because SOME REASONS, but found "xxx".

        // Shouldly
        actualValue.ShouldEndWith(expectedValue, "Some context");
        // actualValue
        //   should end with
        // "YX"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringDoesNotEndWithCaseInsensitive()
    {
        string expectedValue = "XX";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Does.Not.EndWith(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: not String ending with "XX", ignoring case
        //  But was: "xxx"

        // XUnit does not support this case.

        // Fluent
        actualValue.Should().NotEndWithEquivalentOf(expectedValue, "SOME REASONS");
        // Expected actualValue that does not end with equivalent of "xx" because SOME REASONS, but found "xxx".

        // Shouldly
        actualValue.ShouldNotEndWith(expectedValue, "Some context");
        // actualValue
        //   should not end with
        // "XX"
        //   but was
        // "xxx"
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringEqualityCaseInsensitive()
    {
        string expectedValue = "XX";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.EqualTo(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected string length 2 but was 3. Strings differ at index 2.
        //  Expected: "XX", ignoring case
        //  But was: "xxx"
        //  -----------^

        // XUnit does not support this case.
        XUnitAssert.Equal(expectedValue, actualValue, ignoreCase: true,
            ignoreLineEndingDifferences: false,
            ignoreWhiteSpaceDifferences: false);
        // Assert.Equal() Failure
        //             ↓ (pos 2)
        // Expected: XX
        // Actual:   xxx
        //             ↑ (pos 2)

        actualValue.Should().BeEquivalentTo(expectedValue, "SOME REASONS");
        // Expected actualValue to be equivalent to "XX" with a length of 2 because SOME REASONS, but "xxx" has a length of 3.

        // Shouldly
        actualValue.ShouldBe(expectedValue, "Some context",
            StringCompareShould.IgnoreCase /*| StringCompareShould.IgnoreLineEndings*/);
        // actualValue
        //   should be with options: Ignoring case
        // "XX"
        //   but was
        // "xxx"
        //   difference
        // Difference     |            |   
        //                |           \|/  
        // Index          | 0    1    2    
        // Expected Value | X    X         
        // Actual Value   | x    x    x    
        // Expected Code  | 88   88        
        // Actual Code    | 120  120  120  
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void StringInequalityCaseInsensitive()
    {
        string expectedValue = "XXX";
        string actualValue = "xxx";

        // MSTest does not support this case.

        // NUnit
        Assert.That(actualValue, Is.Not.EqualTo(expectedValue).IgnoreCase, () => "Some context");
        // Some context
        //  Expected: not equal to "XXX", ignoring case
        //  But was: "xxx"

        // XUnit does not support this case.
        XUnitAssert.NotEqual(expectedValue, actualValue, StringComparer.CurrentCultureIgnoreCase);
        // Assert.NotEqual() Failure
        // Expected: Not "XXX"
        // Actual: "xxx"

        // Fluent does not support this case.

        // Shouldly does not support this case.
    }
}