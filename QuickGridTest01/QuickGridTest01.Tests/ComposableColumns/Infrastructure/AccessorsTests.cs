using System.Linq.Expressions;
using QuickGridTest01.ComposableColumns.Infrastructure;
using Xunit;

namespace QuickGridTest01.Tests.ComposableColumns.Infrastructure;

public class AccessorsTests
{
    private class TestClass
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string ReadOnlyProperty => "ReadOnly";
        public string? NullableProperty { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private class TestValueType
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }

    private class TestWithField
    {
        public string _field = "FieldValue";
    }

    #region CreateGetter Tests

    [Fact]
    public void CreateGetter_ForStringProperty_ReturnsCorrectValue()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var instance = new TestClass { Name = "TestName" };

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Equal("TestName", result);
    }

    [Fact]
    public void CreateGetter_ForIntProperty_ReturnsCorrectValue()
    {
        // Arrange
        Expression<Func<TestClass, int>> expr = x => x.Age;
        var instance = new TestClass { Age = 42 };

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void CreateGetter_ForReadOnlyProperty_ReturnsCorrectValue()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.ReadOnlyProperty;
        var instance = new TestClass();

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Equal("ReadOnly", result);
    }

    [Fact]
    public void CreateGetter_ForNullableProperty_HandlesNull()
    {
        // Arrange
        Expression<Func<TestClass, string?>> expr = x => x.NullableProperty;
        var instance = new TestClass { NullableProperty = null };

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CreateGetter_ForNullableProperty_HandlesValue()
    {
        // Arrange
        Expression<Func<TestClass, string?>> expr = x => x.NullableProperty;
        var instance = new TestClass { NullableProperty = "HasValue" };

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Equal("HasValue", result);
    }

    [Fact]
    public void CreateGetter_ForDateTimeProperty_ReturnsCorrectValue()
    {
        // Arrange
        var expectedDate = new DateTime(2024, 3, 15, 10, 30, 0);
        Expression<Func<TestClass, DateTime>> expr = x => x.CreatedAt;
        var instance = new TestClass { CreatedAt = expectedDate };

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Fact]
    public void CreateGetter_ForValueTypeProperty_ReturnsCorrectValue()
    {
        // Arrange
        Expression<Func<TestValueType, decimal>> expr = x => x.Amount;
        var instance = new TestValueType { Amount = 123.45m };

        // Act
        var getter = Accessors.CreateGetter(expr);
        var result = getter(instance);

        // Assert
        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void CreateGetter_CalledMultipleTimes_ReturnsSameValues()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var getter = Accessors.CreateGetter(expr);
        var instance = new TestClass { Name = "Consistent" };

        // Act
        var result1 = getter(instance);
        var result2 = getter(instance);
        var result3 = getter(instance);

        // Assert
        Assert.Equal("Consistent", result1);
        Assert.Equal("Consistent", result2);
        Assert.Equal("Consistent", result3);
    }

    [Fact]
    public void CreateGetter_UsedAcrossMultipleInstances_ReturnsCorrectValues()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var getter = Accessors.CreateGetter(expr);
        var instance1 = new TestClass { Name = "Instance1" };
        var instance2 = new TestClass { Name = "Instance2" };

        // Act
        var result1 = getter(instance1);
        var result2 = getter(instance2);

        // Assert
        Assert.Equal("Instance1", result1);
        Assert.Equal("Instance2", result2);
    }

    #endregion

    #region CreateSetter Tests

    [Fact]
    public void CreateSetter_ForWritableProperty_SetsValue()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var instance = new TestClass { Name = "Original" };

        // Act
        var setter = Accessors.CreateSetter(expr);
        Assert.NotNull(setter);
        setter!(instance, "Updated");

        // Assert
        Assert.Equal("Updated", instance.Name);
    }

    [Fact]
    public void CreateSetter_ForIntProperty_SetsValue()
    {
        // Arrange
        Expression<Func<TestClass, int>> expr = x => x.Age;
        var instance = new TestClass { Age = 10 };

        // Act
        var setter = Accessors.CreateSetter(expr);
        Assert.NotNull(setter);
        setter!(instance, 25);

        // Assert
        Assert.Equal(25, instance.Age);
    }

    [Fact]
    public void CreateSetter_ForReadOnlyProperty_ReturnsNull()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.ReadOnlyProperty;

        // Act
        var setter = Accessors.CreateSetter(expr);

        // Assert
        Assert.Null(setter);
    }

    [Fact]
    public void CreateSetter_ForNullableProperty_SetsNull()
    {
        // Arrange
        Expression<Func<TestClass, string?>> expr = x => x.NullableProperty;
        var instance = new TestClass { NullableProperty = "Value" };

        // Act
        var setter = Accessors.CreateSetter(expr);
        Assert.NotNull(setter);
        setter!(instance, null);

        // Assert
        Assert.Null(instance.NullableProperty);
    }

    [Fact]
    public void CreateSetter_ForNullableProperty_SetsValue()
    {
        // Arrange
        Expression<Func<TestClass, string?>> expr = x => x.NullableProperty;
        var instance = new TestClass { NullableProperty = null };

        // Act
        var setter = Accessors.CreateSetter(expr);
        Assert.NotNull(setter);
        setter!(instance, "NewValue");

        // Assert
        Assert.Equal("NewValue", instance.NullableProperty);
    }

    [Fact]
    public void CreateSetter_ForDateTimeProperty_SetsValue()
    {
        // Arrange
        var newDate = new DateTime(2024, 12, 25, 15, 45, 0);
        Expression<Func<TestClass, DateTime>> expr = x => x.CreatedAt;
        var instance = new TestClass { CreatedAt = DateTime.MinValue };

        // Act
        var setter = Accessors.CreateSetter(expr);
        Assert.NotNull(setter);
        setter!(instance, newDate);

        // Assert
        Assert.Equal(newDate, instance.CreatedAt);
    }

    [Fact]
    public void CreateSetter_ForValueTypeProperty_SetsValue()
    {
        // Arrange
        Expression<Func<TestValueType, decimal>> expr = x => x.Amount;
        var instance = new TestValueType { Amount = 0m };

        // Act
        var setter = Accessors.CreateSetter(expr);
        Assert.NotNull(setter);
        setter!(instance, 999.99m);

        // Assert
        Assert.Equal(999.99m, instance.Amount);
    }

    [Fact]
    public void CreateSetter_CalledMultipleTimes_UpdatesValue()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var setter = Accessors.CreateSetter(expr);
        var instance = new TestClass { Name = "Original" };

        // Act & Assert
        Assert.NotNull(setter);
        setter!(instance, "First");
        Assert.Equal("First", instance.Name);

        setter(instance, "Second");
        Assert.Equal("Second", instance.Name);

        setter(instance, "Third");
        Assert.Equal("Third", instance.Name);
    }

    [Fact]
    public void CreateSetter_UsedAcrossMultipleInstances_SetsCorrectValues()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var setter = Accessors.CreateSetter(expr);
        var instance1 = new TestClass { Name = "Original1" };
        var instance2 = new TestClass { Name = "Original2" };

        // Act
        Assert.NotNull(setter);
        setter!(instance1, "Updated1");
        setter(instance2, "Updated2");

        // Assert
        Assert.Equal("Updated1", instance1.Name);
        Assert.Equal("Updated2", instance2.Name);
    }

    #endregion

    #region Getter and Setter Integration Tests

    [Fact]
    public void GetterAndSetter_WorkTogether()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var getter = Accessors.CreateGetter(expr);
        var setter = Accessors.CreateSetter(expr);
        var instance = new TestClass { Name = "Original" };

        // Act
        var originalValue = getter(instance);
        Assert.NotNull(setter);
        setter!(instance, "Modified");
        var modifiedValue = getter(instance);

        // Assert
        Assert.Equal("Original", originalValue);
        Assert.Equal("Modified", modifiedValue);
    }

    [Fact]
    public void GetterAndSetter_ForValueType_WorkTogether()
    {
        // Arrange
        Expression<Func<TestValueType, int>> expr = x => x.Id;
        var getter = Accessors.CreateGetter(expr);
        var setter = Accessors.CreateSetter(expr);
        var instance = new TestValueType { Id = 1 };

        // Act
        var originalValue = getter(instance);
        Assert.NotNull(setter);
        setter!(instance, 100);
        var modifiedValue = getter(instance);

        // Assert
        Assert.Equal(1, originalValue);
        Assert.Equal(100, modifiedValue);
    }

    #endregion

    #region Performance Characteristics Tests

    [Fact]
    public void CreateGetter_CreatesDelegate_NotNull()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;

        // Act
        var getter = Accessors.CreateGetter(expr);

        // Assert
        Assert.NotNull(getter);
    }

    [Fact]
    public void CreateSetter_ForWritableProperty_CreatesDelegate_NotNull()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;

        // Act
        var setter = Accessors.CreateSetter(expr);

        // Assert
        Assert.NotNull(setter);
    }

    [Fact]
    public void CreateGetter_MultipleCallsOnSameExpression_ProducesWorkingDelegates()
    {
        // Arrange
        Expression<Func<TestClass, string>> expr = x => x.Name;
        var instance = new TestClass { Name = "Test" };

        // Act - Create multiple getters from the same expression
        var getter1 = Accessors.CreateGetter(expr);
        var getter2 = Accessors.CreateGetter(expr);

        // Assert - Both should work independently
        Assert.Equal("Test", getter1(instance));
        Assert.Equal("Test", getter2(instance));
    }

    #endregion
}
