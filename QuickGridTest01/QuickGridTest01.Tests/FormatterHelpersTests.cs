using System.Globalization;
using QuickGridTest01.FormattedValue.Core;
using Xunit;

namespace QuickGridTest01.Tests.FormattedValue.Core;

/// <summary>
/// Tests for FormatterHelpers utility methods
/// </summary>
public class FormatterHelpersTests
{
    #region ToDouble Tests

    [Fact]
    public void ToDouble_WithNull_ReturnsNull()
    {
        // Act
        var result = FormatterHelpers.ToDouble(null);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(42, 42.0)]
    [InlineData(42.5, 42.5)]
    [InlineData("123.45", 123.45)]
    public void ToDouble_WithValidNumeric_ReturnsDouble(object input, double expected)
    {
        // Act
        var result = FormatterHelpers.ToDouble(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result.Value, precision: 10);
    }

    [Fact]
    public void ToDouble_WithInvalidString_ReturnsNull()
    {
        // Act
        var result = FormatterHelpers.ToDouble("not a number");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToDouble_WithDateTime_ReturnsNull()
    {
        // Act
        var result = FormatterHelpers.ToDouble(DateTime.Now);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region ToDecimal Tests

    [Fact]
    public void ToDecimal_WithNull_ReturnsNull()
    {
        // Act
        var result = FormatterHelpers.ToDecimal(null);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(42, 42)]
    [InlineData(42.5, 42.5)]
    [InlineData("123.45", 123.45)]
    public void ToDecimal_WithValidNumeric_ReturnsDecimal(object input, decimal expected)
    {
        // Act
        var result = FormatterHelpers.ToDecimal(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void ToDecimal_WithInvalidString_ReturnsNull()
    {
        // Act
        var result = FormatterHelpers.ToDecimal("not a number");

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region FormatOrDefault Tests

    [Fact]
    public void FormatOrDefault_WithNull_ReturnsDefaultEmpty()
    {
        // Arrange
        Func<object, string> formatter = obj => obj.ToString()!.ToUpper();

        // Act
        var result = FormatterHelpers.FormatOrDefault(null, formatter);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FormatOrDefault_WithNullAndCustomDefault_ReturnsCustomDefault()
    {
        // Arrange
        Func<object, string> formatter = obj => obj.ToString()!.ToUpper();

        // Act
        var result = FormatterHelpers.FormatOrDefault(null, formatter, "N/A");

        // Assert
        Assert.Equal("N/A", result);
    }

    [Fact]
    public void FormatOrDefault_WithValue_ReturnsFormattedValue()
    {
        // Arrange
        Func<object, string> formatter = obj => obj.ToString()!.ToUpper();

        // Act
        var result = FormatterHelpers.FormatOrDefault("hello", formatter);

        // Assert
        Assert.Equal("HELLO", result);
    }

    #endregion

    #region ApplyFormat Tests

    [Fact]
    public void ApplyFormat_WithNull_ReturnsEmpty()
    {
        // Act
        var result = FormatterHelpers.ApplyFormat(null, "C2");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ApplyFormat_WithDecimal_UsesCurrentCulture()
    {
        // Arrange
        var value = 1234.56m;

        // Act
        var result = FormatterHelpers.ApplyFormat(value, "C2");

        // Assert
        Assert.Contains("1", result);
        Assert.Contains("234", result);
        Assert.Contains("56", result);
    }

    [Fact]
    public void ApplyFormat_WithSpecificCulture_UsesProvidedCulture()
    {
        // Arrange
        var value = 1234.56m;
        var culture = new CultureInfo("en-US");

        // Act
        var result = FormatterHelpers.ApplyFormat(value, "C2", culture);

        // Assert
        Assert.StartsWith("$", result);
        Assert.Contains("1,234.56", result);
    }

    [Fact]
    public void ApplyFormat_WithNonFormattable_ReturnsToString()
    {
        // Arrange
        var value = new object();

        // Act
        var result = FormatterHelpers.ApplyFormat(value, "C2");

        // Assert
        Assert.Equal(value.ToString(), result);
    }

    #endregion

    #region CleanString Tests

    [Fact]
    public void CleanString_WithNull_ReturnsEmpty()
    {
        // Act
        var result = FormatterHelpers.CleanString(null, '-', '(', ')');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CleanString_WithEmpty_ReturnsEmpty()
    {
        // Act
        var result = FormatterHelpers.CleanString(string.Empty, '-', '(', ')');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CleanString_RemovesSpecifiedCharacters()
    {
        // Arrange
        var input = "(555) 123-4567";

        // Act
        var result = FormatterHelpers.CleanString(input, '-', '(', ')', ' ');

        // Assert
        Assert.Equal("5551234567", result);
    }

    [Fact]
    public void CleanString_PreservesOtherCharacters()
    {
        // Arrange
        var input = "ABC-123-XYZ";

        // Act
        var result = FormatterHelpers.CleanString(input, '-');

        // Assert
        Assert.Equal("ABC123XYZ", result);
    }

    #endregion

    #region IsValidLength Tests

    [Fact]
    public void IsValidLength_WithNull_ReturnsFalse()
    {
        // Act
        var result = FormatterHelpers.IsValidLength(null, 5, 10);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("12345", 5, 10, true)]
    [InlineData("1234567890", 5, 10, true)]
    [InlineData("1234", 5, 10, false)]
    [InlineData("12345678901", 5, 10, false)]
    [InlineData("123456", 5, 10, true)]
    public void IsValidLength_ChecksBounds(string input, int min, int max, bool expected)
    {
        // Act
        var result = FormatterHelpers.IsValidLength(input, min, max);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ChainFormatters Tests

    [Fact]
    public void ChainFormatters_ReturnsFirstNonEmpty()
    {
        // Arrange
        Func<object?, string> formatter1 = _ => string.Empty;
        Func<object?, string> formatter2 = _ => "second";
        Func<object?, string> formatter3 = _ => "third";

        var chained = FormatterHelpers.ChainFormatters(formatter1, formatter2, formatter3);

        // Act
        var result = chained("test");

        // Assert
        Assert.Equal("second", result);
    }

    [Fact]
    public void ChainFormatters_AllEmpty_ReturnsEmpty()
    {
        // Arrange
        Func<object?, string> formatter1 = _ => string.Empty;
        Func<object?, string> formatter2 = _ => "";
        Func<object?, string> formatter3 = _ => string.Empty;

        var chained = FormatterHelpers.ChainFormatters(formatter1, formatter2, formatter3);

        // Act
        var result = chained("test");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ChainFormatters_FirstSucceeds_StopsEarly()
    {
        // Arrange
        var callCount = 0;
        Func<object?, string> formatter1 = _ => { callCount++; return "first"; };
        Func<object?, string> formatter2 = _ => { callCount++; return "second"; };

        var chained = FormatterHelpers.ChainFormatters(formatter1, formatter2);

        // Act
        var result = chained("test");

        // Assert
        Assert.Equal("first", result);
        Assert.Equal(1, callCount); // Only first formatter should be called
    }

    #endregion

    #region WithNullDisplay Tests

    [Fact]
    public void WithNullDisplay_WithNull_ReturnsNullDisplay()
    {
        // Arrange
        Func<object?, string> baseFormatter = val => val?.ToString()?.ToUpper() ?? string.Empty;
        var formatter = FormatterHelpers.WithNullDisplay(baseFormatter, "N/A");

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal("N/A", result);
    }

    [Fact]
    public void WithNullDisplay_WithValue_UsesBaseFormatter()
    {
        // Arrange
        Func<object?, string> baseFormatter = val => val?.ToString()?.ToUpper() ?? string.Empty;
        var formatter = FormatterHelpers.WithNullDisplay(baseFormatter, "N/A");

        // Act
        var result = formatter("hello");

        // Assert
        Assert.Equal("HELLO", result);
    }

    #endregion
}
