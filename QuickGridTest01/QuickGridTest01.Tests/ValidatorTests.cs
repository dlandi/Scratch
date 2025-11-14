using QuickGridTest01.MultiState.Validation;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Validation;

public class ValidationResultTests
{
    [Fact]
    public void Success_CreatesValidResult()
    {
        // Act
        var result = ValidationResult.Success();

        // Assert
        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void Failure_CreatesInvalidResult()
    {
        // Act
        var result = ValidationResult.Failure("Test error");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Test error", result.ErrorMessage);
    }

    [Fact]
    public void Failure_ThrowsOnNullErrorMessage()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ValidationResult.Failure(null!));
    }
}

public class RequiredValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ReturnsSuccess_ForNonEmptyString()
    {
        // Arrange
        var validator = new RequiredValidator();

        // Act
        var result = await validator.ValidateAsync("test value");

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public async Task ValidateAsync_ReturnsFailure_ForEmptyString(string? value)
    {
        // Arrange
        var validator = new RequiredValidator();

        // Act
        var result = await validator.ValidateAsync(value);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UsesCustomErrorMessage()
    {
        // Arrange
        var validator = new RequiredValidator("Custom error");

        // Act
        var result = await validator.ValidateAsync("");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Custom error", result.ErrorMessage);
    }
}

public class EmailValidatorTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@example.com")]
    [InlineData("user+tag@example.co.uk")]
    [InlineData("test123@test-domain.com")]
    public async Task ValidateAsync_ReturnsSuccess_ForValidEmails(string email)
    {
        // Arrange
        var validator = new EmailValidator();

        // Act
        var result = await validator.ValidateAsync(email);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user @example.com")]
    [InlineData("user@.com")]
    public async Task ValidateAsync_ReturnsFailure_ForInvalidEmails(string email)
    {
        // Arrange
        var validator = new EmailValidator();

        // Act
        var result = await validator.ValidateAsync(email);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateAsync_ReturnsFailure_ForEmptyString(string? email)
    {
        // Arrange
        var validator = new EmailValidator();

        // Act
        var result = await validator.ValidateAsync(email);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("cannot be empty", result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UsesCustomErrorMessage()
    {
        // Arrange
        var validator = new EmailValidator("Invalid email format");

        // Act
        var result = await validator.ValidateAsync("bad-email");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid email format", result.ErrorMessage);
    }
}

public class PatternValidatorTests
{
    [Theory]
    [InlineData("(555) 123-4567")]
    [InlineData("555-123-4567")]
    [InlineData("5551234567")]
    public async Task ValidateAsync_ReturnsSuccess_ForMatchingPattern(string phone)
    {
        // Arrange - US phone number pattern
        var validator = new PatternValidator(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$");

        // Act
        var result = await validator.ValidateAsync(phone);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("abcd")]
    [InlineData("555-12-345")]
    public async Task ValidateAsync_ReturnsFailure_ForNonMatchingPattern(string phone)
    {
        // Arrange
        var validator = new PatternValidator(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$");

        // Act
        var result = await validator.ValidateAsync(phone);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public void Constructor_ThrowsOnNullPattern()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PatternValidator(null!));
    }

    [Fact]
    public void Constructor_ThrowsOnEmptyPattern()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PatternValidator(""));
    }

    [Fact]
    public async Task ValidateAsync_UsesCustomErrorMessage()
    {
        // Arrange
        var validator = new PatternValidator(@"^\d+$", "Must be numeric");

        // Act
        var result = await validator.ValidateAsync("abc");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Must be numeric", result.ErrorMessage);
    }
}

public class LengthValidatorTests
{
    [Theory]
    [InlineData("test", 2, 10)]
    [InlineData("ab", 2, 2)]
    [InlineData("hello world", 5, 20)]
    public async Task ValidateAsync_ReturnsSuccess_WhenLengthInRange(
        string value, int minLength, int maxLength)
    {
        // Arrange
        var validator = new LengthValidator(minLength, maxLength);

        // Act
        var result = await validator.ValidateAsync(value);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("a", 2, 10)]      // Too short
    [InlineData("12345678901", 2, 10)]  // Too long
    public async Task ValidateAsync_ReturnsFailure_WhenLengthOutOfRange(
        string value, int minLength, int maxLength)
    {
        // Arrange
        var validator = new LengthValidator(minLength, maxLength);

        // Act
        var result = await validator.ValidateAsync(value);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_MinLengthOnly_ValidatesCorrectly()
    {
        // Arrange
        var validator = new LengthValidator(minLength: 5);

        // Act & Assert
        var resultShort = await validator.ValidateAsync("abc");
        Assert.False(resultShort.IsValid);

        var resultLong = await validator.ValidateAsync("hello world");
        Assert.True(resultLong.IsValid);
    }

    [Fact]
    public async Task ValidateAsync_MaxLengthOnly_ValidatesCorrectly()
    {
        // Arrange
        var validator = new LengthValidator(maxLength: 5);

        // Act & Assert
        var resultShort = await validator.ValidateAsync("abc");
        Assert.True(resultShort.IsValid);

        var resultLong = await validator.ValidateAsync("hello world");
        Assert.False(resultLong.IsValid);
    }

    [Fact]
    public async Task ValidateAsync_HandlesNullValue()
    {
        // Arrange
        var validator = new LengthValidator(minLength: 1);

        // Act
        var result = await validator.ValidateAsync(null);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Constructor_ThrowsOnNegativeMinLength()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new LengthValidator(minLength: -1));
    }

    [Fact]
    public void Constructor_ThrowsOnNegativeMaxLength()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new LengthValidator(maxLength: -1));
    }

    [Fact]
    public void Constructor_ThrowsWhenMinGreaterThanMax()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new LengthValidator(minLength: 10, maxLength: 5));
    }
}

public class UniqueValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ReturnsSuccess_WhenValueIsUnique()
    {
        // Arrange
        var validator = new UniqueValidator((value, _) => Task.FromResult(true));

        // Act
        var result = await validator.ValidateAsync("unique@example.com");

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsFailure_WhenValueIsNotUnique()
    {
        // Arrange
        var validator = new UniqueValidator((value, _) => Task.FromResult(false));

        // Act
        var result = await validator.ValidateAsync("duplicate@example.com");

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PassesValueToCheckFunction()
    {
        // Arrange
        string? capturedValue = null;
        var validator = new UniqueValidator((value, _) =>
        {
            capturedValue = value;
            return Task.FromResult(true);
        });

        // Act
        await validator.ValidateAsync("test@example.com");

        // Assert
        Assert.Equal("test@example.com", capturedValue);
    }

    [Fact]
    public async Task ValidateAsync_PassesCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        CancellationToken capturedToken = default;
        var validator = new UniqueValidator((value, ct) =>
        {
            capturedToken = ct;
            return Task.FromResult(true);
        });

        // Act
        await validator.ValidateAsync("test@example.com", cts.Token);

        // Assert
        Assert.Equal(cts.Token, capturedToken);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateAsync_ReturnsFailure_ForEmptyString(string? value)
    {
        // Arrange
        var validator = new UniqueValidator((v, _) => Task.FromResult(true));

        // Act
        var result = await validator.ValidateAsync(value);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("cannot be empty", result.ErrorMessage);
    }

    [Fact]
    public void Constructor_ThrowsOnNullCheckFunction()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UniqueValidator(null!));
    }
}
