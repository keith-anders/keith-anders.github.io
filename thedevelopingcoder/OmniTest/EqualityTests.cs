using FluentAssertions;
using NUnit.Framework;
using Shouldly;
using System;
using Fact = Xunit.FactAttribute;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestClass = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using XUnitAssert = Xunit.Assert;

/// <summary>
/// For all tests on value types (ints, floats, DateTimes, bools, etc),
/// MSTest and Shouldly will not take nullables.
/// NUnit and Fluent will take nullables on all.
/// XUnit will take nullables on all except DateTimes and TimeSpans.
/// For the floating point numbers, all frameworks support float and double. Decimal
/// is also supported by all but MSTest.
/// Shouldly also does not support approximate inequality of numbers, but it does
/// with times.
/// MSTest does not support approximate equality of times.
/// Fluent prefers BeCloseTo rather than Shouldly's Be.
/// </summary>
[TestClass]
[TestFixture]
public class EqualityTests
{
    [TestMethod, Test, Fact]
    public void Equality()
    {
        int expectedValue = 4;
        int actualValue = 3;

        // MSTest
        MSTestAssert.AreEqual(expectedValue, actualValue, "Some context");
        // Assert.AreEqual failed. Expected:<4>. Actual:<3>. Some context

        // NUnit
        Assert.That(actualValue, Is.EqualTo(expectedValue), () => "Some context");
        // Some context
        //  Expected: 4
        //  But was: 3

        // XUnit
        XUnitAssert.Equal(expectedValue, actualValue);
        // Assert.Equal() Failure
        // Expected: 4
        // Actual:  3

        // Fluent
        actualValue.Should().Be(expectedValue, "SOME REASONS");
        // Expected actualValue to be 3 because SOME REASONS, but found 4.

        // Shouldly
        actualValue.ShouldBe(expectedValue, "Some context");
        // actualValue
        //   should be
        // 3
        //   but was
        // 4
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void Inequality()
    {
        int expectedValue = 3;
        int actualValue = 3;
    
        // MSTest
        MSTestAssert.AreNotEqual(expectedValue, actualValue, "Some context");
        // Assert.AreNotEqual failed. Expected any value except:<3>. Actual:<3>. Some context

        // NUnit
        Assert.That(actualValue, Is.Not.EqualTo(expectedValue), () => "Some context");
        // Some context
        //  Expected: not equal to 3
        //  But was: 3

        // XUnit
        XUnitAssert.NotEqual(expectedValue, actualValue);
        // Assert.NotEqual() Failure
        // Expected: Not 3
        // Actual:  3

        // Fluent
        actualValue.Should().NotBe(expectedValue, "SOME REASONS");
        // Did not expect actualValue to be 3 because SOME REASONS.

        // Shouldly
        actualValue.ShouldNotBe(expectedValue, "Some context");
        // actualValue
        //   should not be
        // 3
        //   but was
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void False()
    {
        bool myBool = true;

        // MSTest
        MSTestAssert.IsFalse(myBool, "Some context");
        // Assert.IsFalse failed. Some context

        // NUnit
        Assert.That(myBool, Is.False, () => "Some context");
        // Some context
        //  Expected: False
        //  But was: True

        // XUnit
        XUnitAssert.False(myBool, "User message");
        // User message
        // Expected: False
        // Actual:  True

        // Fluent
        myBool.Should().BeFalse("SOME REASONS");
        // Expected myBool to be false because SOME REASONS, but found True.

        // Shouldly
        myBool.ShouldBeFalse("Some context");
        // myBool
        //   should be
        // False
        //   but was
        // True
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void True()
    {
        bool myBool = false;

        // MSTest
        MSTestAssert.IsTrue(myBool, "Some context");
        // Assert.IsTrue failed. Some context

        // NUnit
        Assert.That(myBool, Is.True, () => "Some context");
        // Some context
        //  Expected: True
        //  But was: False

        // XUnit
        XUnitAssert.True(myBool, "User message");
        // User message
        // Expected: True
        // Actual:  False

        // Fluent Assertions
        myBool.Should().BeTrue("SOME REASONS");
        // Expected myBool to be true because SOME REASONS, but found False.

        // Shouldly
        myBool.ShouldBeTrue("Some context");
        // myBool
        //   should be
        // True
        //   but was
        // False
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void ApproximateEqualityOfFloatingPointNumbers()
    {
        double pi = 3.13;
        
        // MSTest
        MSTestAssert.AreEqual(Math.PI, pi, 0.01, "Some context");
        // Assert.AreEqual failed. Expected a difference no greater than <0.01> between expected value <3.14159265358979> and actual value <3.13>. Some context

        // NUnit
        Assert.That(pi, Is.EqualTo(Math.PI).Within(0.1).Percent, () => "Some context");
        // Some context
        //  Expected: 3.1415926535897931d +/- 0.10000000000000001d Percent
        //  But was: 3.1299999999999999d

        // XUnit
        XUnitAssert.Equal(Math.PI, pi, 3);
        // Assert.Equal() Failure
        // Expected: 3.142 (rounded from 3.14159265358979)
        // Actual:  3.13 (rounded from 3.13)

        // Fluent Assertions
        pi.Should().BeApproximately(Math.PI, 0.01, "SOME REASONS");
        // Expected pi to approximate 3.1415926535897931 +/- 0.01 because SOME REASONS, but 3.13 differed by 0.011592653589793223.

        // Shouldly
        pi.ShouldBe(Math.PI, 0.01, "Some context");
        // pi
        //   should be within
        // 0.01d
        //   of
        // 3.14159265358979d
        //   but was
        // 3.13d
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void ApproximateInequalityOfFloatingPointNumbers()
    {
        double pi = 3.14;

        // MSTest
        MSTestAssert.AreNotEqual(Math.PI, pi, 0.01, "Some context");
        // Assert.AreNotEqual failed. Expected a difference greater than <0.01> between expected value <3.14159265358979> and actual value <3.14>. Some context

        // NUnit
        Assert.That(pi, Is.Not.EqualTo(Math.PI).Within(0.1).Percent, () => "Some context");
        // Some context
        //  Expected: not equal to 3.1415926535897931d +/- 0.10000000000000001d Percent
        //  But was: 3.1400000000000001d

        // XUnit
        XUnitAssert.NotEqual(Math.PI, pi, 1);
        // Message: Assert.NotEqual() Failure
        // Expected: Not 3.1 (rounded from 3.14159265358979)
        // Actual:  3.1 (rounded from 3.14)

        // Fluent
        pi.Should().NotBeApproximately(Math.PI, 0.01, "SOME REASONS");
        // Expected pi to not approximate 3.1415926535897931 +/- 0.01 because SOME REASONS, but 3.14 only differed by 0.001592653589793223.

        // Shouldly does not support this case.
    }
    
    [TestMethod, Test, Fact]
    public void ApproximateEqualityOfDateTimes()
    {
        DateTime dt = DateTime.Parse("2015-10-21T19:28:00");
        DateTime later = dt + TimeSpan.FromSeconds(2);
        
        // MSTest does not support this case.

        // NUnit
        Assert.That(later, Is.EqualTo(dt).Within(1).Seconds, () => "Some context");
        // Some context
        //  Expected: 2015-10-21 19:28:00 +/- 00:00:01
        //  But was: 2015-10-21 19:28:02

        // XUnit
        XUnitAssert.Equal(later, dt, TimeSpan.FromSeconds(1));
        // Assert.Equal() Failure
        // Expected: 10/21/2015 7:28:02 PM
        // Actual:  10/21/2015 7:28:00 PM difference 00:00:02 is larger than 00:00:01

        // Fluent Assertions
        dt.Should().BeCloseTo(later, TimeSpan.FromSeconds(1), "SOME REASONS"); // Also accepts an int number of milliseconds for the time span.
        // Expected dt to be within 1s from <2015-10-21 19:28:02> because SOME REASONS, but found <2015-10-21 19:28:00>.

        // Shouldly
        dt.ShouldBe(later, TimeSpan.FromSeconds(1), "Some context");
        // dt
        //   should be within
        // 00:00:01
        //   of
        // 2015-10-21T19:28:02.0000000
        //   but was
        // 2015-10-21T19:28:00.0000000
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void ApproximateInequalityOfDateTimes()
    {
        DateTime dt = DateTime.Parse("2015-10-21T19:28:00");
        DateTime later = dt + TimeSpan.FromSeconds(1);

        // MSTest does not support this case.

        // NUnit
        Assert.That(later, Is.Not.EqualTo(dt).Within(2).Seconds, () => "Some context");
        // Some context
        //  Expected: not equal to 2015-10-21 19:28:00 +/- 00:00:02
        //  But was: 2015-10-21 19:28:01

        // XUnit does not support this case.

        // Fluent
        dt.Should().NotBeCloseTo(later, TimeSpan.FromSeconds(2), "SOME REASONS"); // Also accepts an int number of milliseconds for the time span.
        // Did not expect dt to be within 2s from <2015-10-21 19:28:01> because SOME REASONS, but it was <2015-10-21 19:28:00>.

        // Shouldly
        dt.ShouldNotBe(later, TimeSpan.FromSeconds(2), "Some context");
        // dt
        //   should not be within
        // 00:00:02
        //   of
        // 2015-10-21T19:28:01.0000000
        //   but was
        // 2015-10-21T19:28:00.0000000
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void ApproximateEqualityOfTimeSpans()
    {
        TimeSpan ts = TimeSpan.FromSeconds(3600);
        TimeSpan larger = TimeSpan.FromSeconds(3602);

        // MSTest does not support this case.

        // NUnit
        Assert.That(ts, Is.EqualTo(larger).Within(1).Seconds, () => "Some context");
        // Some context
        //  Expected: 01:00:02 +/- 00:00:01
        //  But was: 01:00:00

        // XUnit does not support this case.

        // Fluent
        ts.Should().BeCloseTo(larger, TimeSpan.FromSeconds(1), "SOME REASONS"); // Also accepts an int number of milliseconds for the time span.
        // Expected ts to be within 1s from 1h and 2s because SOME REASONS, but found 1h.

        // Shouldly
        ts.ShouldBe(larger, TimeSpan.FromSeconds(1), "Some context");
        // ts
        //   should be within
        // 00:00:01
        //   of
        // 01:00:02
        //   but was
        // 01:00:00
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void ApproximateInequalityOfTimeSpans()
    {
        TimeSpan ts = TimeSpan.FromSeconds(3600);
        TimeSpan larger = TimeSpan.FromSeconds(3601);

        // MSTest does not support this case.

        // NUnit
        Assert.That(ts, Is.Not.EqualTo(larger).Within(2).Seconds, () => "Some context");
        // Some context
        //  Expected: not equal to 01:00:01 +/- 00:00:02
        //  But was: 01:00:00

        // XUnit does not support this case.

        // Fluent
        ts.Should().NotBeCloseTo(larger, TimeSpan.FromSeconds(2), "SOME REASONS"); // Also accepts an int number of milliseconds for the time span.
        // Expected ts to not be within 2s from 1h and 1s because SOME REASONS, but found 1h.

        // Shouldly
        ts.ShouldNotBe(larger, TimeSpan.FromSeconds(2), "Some context");
        // ts
        //   should not be within
        // 00:00:02
        //   of
        // 01:00:01
        //   but was
        // 01:00:00
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void ReferenceEquality()
    {
        object act = new object();
        object exp = new object();

        // MSTest
        MSTestAssert.AreSame(exp, act, "Some context");
        // Assert.AreSame failed. Some context

        // NUnit
        Assert.That(act, Is.SameAs(exp), () => "Some context");
        // Some context
        //  Expected: same as <System.Object>
        //  But was: <System.Object>

        // XUnit
        XUnitAssert.Same(exp, act);
        // Assert.Same() Failure
        // Expected: Object { }
        // Actual:  Object { }

        // Fluent
        act.Should().BeSameAs(exp, "SOME REASONS");
        // Expected act to refer to System.Object (HashCode=63566392) because SOME REASONS, but found System.Object (HashCode=24004906).

        // Shouldly
        act.ShouldBeSameAs(exp, "Some context");
        // act
        //   should be same as
        // System.Object (24965396)
        //   but was
        // System.Object (37190989)
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void ReferenceInequality()
    {
        object act = new object();
        object exp = act;

        // MSTest
        MSTestAssert.AreNotSame(exp, act, "Some context");
        // Assert.AreNotSame failed. Some context

        // NUnit
        Assert.That(act, Is.Not.SameAs(exp), () => "Some context");
        // Some context
        //  Expected: not same as <System.Object>
        //  But was: <System.Object>

        // XUnit
        XUnitAssert.NotSame(exp, act);
        // Assert.NotSame() Failure

        // Fluent
        act.Should().NotBeSameAs(exp, "SOME REASONS");
        // Did not expect act to refer to System.Object (HashCode=63566392) because SOME REASONS.

        // Shouldly
        act.ShouldNotBeSameAs(exp, "Some context");
        // act
        //   should not be same as
        // System.Object (48874361)
        //   but was
        // System.Object (48874361)
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void Nullity()
    {
        object act = new object();

        // MSTest
        MSTestAssert.IsNull(act, "Some context");
        // Assert.IsNull failed. Some context

        // NUnit
        Assert.That(act, Is.Null, () => "Some context");
        // Some context
        //  Expected: null
        //  But was: <System.Object>

        // XUnit
        XUnitAssert.Null(act);
        // Assert.Null() Failure
        // Expected: (null)
        // Actual:  Object { }

        // Fluent
        act.Should().BeNull("SOME REASONS");
        // Expected act to be <null> because SOME REASONS, but found System.Object (HashCode=63566392).

        // Shouldly
        act.ShouldBeNull("Some context");
        // act
        //   should be null but was
        // System.Object (63566392)
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void NonNullity()
    {
        object act = null;

        // MSTest
        MSTestAssert.IsNotNull(act, "Some context");
        // Assert.IsNotNull failed. Some context

        // NUnit
        Assert.That(act, Is.Not.Null, () => "Some context");
        // Some context
        //  Expected: not null
        //  But was: null

        // XUnit
        XUnitAssert.NotNull(act);
        // Assert.NotNull() Failure

        // Fluent
        act.Should().NotBeNull("SOME REASONS");
        // Expected act not to be <null> because SOME REASONS.

        // Shouldly
        act.ShouldNotBeNull("Some context");
        // act
        //   should not be null but was
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void FailedTest()
    {
        // MSTest
        MSTestAssert.Fail("Some context");
        // Assert.Fail failed. Some context

        // NUnit
        Assert.Fail("Some context");
        // Message: Some context

        // XUnit does not support this case.

        // Fluent Assertions does not support this case.

        // Shouldly does not support this case.
    }
}