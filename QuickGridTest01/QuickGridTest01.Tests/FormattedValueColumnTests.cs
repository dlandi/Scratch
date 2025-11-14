using QuickGridTest01.FormattedValue.Component;
using QuickGridTest01.FormattedValue.Formatters;
using Microsoft.AspNetCore.Components.QuickGrid;
using Xunit;

namespace QuickGridTest01.Tests.FormattedValue.Component;

/// <summary>
/// Tests for FormattedValueColumn component
/// </summary>
public class FormattedValueColumnTests
{
    #region Test Models

    private class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion

    #region Parameter Validation Tests

    [Fact]
    public void OnParametersSet_WithNullProperty_ThrowsException()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = null!,
            Formatter = CurrencyFormatters.Currency()
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => column.OnParametersSetPublic());

        Assert.Contains("Property", exception.Message);
    }

    [Fact]
    public void OnParametersSet_WithNullFormatter_ThrowsException()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = null!
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => column.OnParametersSetPublic());

        Assert.Contains("Formatter", exception.Message);
    }

    [Fact]
    public void OnParametersSet_WithValidParameters_DoesNotThrow()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };

        // Act & Assert
        column.OnParametersSetPublic(); // Should not throw
    }

    #endregion

    #region Expression Compilation Tests

    [Fact]
    public void OnParametersSet_CompilesPropertyExpression()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };

        // Act
        column.OnParametersSetPublic();
        var compiledGetter = column.GetCompiledGetter();

        // Assert
        Assert.NotNull(compiledGetter);

        var item = new TestItem { Amount = 100m };
        var value = compiledGetter(item);
        Assert.Equal(100m, value);
    }

    [Fact]
    public void OnParametersSet_CachesCompiledExpression()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };

        // Act - Call OnParametersSet multiple times with same property
        column.OnParametersSetPublic();
        var firstGetter = column.GetCompiledGetter();

        column.OnParametersSetPublic();
        var secondGetter = column.GetCompiledGetter();

        // Assert - Should use cached compilation (same reference)
        Assert.Same(firstGetter, secondGetter);
    }

    [Fact]
    public void OnParametersSet_RecompilesWhenPropertyChanges()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };

        // Act - Compile first property
        column.OnParametersSetPublic();
        var firstGetter = column.GetCompiledGetter();

        // Change property
        column.Property = item => item.Amount * 2;
        column.OnParametersSetPublic();
        var secondGetter = column.GetCompiledGetter();

        // Assert - Should have different compiled getter
        Assert.NotSame(firstGetter, secondGetter);
    }

    #endregion

    #region Formatting Tests

    [Fact]
    public void Formatting_WithCurrencyFormatter_FormatsCorrectly()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };
        column.OnParametersSetPublic();

        var item = new TestItem { Amount = 1234.56m };

        // Act
        var getter = column.GetCompiledGetter();
        var value = getter(item);
        var formatted = column.Formatter(value);

        // Assert
        Assert.Contains("1", formatted);
        Assert.Contains("234", formatted);
        Assert.Contains("56", formatted);
    }

    [Fact]
    public void Formatting_WithNullValue_FormatsAsEmpty()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, string?>
        {
            Property = item => item.Name,
            Formatter = value => value?.ToString() ?? string.Empty
        };
        column.OnParametersSetPublic();

        var item = new TestItem { Name = null! };

        // Act
        var getter = column.GetCompiledGetter();
        var value = getter(item);
        var formatted = column.Formatter(value);

        // Assert
        Assert.Equal(string.Empty, formatted);
    }

    [Fact]
    public void Formatting_WithDifferentFormatters_ProducesDifferentResults()
    {
        // Arrange - Currency formatter
        var currencyColumn = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };
        currencyColumn.OnParametersSetPublic();

        // Arrange - Number formatter
        var numberColumn = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = NumericFormatters.Number(0)
        };
        numberColumn.OnParametersSetPublic();

        var item = new TestItem { Amount = 1234.56m };

        // Act
        var currencyGetter = currencyColumn.GetCompiledGetter();
        var currencyValue = currencyGetter(item);
        var currencyResult = currencyColumn.Formatter(currencyValue);

        var numberGetter = numberColumn.GetCompiledGetter();
        var numberValue = numberGetter(item);
        var numberResult = numberColumn.Formatter(numberValue);

        // Assert - Results should be different
        Assert.NotEqual(currencyResult, numberResult);
    }

    [Fact]
    public void Formatting_WithDateFormatter_FormatsDate()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, DateTime>
        {
            Property = item => item.Date,
            Formatter = DateTimeFormatters.ShortDate()
        };
        column.OnParametersSetPublic();

        var item = new TestItem { Date = new DateTime(2025, 1, 15) };

        // Act
        var getter = column.GetCompiledGetter();
        var value = getter(item);
        var formatted = column.Formatter(value);

        // Assert
        Assert.Contains("1", formatted);
        Assert.Contains("15", formatted);
        Assert.Contains("2025", formatted);
    }

    [Fact]
    public void Formatting_WithBooleanFormatter_FormatsBoolean()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, bool>
        {
            Property = item => item.IsActive,
            Formatter = SpecializedFormatters.YesNo()
        };
        column.OnParametersSetPublic();

        var activeItem = new TestItem { IsActive = true };
        var inactiveItem = new TestItem { IsActive = false };

        // Act
        var getter = column.GetCompiledGetter();

        var activeValue = getter(activeItem);
        var activeFormatted = column.Formatter(activeValue);

        var inactiveValue = getter(inactiveItem);
        var inactiveFormatted = column.Formatter(inactiveValue);

        // Assert
        Assert.Equal("Yes", activeFormatted);
        Assert.Equal("No", inactiveFormatted);
    }

    #endregion

    #region Sorting Tests

    [Fact]
    public void IsSortableByDefault_WithProperty_ReturnsTrue()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };
        column.OnParametersSetPublic();

        // Act
        var result = column.IsSortableByDefaultPublic();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OnParametersSet_AutoEnablesSorting()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency()
        };

        // Act
        column.OnParametersSetPublic();

        // Assert
        Assert.NotNull(column.SortBy);
    }

    [Fact]
    public void OnParametersSet_PreservesExplicitSortBy()
    {
        // Arrange
        var customSort = GridSort<TestItem>.ByDescending(item => item.Amount);
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount,
            Formatter = CurrencyFormatters.Currency(),
            SortBy = customSort
        };

        // Act
        column.OnParametersSetPublic();

        // Assert
        Assert.Equal(customSort, column.SortBy);
    }

    #endregion

    #region Complex Property Access Tests

    [Fact]
    public void PropertyAccess_WithNestedProperty_AccessesCorrectly()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, int>
        {
            Property = item => item.Name.Length,
            Formatter = NumericFormatters.Number(0)
        };
        column.OnParametersSetPublic();

        var item = new TestItem { Name = "Test" };

        // Act
        var getter = column.GetCompiledGetter();
        var value = getter(item);
        var formatted = column.Formatter(value);

        // Assert
        Assert.Equal(4, value);
        Assert.Equal("4", formatted);
    }

    [Fact]
    public void PropertyAccess_WithCalculatedProperty_CalculatesCorrectly()
    {
        // Arrange
        var column = new FormattedValueColumn<TestItem, decimal>
        {
            Property = item => item.Amount * 2,
            Formatter = CurrencyFormatters.Currency()
        };
        column.OnParametersSetPublic();

        var item = new TestItem { Amount = 100m };

        // Act
        var getter = column.GetCompiledGetter();
        var value = getter(item);

        // Assert
        Assert.Equal(200m, value);
    }

    #endregion
}

