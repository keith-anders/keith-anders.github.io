using FluentAssertions;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;
using Fact = Xunit.FactAttribute;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestClass = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using XUnitAssert = Xunit.Assert;

/// <summary>
/// MSTest and XUnit support Func<object> where they support Action.
/// FluentAssertions and MSTest do not support checking for exceptions matching a type reference; you have to use a generic parameter.
/// MSTest checks that exceptions are exactly of a given type; it has no mechanism for checking whether the exception type is assignable.
/// NUnit checks for exact type equality with Throws.TypeOf and checks for assignability with Throws.InstanceOf
/// XUnit checks for exact type equality with Throws and checks for assignability with ThrowsAny
/// FluentAssertions checks for exact type equality with ThrowsExactly and checks for assignability with Throws
/// Shouldly checks for assignability on methods with the generic type parameter and checks for exact type equality in methods that take a type reference.
/// Shouldly has a problem with its message in async.
/// </summary>
[TestClass]
[TestFixture]
public class ExceptionTests
{
    [TestMethod, Test, Fact]
    public void ActionThrowsExceptionExactlyOfStaticType()
    {
        void a() { throw new ArgumentNullException("id"); }
        Action action = a;
        ArgumentException ae;

        // MSTest
        ae = MSTestAssert.ThrowsException<ArgumentException>(action);
        // Assert.ThrowsException failed. Threw exception ArgumentNullException, but exception ArgumentException was expected.
        // Exception Message: Value cannot be null.
        // Parameter name: id
        // Stack Trace: at ...

        // NUnit
        Assert.That(a, Throws.TypeOf<ArgumentException>(), () => "Some context");
        // Some context
        //  Expected: <System.ArgumentException>
        //  But was: <System.ArgumentNullException>

        // XUnit
        ae = XUnitAssert.Throws<ArgumentException>(action);
        // Assert.Throws() Failure
        // Expected: typeof(System.ArgumentException)
        // Actual:  typeof(System.ArgumentNullException): Value cannot be null.
        // Parameter name: id
        // ---- System.ArgumentNullException: Value cannot be null.
        // Parameter name: id

        // Fluent
        ae = action.Should().ThrowExactly<ArgumentException>("SOME REASONS").Which;
        // Expected type to be System.ArgumentException because SOME REASONS, but found System.ArgumentNullException.

        // Shouldly does not support this case.
    }

    [TestMethod, Test, Fact]
    public void ActionThrowsExceptionExactlyOfTypeReference()
    {
        void a() { throw new ArgumentNullException("id"); }
        Action action = a;
        Type t = typeof(ArgumentException);
        Exception ex;

        // MSTest does not support this case.

        // NUnit
        Assert.That(a, Throws.TypeOf(t), () => "Some context");
        // Some context
        //  Expected: <System.ArgumentException>
        //  But was: <System.ArgumentNullException>

        // XUnit
        ex = XUnitAssert.Throws(t, action);
        // Assert.Throws() Failure
        // Expected: typeof(System.ArgumentException)
        // Actual:  typeof(System.ArgumentNullException): Value cannot be null.
        // Parameter name: id
        // ---- System.ArgumentNullException: Value cannot be null.
        // Parameter name: id

        // Fluent does not support this case.

        // Shouldly
        ex = action.ShouldThrow("Some context", t);
        ex = Should.Throw(action, "Some context", t);
        // `action()`
        //   should throw
        // System.ArgumentException
        //   but threw
        // System.InvalidOperationException
    }

    [TestMethod, Test, Fact]
    public void ActionThrowsExceptionAssignableToStaticType()
    {
        void a() { throw new InvalidOperationException("Operation is invalid."); }
        Action action = a;
        ArgumentException ae;

        // MSTest does not support this case.
    
        // NUnit
        Assert.That(a, Throws.InstanceOf<ArgumentException>(), () => "Some context");
        // Some context
        //  Expected: instance of <System.ArgumentException>
        //  But was: <System.InvalidOperationException>

        // XUnit
        ae = XUnitAssert.ThrowsAny<ArgumentException>(action);
        // Assert.Throws() Failure
        // Expected: typeof(System.ArgumentException)
        // Actual:  typeof(System.InvalidOperationException): Operation is invalid.
        // ---- System.InvalidOperationException: Operation is invalid.

        // Fluent
        ae = action.Should().Throw<ArgumentException>("SOME REASONS").Which;
        // Expected a <System.ArgumentException> to be thrown because SOME REASONS, but found a <System.InvalidOperationException>: System.InvalidOperationException with message "Operation is invalid."
        // at...

        // Shouldly
        ae = action.ShouldThrow<ArgumentException>("Some context");
        // `action()`
        //   should throw
        // System.ArgumentException
        //   but threw
        // System.InvalidOperationException
        //
        // Additional Info:
        //  Some context ---> System.InvalidOperationException: Operation is invalid.
    }

