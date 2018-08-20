using FluentAssertions;
using NUnit.Framework;
using Shouldly;
using System;
using Fact = Xunit.FactAttribute;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestClass = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestMethod = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using XUnitAssert = Xunit.Assert;

public class ModelBase { }
public interface IModel { int Id { get; } }
public class ModelDerived : ModelBase, IModel { public int Id { get { return 0; } } }

/// <summary>
/// MSTest and NUnit do not support getting the casted object back from the assert.
/// The others support that in the generic versions.
/// Fluent's parser has a bug on InstanceIsAssignableToStaticType.
/// </summary>
[TestClass]
[TestFixture]
public class TypeComparisonTests
{
    [TestMethod, Test, Fact]
    public void InstanceIsExactlyStaticType()
    {
        ModelBase act = new ModelDerived();
        ModelBase iModel;

        // MSTest does not support this case.

        // NUnit
        Assert.That(act, Is.TypeOf<ModelBase>(), () => "Some context");
        // Some context
        //  Expected: <ModelBase>
        //  But was: <ModelDerived>

        // XUnit
        iModel = XUnitAssert.IsType<ModelBase>(act);
        // Assert.IsType() Failure
        // Expected: ModelBase
        // Actual:  ModelDerived

        // Fluent
        iModel = act.Should().BeOfType<ModelBase>("SOME REASONS").Which;
        // Expected type to be ModelBase because SOME REASONS, but found ModelDerived.

        // Shouldly
        iModel = act.ShouldBeOfType<ModelBase>("Some context");
        // act
        //   should be of type
        // ModelBase
        //   but was
        // ModelDerived
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void InstanceIsExactlyTypeReference()
    {
        ModelBase act = new ModelDerived();
        Type t = typeof(ModelBase);

        // MSTest does not support this case.

        // NUnit
        Assert.That(act, Is.TypeOf(t), () => "Some context");
        // Some context
        //  Expected: <ModelBase>
        //  But was: <ModelDerived>

        // XUnit
        XUnitAssert.IsType(t, act);
        // Assert.IsType() Failure
        // Expected: ModelBase
        // Actual:  ModelDerived

        // Fluent
        act.Should().BeOfType(t, "SOME REASONS");
        // Expected type to be ModelBase because SOME REASONS, but found ModelDerived.

        // Shouldly
        act.ShouldBeOfType(t, "Some context");
        // act
        //   should be of type
        // ModelBase
        //   but was
        // ModelDerived
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void InstanceIsAssignableToStaticType()
    {
        ModelBase act = new ModelBase();
        IModel iModel;

        // MSTest does not support this case.
    
        // NUnit
        Assert.That(act, Is.InstanceOf<IModel>(), () => "Some context");
        // Some context
        //  Expected: instance of <IModel>
        //  But was: <ModelBase>

        // XUnit
        iModel = XUnitAssert.IsAssignableFrom<IModel>(act);
        // Assert.IsAssignableFrom() Failure
        // Expected: typeof(IModel)
        // Actual:  typeof(ModelBase)

        // Fluent
        iModel = act.Should().BeAssignableTo<IModel>("SOME REASONS").Which;
        // Expected iModel = act to be assignable to IModel because SOME REASONS, but ModelBase is not.

        // Shouldly
        iModel = act.ShouldBeAssignableTo<IModel>("Some context");
        // act
        //   should be assignable to
        // IModel
        //   but was
        // ModelBase
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void InstanceIsAssignableToTypeReference()
    {
        ModelBase act = new ModelBase();
        Type t = typeof(IModel);

        // MSTest
        MSTestAssert.IsInstanceOfType(act, t, "Some context");
        // Assert.IsInstanceOfType failed. Some context Expected type:<IModel>. Actual type:<ModelBase>.

        // NUnit
        Assert.That(act, Is.InstanceOf(t), () => "Some context");
        // Some context
        //  Expected: instance of <IModel>
        //  But was: <ModelBase>

        // XUnit
        XUnitAssert.IsAssignableFrom(t, act);
        // Assert.IsAssignableFrom() Failure
        // Expected: typeof(IModel)
        // Actua:  typeof(ModelBase)

        // Fluent
        act.Should().BeAssignableTo(t, "SOME REASONS");
        // Expected act to be assignable to IModel because SOME REASONS, but ModelBase is not.

        // Shouldly
        act.ShouldBeAssignableTo(t, "Some context");
        // act
        //   should be assignable to
        // IModel
        //   but was
        // ModelBase
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void InstanceIsNotExactlyStaticType()
    {
        ModelBase act = new ModelDerived();

        // MSTest does not support this case.
    
        // NUnit
        Assert.That(act, Is.Not.TypeOf<ModelDerived>(), () => "Some context");
        // Some context
        //  Expected: not <ModelDerived>
        //  But was: <ModelDerived>

        // XUnit
        XUnitAssert.IsNotType<ModelDerived>(act);
        // Assert.IsNotType() Failure
        // Expected: typeof(ModelDerived)
        // Actual:  typeof(ModelDerived)

        // Fluent
        act.Should().NotBeOfType<ModelDerived>("SOME REASONS");
        // Expected type not to be [ModelDerived, OmniTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null] because SOME REASONS, but it is.

        // Shouldly
        act.ShouldNotBeOfType<ModelDerived>("Some context");
        // act
        //   should not be of type
        // ModelDerived
        //   but was
        // ModelDerived (44338948)
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void InstanceIsNotExactlyTypeReference()
    {
        ModelBase act = new ModelDerived();
        Type t = typeof(ModelDerived);

        // MSTest does not support this case.

        // NUnit
        Assert.That(act, Is.Not.TypeOf(t), () => "Some context");
        // Some context
        //  Expected: not <ModelDerived>
        //  But was: <ModelDerived>

        // XUnit
        XUnitAssert.IsNotType(t, act);
        // Assert.IsNotType() Failure
        // Expected: typeof(ModelDerived)
        // Actual:  typeof(ModelDerived)

        // Fluent
        act.Should().NotBeOfType(t, "SOME REASONS");
        // Expected type not to be [ModelDerived, OmniTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null] because SOME REASONS, but it is.

        // Shouldly
        act.ShouldNotBeOfType(t, "Some context");
        // act
        //   should not be of type
        // ModelDerived
        //   but was
        // ModelDerived (44338948)
        //
        // Additional Info:
        //  Some context
    }

    [TestMethod, Test, Fact]
    public void InstanceIsNotAssignableToStaticType()
    {
        ModelBase act = new ModelDerived();
        
        // MSTest does not support this case.

        // NUnit
        Assert.That(act, Is.Not.InstanceOf<IModel>(), () => "Some context");
        // Some context
        //  Expected: not instance of <IModel>
        //  But was: <ModelDerived>

        // XUnit does not support this case.

        // Fluent
        act.Should().NotBeAssignableTo<IModel>("SOME REASONS");
        // Expected act to not be assignable to IModel because SOME REASONS, but ModelDerived is.

        // Shouldly
        act.ShouldNotBeAssignableTo<IModel>("Some context");
        // act
        //   should not be assignable to
        // IModel
        //   but was
        // ModelDerived (13726014)
        //
        // Additional Info:
        //  Some context
    }
    
    [TestMethod, Test, Fact]
    public void InstanceIsNotAssignableToTypeReference()
    {
        ModelBase act = new ModelDerived();
        Type t = typeof(IModel);

        // MSTest
        MSTestAssert.IsNotInstanceOfType(act, t, "Some context");
        // Assert.IsNotInstanceOfType failed. Wrong Type:<IModel>. Actual type:<ModelDerived>. Some context

        // NUnit
        Assert.That(act, Is.Not.InstanceOf(t), () => "Some context");
        // Some context
        //  Expected: not instance of <IModel>
        //  But was: <ModelDerived>

        // XUnit does not support this case.

        // Fluent
        act.Should().NotBeAssignableTo(t, "SOME REASONS");
        // Expected act to not be assignable to IModel because SOME REASONS, but ModelDerived is.

        // Shouldly
        act.ShouldNotBeAssignableTo(t, "Some context");
        // act
        //   should not be assignable to
        // IModel
        //   but was
        // ModelDerived (63566392)
        //
        // Additional Info:
        //  Some context
    }
}