/// <summary>
/// Extensions to expose protected/internal methods for testing
/// </summary>
internal static class FormattedValueColumnTestExtensions
{
    public static void OnParametersSetPublic<TGridItem, TValue>(
        this FormattedValueColumn<TGridItem, TValue> column)
    {
        try
        {
            // Use reflection to call protected OnParametersSet
            var method = typeof(FormattedValueColumn<TGridItem, TValue>)
                .GetMethod("OnParametersSet",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
            method?.Invoke(column, null);
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            // Unwrap the reflection exception and throw the actual exception
            if (ex.InnerException != null)
                throw ex.InnerException;
            throw;
        }
    }

    public static bool IsSortableByDefaultPublic<TGridItem, TValue>(
        this FormattedValueColumn<TGridItem, TValue> column)
    {
        // Use reflection to call protected IsSortableByDefault
        var method = typeof(FormattedValueColumn<TGridItem, TValue>)
            .GetMethod("IsSortableByDefault",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
        return (bool)(method?.Invoke(column, null) ?? false);
    }

    public static Func<TGridItem, TValue>? GetCompiledGetter<TGridItem, TValue>(
        this FormattedValueColumn<TGridItem, TValue> column)
    {
        // Access private field via reflection
        var field = typeof(FormattedValueColumn<TGridItem, TValue>)
            .GetField("_compiledPropertyGetter",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
        return field?.GetValue(column) as Func<TGridItem, TValue>;
    }
}