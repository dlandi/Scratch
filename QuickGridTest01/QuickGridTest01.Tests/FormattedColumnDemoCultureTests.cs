using Bunit;
using Microsoft.AspNetCore.Components;
using QuickGridTest01.Pages;
using QuickGridTest01.FormattedValue.Formatters;
using Xunit;
using System.Globalization;

namespace QuickGridTest01.Tests.FormattedValue.Integration;

public class FormattedColumnDemoCultureTests : TestContext
{
    public FormattedColumnDemoCultureTests()
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.SetupModule("./_content/Microsoft.AspNetCore.Components.QuickGrid/QuickGrid.razor.js");
    }

    [Fact]
    public void FormattedColumnDemo_RendersWithDefaultCulture()
    {
        // Arrange & Act
        var cut = RenderComponent<FormattedColumnDemo>();
        var markup = cut.Markup;

        // Assert - Verify page renders
        Assert.Contains("Formatted Value Column Demo", markup); // current H1
        Assert.Contains("Culture", markup); // selector label (no colon now)
        Assert.Contains("Financial Transactions", markup);
        Assert.Contains("Product Inventory", markup);
        Assert.Contains("Employee Directory", markup);
    }

    [Fact]
    public void CultureSelector_ContainsAllExpectedCultures()
    {
        // Arrange & Act
        var cut = RenderComponent<FormattedColumnDemo>();
        var markup = cut.Markup;

        // Assert - Verify all culture options are present
        Assert.Contains("en-US", markup);
        Assert.Contains("en-GB", markup);
        Assert.Contains("de-DE", markup);
        Assert.Contains("fr-FR", markup);
        Assert.Contains("ja-JP", markup);
        Assert.Contains("es-ES", markup);
        
        Assert.Contains("English (United States)", markup);
        Assert.Contains("German (Germany)", markup);
        Assert.Contains("French (France)", markup);
    }

    [Fact]
    public void FinancialTransactions_DisplaysCurrencyInEnUS()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        
        // Act
        var cut = RenderComponent<FormattedColumnDemo>();
        var markup = cut.Markup;

        // Assert - US currency format: $5,000.00
        Assert.Contains("$", markup); // Dollar sign should be present
        
        // Look for formatted amounts - US format with commas
        var hasUsFormat = markup.Contains("$5,000") || 
                         markup.Contains("$1,500") || 
                         markup.Contains("$3,500") ||
                         markup.Contains("$800");
        
        Assert.True(hasUsFormat, $"Should contain US currency format. Sample: {markup.Substring(0, Math.Min(400, markup.Length))}");
    }

    [Fact]
    public void ChangeCultureProperty_UpdatesCultureInfo()
    {
        var cut = RenderComponent<FormattedColumnDemo>();
        var initialCulture = CultureInfo.CurrentCulture.Name;

        // Invoke private ApplyCulture directly (avoids StateHasChanged dispatcher constraint)
        var applyCulture = typeof(FormattedColumnDemo).GetMethod("ApplyCulture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(applyCulture);
        applyCulture!.Invoke(cut.Instance, new object[] { "de-DE" });

        Assert.Equal("de-DE", CultureInfo.CurrentCulture.Name);
        Assert.NotEqual(initialCulture, CultureInfo.CurrentCulture.Name);
    }

    [Fact]
    public void CurrencyFormatter_RespectsCurrentCulture_EnUS()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        var formatter = CurrencyFormatters.Currency(2);
        
        // Act
        var result = formatter(1234.56m);
        
        // Assert - US format: $1,234.56
        Assert.Contains("$", result);
        Assert.Contains("1,234", result);
        Assert.Contains(".56", result);
    }

    [Fact]
    public void CurrencyFormatter_RespectsCurrentCulture_DeDE()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
        var formatter = CurrencyFormatters.Currency(2);
        
        // Act
        var result = formatter(1234.56m);
        
        // Assert - German format: 1.234,56 €
        Assert.Contains("€", result);
        Assert.Contains("1.234", result); // Period for thousands
        Assert.Contains(",56", result);   // Comma for decimal
    }

    [Fact]
    public void CurrencyFormatter_RespectsCurrentCulture_FrFR()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
        var formatter = CurrencyFormatters.Currency(2);
        
        // Act
        var result = formatter(1234.56m);
        
        // Assert - French format: 1 234,56 €
        Assert.Contains("€", result);
        Assert.Contains("234", result);
        Assert.Contains(",56", result); // Comma for decimal
    }

    [Fact]
    public void NumericFormatter_RespectsCurrentCulture_EnUS()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        var formatter = NumericFormatters.Number(1);
        
        // Act
        var result = formatter(1234.5);
        
        // Assert - US format: 1,234.5
        Assert.Contains("1,234", result); // Comma for thousands
        Assert.Contains(".5", result);    // Period for decimal
    }

    [Fact]
    public void NumericFormatter_RespectsCurrentCulture_DeDE()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
        var formatter = NumericFormatters.Number(1);
        
        // Act
        var result = formatter(1234.5);
        
        // Assert - German format: 1.234,5
        Assert.Contains("1.234", result); // Period for thousands
        Assert.Contains(",5", result);    // Comma for decimal
    }

    [Fact]
    public void DateFormatter_RespectsCurrentCulture_EnUS()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        var formatter = DateTimeFormatters.ShortDate();
        
        // Act
        var result = formatter(new DateTime(2025, 1, 15));
        
        // Assert - US format: 1/15/2025
        Assert.Contains("1/15/2025", result);
    }

    [Fact]
    public void DateFormatter_RespectsCurrentCulture_DeDE()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
        var formatter = DateTimeFormatters.ShortDate();
        
        // Act
        var result = formatter(new DateTime(2025, 1, 15));
        
        // Assert - German format: 15.01.2025
        Assert.Contains("15.01.2025", result);
    }

    [Fact]
    public void PercentageFormatter_RespectsCurrentCulture_EnUS()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        var formatter = NumericFormatters.Percentage(1);
        
        // Act
        var result = formatter(0.15); // 15%
        
        // Assert - US format: 15.0%
        Assert.Contains("15.0", result);
        Assert.Contains("%", result);
    }

    [Fact]
    public void PercentageFormatter_RespectsCurrentCulture_DeDE()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
        var formatter = NumericFormatters.Percentage(1);
        
        // Act
        var result = formatter(0.15); // 15%
        
        // Assert - German format: 15,0 %
        Assert.Contains("15,0", result);
        Assert.Contains("%", result);
    }

    [Theory]
    [InlineData("en-US", "$")]
    [InlineData("de-DE", "€")]
    [InlineData("fr-FR", "€")]
    [InlineData("ja-JP", "¥")]
    public void CurrencyFormatter_UsesCorrectSymbol_ForCulture(string cultureName, string expectedSymbol)
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
        var formatter = CurrencyFormatters.Currency(2);
        
        // Act
        var result = formatter(1000m);
        
        // Assert
        Assert.Contains(expectedSymbol, result);
    }

    [Fact]
    public void CurrencyFormatter_SameFunctionInstance_ProducesDifferentResults_WhenCultureChanges()
    {
        // This is the KEY test that demonstrates the issue
        // The same formatter SHOULD produce different results
        // when CultureInfo.CurrentCulture changes
        
        // Arrange - Create formatter ONCE
        var formatter = CurrencyFormatters.Currency(2);
        decimal testValue = 1234.56m;
        
        // Act 1 - Format in US culture
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        var usResult = formatter(testValue);
        
        // Act 2 - Format in German culture
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
        var deResult = formatter(testValue);
        
        // Act 3 - Format in French culture
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
        var frResult = formatter(testValue);
        
        // Assert - All three results should be different
        Assert.Contains("$", usResult);      // US: $1,234.56
        Assert.Contains("€", deResult);      // DE: 1.234,56 €
        Assert.Contains("€", frResult);      // FR: 1 234,56 €
        
        Assert.NotEqual(usResult, deResult);
        Assert.NotEqual(usResult, frResult);
        Assert.NotEqual(deResult, frResult); // DE and FR differ in thousand separator
        
        // This test PASSES, proving that formatters DO respect CurrentCulture!
        // The issue must be in how the component re-renders, not in the formatters themselves.
    }
}