    [TestMethod, Test, Fact]
    public void ActionThrowsExceptionAssignableToTypeReference()
    {
        void a() { throw new InvalidOperationException("Operation is invalid."); }
        Action action = a;
        Type t = typeof(ArgumentException);

        // MSTest does not support this case.

        // NUnit
        Assert.That(a, Throws.InstanceOf(t), () => "Some context");
        // Some context
        //  Expected: <System.ArgumentException>
        //  But was: <System.InvalidOperationException>

        // XUnit does not support this case.

        // FluentAssertions does not support this case.

        // Shouldly does not support this case.
    }
    
    [TestMethod, Test, Fact]
    public async Task AsyncThrowsExceptionExactlyOfStaticType()
    {
        async Task a() { await Task.Delay(1); throw new ArgumentNullException("id"); }
        Func<Task> action = a;
        ArgumentException ae;

        // MSTest
        ae = await MSTestAssert.ThrowsExceptionAsync<ArgumentException>(action, "Some context");
        // Assert.ThrowsException failed. Threw exception ArgumentNullException, but exception ArgumentException was expected. Some context
        // Exception Message: Value cannot be null.
        // Parameter name: id
        // Stack Trace: ...

        // NUnit does not support this use case.

        // XUnit
        ae = await XUnitAssert.ThrowsAsync<ArgumentException>(action);
        // Assert.Throws() Failure
        // Expected: typeof(System.ArgumentException)
        // Actual:  typeof(System.ArgumentNullException): Value cannot be null.
        // Parameter name: id
        // ---- System.ArgumentNullException: Value cannot be null.
        // Parameter name: id

        // Fluent
        ae = action.Should().ThrowExactly<ArgumentException>("SOME REASONS").Which;
        // Expected type to be System.ArgumentException because SOME REASONS, but found System.ArgumentNullException.

        // Shouldly does not support this case.
    }

    [TestMethod, Test, Fact]
    public async Task AsyncThrowsExceptionExactlyOfTypeReference()
    {
        async Task a() { await Task.Delay(1); throw new ArgumentNullException("id"); }
        Func<Task> action = a;
        Type t = typeof(ArgumentException);
        Exception ex;

        // MSTest does not support this case.

        // NUnit
        Assert.That(a, Throws.TypeOf(t), () => "Some context");
        // Some context
        //  Expected: <System.ArgumentException>
        //  But was: <System.ArgumentNullException>

        // XUnit
        ex = await XUnitAssert.ThrowsAsync(t, action);
        // Assert.Throws() Failure
        // Expected: typeof(System.ArgumentException)
        // Actual:  typeof(System.ArgumentNullException): Value cannot be null.
        // Parameter name: id
        // ---- System.ArgumentNullException: Value cannot be null.
        // Parameter name: id

        // Fluent does not support this case.

        // Shouldly
        ex = await Should.ThrowAsync(action, "Some context", t);
        // Shouldly uses your source code to generate its great error messages, build your test project with full debug information to get better error messages
        // The provided expression
        //   handle aggregate exception
        // System.ArgumentException
        //   but was
        // System.ArgumentNullException
        //
        // Additional Info:
        //   Some context
    }

    [TestMethod, Test, Fact]
    public async Task AsyncThrowsExceptionAssignableToStaticType()
    {
        async Task a() { await Task.Delay(1); throw new InvalidOperationException("Operation is invalid."); }
        Func<Task> action = a;
        ArgumentException ae;

        // MSTest does not support this case.

        // NUnit does not support this case.

        // XUnit
        ae = await XUnitAssert.ThrowsAnyAsync<ArgumentException>(action);
        // Assert.Throws() Failure
        // Expected: typeof(System.ArgumentException)
        // Actual:  typeof(System.InvalidOperationException): Operation is invalid.
        // ---- System.InvalidOperationException: Operation is invalid.

        // Fluent
        ae = action.Should().Throw<ArgumentException>("SOME REASONS").Which;
        // Expected System.ArgumentException because SOME REASONS, but found (aggregated) System.InvalidOperationException with message "Operation is invalid."
        // at...

        // Shouldly
        ae = await action.ShouldThrowAsync<ArgumentException>("Some context");
        // Shouldly uses your source code to generate its great error messages, build your test project with full debug information to get better error messages
        // The provided expression
        //   handle aggregate exception
        // System.ArgumentException
        //   but was
        // System.InvalidOperationException
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public async Task AsyncThrowsExceptionAssignableToTypeReference()
    {
        await Task.Delay(1);
        throw new InvalidOperationException();
        // All for different reasons, none of the frameworks support this case.
        // MSTest - doesn't support "assignable to"
        // NUnit - doesn't support async exception checking
        // XUnit - doesn't offer a non-generic version of ThrowsAnyAsync
        // Fluent - doesn't support non-generic exception type checking
        // Shouldly - doesn't support "assignable to" with type references
    }
}