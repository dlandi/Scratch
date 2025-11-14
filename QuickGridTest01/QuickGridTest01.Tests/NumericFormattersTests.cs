using System.Globalization;
using QuickGridTest01.FormattedValue.Formatters;
using Xunit;

namespace QuickGridTest01.Tests.FormattedValue.Formatters;

/// <summary>
/// Tests for NumericFormatters
/// </summary>
public class NumericFormattersTests
{
    #region Number() Tests

    [Fact]
    public void Number_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.Number();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Number_WithThousands_HasSeparator()
    {
        // Arrange
        var formatter = NumericFormatters.Number();
        var value = 1234567.89;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains(",", result); // Assumes en-US culture
        Assert.Contains("234", result);
        Assert.Contains("567", result);
        Assert.Contains(".89", result);
    }

    [Fact]
    public void Number_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = NumericFormatters.Number(decimals: 0);
        var value = 1234.56;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1,235", result); // Rounded, no decimals
    }

    #endregion

    #region FixedPoint() Tests

    [Fact]
    public void FixedPoint_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.FixedPoint();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FixedPoint_WithThousands_NoSeparator()
    {
        // Arrange
        var formatter = NumericFormatters.FixedPoint();
        var value = 1234.56;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1234.56", result);
        Assert.DoesNotContain(",", result);
    }

    [Fact]
    public void FixedPoint_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = NumericFormatters.FixedPoint(decimals: 4);
        var value = 1234.5;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1234.5000", result);
    }

    #endregion

    #region Scientific() Tests

    [Fact]
    public void Scientific_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.Scientific();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Scientific_WithLargeNumber_UsesExponentialNotation()
    {
        // Arrange
        var formatter = NumericFormatters.Scientific();
        var value = 1234567.89;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("E", result);
        Assert.Contains("1.23", result);
    }

    [Fact]
    public void Scientific_WithSmallNumber_UsesExponentialNotation()
    {
        // Arrange
        var formatter = NumericFormatters.Scientific();
        var value = 0.0000123;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("E", result);
        Assert.Contains("1.23", result);
    }

    #endregion

    #region CompactNumber() Tests

    [Fact]
    public void CompactNumber_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.CompactNumber();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(123, "123.0")]
    [InlineData(1234, "1.2K")]
    [InlineData(12345, "12.3K")]
    [InlineData(1234567, "1.2M")]
    [InlineData(1234567890, "1.2B")]
    public void CompactNumber_DifferentMagnitudes_UsesCorrectSuffix(double value, string expected)
    {
        // Arrange
        var formatter = NumericFormatters.CompactNumber();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CompactNumber_WithNegative_IncludesMinusSign()
    {
        // Arrange
        var formatter = NumericFormatters.CompactNumber();
        var value = -1234567;

        // Act
        var result = formatter(value);

        // Assert
        Assert.StartsWith("-", result);
        Assert.Contains("1.2M", result);
    }

    [Fact]
    public void CompactNumber_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = NumericFormatters.CompactNumber(decimals: 2);
        var value = 1234567;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1.23M", result);
    }

    [Fact]
    public void CompactNumber_WithZero_ReturnsZero()
    {
        // Arrange
        var formatter = NumericFormatters.CompactNumber();
        var value = 0;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("0.0", result);
    }

    #endregion

    #region Percentage() Tests

    [Fact]
    public void Percentage_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.Percentage();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(0.1234, "12.34")]
    [InlineData(1.0, "100.00")]
    [InlineData(0.5, "50.00")]
    public void Percentage_WithDecimal_FormatsAsPercentage(double value, string expectedNumber)
    {
        // Arrange
        var formatter = NumericFormatters.Percentage();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains(expectedNumber, result);
        Assert.Contains("%", result);
    }

    [Fact]
    public void Percentage_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = NumericFormatters.Percentage(decimals: 0);
        var value = 0.1234;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("12", result);
        Assert.Contains("%", result);
        Assert.DoesNotContain(".34", result);
    }

    #endregion

    #region PercentageFromWhole() Tests

    [Fact]
    public void PercentageFromWhole_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.PercentageFromWhole();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(12.34, "12.34")]
    [InlineData(100, "100.00")]
    [InlineData(50, "50.00")]
    public void PercentageFromWhole_WithWholeNumber_FormatsAsPercentage(double value, string expectedNumber)
    {
        // Arrange
        var formatter = NumericFormatters.PercentageFromWhole();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains(expectedNumber, result);
        Assert.Contains("%", result);
    }

    [Fact]
    public void PercentageFromWhole_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = NumericFormatters.PercentageFromWhole(decimals: 1);
        var value = 12.345;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("12.3", result);
        Assert.Contains("%", result);
    }

    #endregion

    #region NumberWithPrefixSuffix() Tests

    [Fact]
    public void NumberWithPrefixSuffix_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.NumberWithPrefixSuffix("$", "kg");

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void NumberWithPrefixSuffix_WithValue_AddsPrefixAndSuffix()
    {
        // Arrange
        var formatter = NumericFormatters.NumberWithPrefixSuffix("±", "kg");
        var value = 12.345;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("±12.35kg", result);
    }

    [Fact]
    public void NumberWithPrefixSuffix_WithEmptyPrefixSuffix_FormatsNumberOnly()
    {
        // Arrange
        var formatter = NumericFormatters.NumberWithPrefixSuffix();
        var value = 12.34;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("12.34", result);
    }

    [Fact]
    public void NumberWithPrefixSuffix_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = NumericFormatters.NumberWithPrefixSuffix("~", "m", decimals: 1);
        var value = 12.345;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("~12.3m", result);
    }

    [Fact]
    public void NumberWithPrefixSuffix_WithInvalidValue_ReturnsEmpty()
    {
        // Arrange
        var formatter = NumericFormatters.NumberWithPrefixSuffix("$", "kg");

        // Act
        var result = formatter("invalid");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion
}
