using QuickGridTest01.FormattedValue.Formatters;
using Xunit;

namespace QuickGridTest01.Tests.FormattedValue.Formatters;

/// <summary>
/// Tests for SpecializedFormatters
/// </summary>
public class SpecializedFormattersTests
{
    #region FileSize() Tests

    [Fact]
    public void FileSize_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.FileSize();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(0, "0.00 B")]
    [InlineData(512, "512.00 B")]
    [InlineData(1024, "1.00 KB")]
    [InlineData(1536, "1.50 KB")]
    [InlineData(1048576, "1.00 MB")]
    [InlineData(1073741824, "1.00 GB")]
    [InlineData(1099511627776, "1.00 TB")]
    public void FileSize_DifferentSizes_UsesCorrectUnit(long bytes, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.FileSize();

        // Act
        var result = formatter(bytes);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FileSize_WithCustomDecimals_UsesSpecifiedDecimals()
    {
        // Arrange
        var formatter = SpecializedFormatters.FileSize(decimals: 1);
        var value = 1536L;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1.5 KB", result);
    }

    [Fact]
    public void FileSize_WithInvalidValue_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.FileSize();

        // Act
        var result = formatter("invalid");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region PhoneNumberUs() Tests

    [Fact]
    public void PhoneNumberUs_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.PhoneNumberUs();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("5551234567", "(555) 123-4567")]
    [InlineData("(555) 123-4567", "(555) 123-4567")]
    [InlineData("555-123-4567", "(555) 123-4567")]
    [InlineData("15551234567", "(555) 123-4567")]
    public void PhoneNumberUs_DifferentFormats_FormatsCorrectly(string input, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.PhoneNumberUs();

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PhoneNumberUs_WithInvalidLength_ReturnsInput()
    {
        // Arrange
        var formatter = SpecializedFormatters.PhoneNumberUs();
        var input = "123456";

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal("123456", result);
    }

    #endregion

    #region PhoneNumberInternational() Tests

    [Fact]
    public void PhoneNumberInternational_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.PhoneNumberInternational();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("15551234567", "+1 555-123-4567")]
    [InlineData("5551234567", "+1 555-123-4567")]
    [InlineData("+1 555-123-4567", "+1 555-123-4567")]
    public void PhoneNumberInternational_DifferentFormats_FormatsCorrectly(string input, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.PhoneNumberInternational();

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PhoneNumberInternational_WithInvalidLength_ReturnsInput()
    {
        // Arrange
        var formatter = SpecializedFormatters.PhoneNumberInternational();
        var input = "123456";

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal("123456", result);
    }

    #endregion

    #region CreditCardMasked() Tests

    [Fact]
    public void CreditCardMasked_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.CreditCardMasked();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("4111111111111111", "****-****-****-1111")]
    [InlineData("4111-1111-1111-1111", "****-****-****-1111")]
    [InlineData("4111 1111 1111 1111", "****-****-****-1111")]
    public void CreditCardMasked_DifferentFormats_MasksCorrectly(string input, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.CreditCardMasked();

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CreditCardMasked_WithShortNumber_ReturnsInput()
    {
        // Arrange
        var formatter = SpecializedFormatters.CreditCardMasked();
        var input = "123";

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal("123", result);
    }

    #endregion

    #region SocialSecurityNumber() Tests

    [Fact]
    public void SocialSecurityNumber_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.SocialSecurityNumber();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("123456789", "123-45-6789")]
    [InlineData("123-45-6789", "123-45-6789")]
    public void SocialSecurityNumber_DifferentFormats_FormatsCorrectly(string input, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.SocialSecurityNumber();

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SocialSecurityNumber_WithInvalidLength_ReturnsInput()
    {
        // Arrange
        var formatter = SpecializedFormatters.SocialSecurityNumber();
        var input = "12345";

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal("12345", result);
    }

    #endregion

    #region SocialSecurityNumberMasked() Tests

    [Fact]
    public void SocialSecurityNumberMasked_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.SocialSecurityNumberMasked();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("123456789", "***-**-6789")]
    [InlineData("123-45-6789", "***-**-6789")]
    public void SocialSecurityNumberMasked_DifferentFormats_MasksCorrectly(string input, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.SocialSecurityNumberMasked();

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SocialSecurityNumberMasked_WithInvalidLength_ReturnsInput()
    {
        // Arrange
        var formatter = SpecializedFormatters.SocialSecurityNumberMasked();
        var input = "12345";

        // Act
        var result = formatter(input);

        // Assert
        Assert.Equal("12345", result);
    }

    #endregion

    #region YesNo() Tests

    [Fact]
    public void YesNo_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.YesNo();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    public void YesNo_WithBoolean_ReturnsYesOrNo(bool value, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.YesNo();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void YesNo_WithNonBoolean_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.YesNo();

        // Act
        var result = formatter("not a boolean");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region CheckMark() Tests

    [Fact]
    public void CheckMark_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.CheckMark();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(true, "✓")]
    [InlineData(false, "✗")]
    public void CheckMark_WithBoolean_ReturnsCheckOrCross(bool value, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.CheckMark();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ActiveInactive() Tests

    [Fact]
    public void ActiveInactive_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.ActiveInactive();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(true, "Active")]
    [InlineData(false, "Inactive")]
    public void ActiveInactive_WithBoolean_ReturnsActiveOrInactive(bool value, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.ActiveInactive();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region EnabledDisabled() Tests

    [Fact]
    public void EnabledDisabled_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = SpecializedFormatters.EnabledDisabled();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData(true, "Enabled")]
    [InlineData(false, "Disabled")]
    public void EnabledDisabled_WithBoolean_ReturnsEnabledOrDisabled(bool value, string expected)
    {
        // Arrange
        var formatter = SpecializedFormatters.EnabledDisabled();

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion
}
