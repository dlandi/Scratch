using System.Globalization;
using QuickGridTest01.FormattedValue.Formatters;
using Xunit;

namespace QuickGridTest01.Tests.FormattedValue.Formatters;

/// <summary>
/// Tests for CurrencyFormatters
/// </summary>
public class CurrencyFormattersTests
{
    #region Currency() Tests

    [Fact]
    public void Currency_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = CurrencyFormatters.Currency();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Currency_WithDecimal_FormatsWithCurrentCulture()
    {
        // Arrange
        var formatter = CurrencyFormatters.Currency();
        var value = 1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("1", result);
        Assert.Contains("234", result);
        Assert.Contains("56", result);
    }

    [Fact]
    public void Currency_WithInteger_FormatsCorrectly()
    {
        // Arrange
        var formatter = CurrencyFormatters.Currency();
        var value = 1000;

        // Act
        var result = formatter(value);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("1", result);
        Assert.Contains("000", result);
    }

    [Fact]
    public void Currency_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = CurrencyFormatters.Currency(decimals: 0);
        var value = 1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.NotEmpty(result);
        // Should not contain decimal portion
        Assert.DoesNotContain(".56", result);
    }

    #endregion

    #region Currency(CultureInfo) Tests

    [Fact]
    public void CurrencyWithCulture_WithNull_ReturnsEmpty()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        var formatter = CurrencyFormatters.Currency(culture);

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("en-US", "$")]
    [InlineData("en-GB", "£")]
    [InlineData("ja-JP", "￥")]  // Japanese uses full-width yen symbol
    public void CurrencyWithCulture_DifferentCultures_UsesCorrectSymbol(string cultureName, string expectedSymbol)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var formatter = CurrencyFormatters.Currency(culture);
        var value = 1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains(expectedSymbol, result);
        Assert.Contains("1", result);
        Assert.Contains("234", result);
    }

    [Fact]
    public void CurrencyWithCulture_FrenchCulture_UsesSpaceAndComma()
    {
        // Arrange
        var culture = new CultureInfo("fr-FR");
        var formatter = CurrencyFormatters.Currency(culture);
        var value = 1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("€", result);
        // French uses space as thousand separator and comma as decimal
        Assert.Contains(",56", result);
    }

    [Fact]
    public void CurrencyWithCulture_NegativeValue_FormatsWithMinus()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        var formatter = CurrencyFormatters.Currency(culture);
        var value = -1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("$", result);
        Assert.Contains("1,234.56", result);
        // Contains negative indicator (format varies by culture)
        Assert.True(result.Contains("-") || result.Contains("("));
    }

    #endregion

    #region CurrencyAccounting() Tests

    [Fact]
    public void CurrencyAccounting_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyAccounting();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CurrencyAccounting_WithPositive_NoParentheses()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyAccounting();
        var value = 1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("$1,234.56", result);
        Assert.DoesNotContain("(", result);
        Assert.DoesNotContain(")", result);
    }

    [Fact]
    public void CurrencyAccounting_WithNegative_HasParentheses()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyAccounting();
        var value = -1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("($1,234.56)", result);
        Assert.StartsWith("(", result);
        Assert.EndsWith(")", result);
        Assert.DoesNotContain("-", result);
    }

    [Fact]
    public void CurrencyAccounting_WithZero_NoParentheses()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyAccounting();
        var value = 0m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("$0.00", result);
        Assert.DoesNotContain("(", result);
    }

    [Fact]
    public void CurrencyAccounting_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyAccounting(decimals: 0);
        var value = 1234.56m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("$1,235", result); // Rounded
    }

    [Fact]
    public void CurrencyAccounting_WithInvalidValue_ReturnsEmpty()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyAccounting();

        // Act
        var result = formatter("not a number");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region CurrencyWithSymbol() Tests

    [Fact]
    public void CurrencyWithSymbol_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyWithSymbol("€");

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("$", 1234.56, "$1,234.56")]
    [InlineData("€", 1234.56, "€1,234.56")]
    [InlineData("£", 1234.56, "£1,234.56")]
    [InlineData("¥", 1234, "¥1,234.00")]
    public void CurrencyWithSymbol_DifferentSymbols_UsesCorrectSymbol(string symbol, decimal value, string expected)
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyWithSymbol(symbol);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CurrencyWithSymbol_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyWithSymbol("$", decimals: 3);
        var value = 1234.567m;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("$1,234.567", result);
    }

    [Fact]
    public void CurrencyWithSymbol_WithInvalidValue_ReturnsEmpty()
    {
        // Arrange
        var formatter = CurrencyFormatters.CurrencyWithSymbol("$");

        // Act
        var result = formatter("invalid");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion
}