/// <summary>
/// Manual test instructions for verifying culture changes.
/// </summary>
public class ManualCultureTestInstructions
{
    /*
     * MANUAL TEST CHECKLIST FOR CULTURE CHANGES
     * ==========================================
     * 
     * DIAGNOSIS: Unit tests show that formatters DO respect CultureInfo.CurrentCulture.
     * The same formatter function produces different results when culture changes.
     * 
     * This means the issue is NOT with the formatters, but with the component lifecycle.
     * 
     * The problem is likely one of these:
     * 1. Component is not re-rendering when culture changes
     * 2. Formatter function calls are cached/memoized somewhere
     * 3. @key attribute is not forcing complete re-render
     * 
     * To verify:
     * 
     * 1. Start the application:
     *    dotnet run --project QuickGridTest01
     * 
     * 2. Navigate to: https://localhost:xxxx/formatted-column-demo
     * 
     * 3. Open browser DevTools Console
     * 
     * 4. Verify default (en-US) formatting:
     *    ? Financial Transactions shows: $5,000.00, $1,500.00
     *    ? Product prices show: $1,299.99, $29.99
     * 
     * 5. Change culture to German (de-DE) and check console for:
     *    - Any errors
     *    - Any warnings about re-rendering
     *    - Network activity (should NOT reload page)
     * 
     * 6. Inspect the DOM:
     *    - Check if @key value on demo-container changed
     *    - Check if QuickGrid elements have new keys
     *    - Check if cell content actually updated
     * 
     * 7. Try this in code-behind:
     *    Add logging to OnCultureChanged() to verify it's being called
     *    Add logging to LoadSampleData() to verify it's NOT being called again
     * 
     * 8. HYPOTHESIS TO TEST:
     *    The @key on demo-container should recreate the ENTIRE component tree,
     *    which should cause all formatter calls to be re-executed with new culture.
     *    
     *    If this doesn't work, the formatters might be getting "baked in" at
     *    compile time rather than being re-evaluated at render time.
     * 
     * NEXT STEPS IF ISSUE PERSISTS:
     * - Try removing @key from demo-container
     * - Try adding StateHasChanged() call after culture change
     * - Try making formatters into properties instead of method calls
     * - Check if there's any Blazor caching of attribute values
     */
}
