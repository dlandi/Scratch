using QuickGridTest01.FormattedValue.Demo.Models;
using Xunit;

namespace QuickGridTest01.FormattedValue.Tests.Demo;

/// <summary>
/// Tests for demo model classes ensuring calculated properties work correctly.
/// </summary>
public class DemoModelsTests
{
    #region Product Tests

    [Fact]
    public void Product_DiscountedPrice_CalculatesCorrectly()
    {
        // Arrange
        var product = new Product
        {
            Price = 100m,
            DiscountPercent = 20m
        };

        // Act
        var discountedPrice = product.DiscountedPrice;

        // Assert
        Assert.Equal(80m, discountedPrice);
    }

    [Fact]
    public void Product_DiscountedPrice_NoDiscount_ReturnFullPrice()
    {
        // Arrange
        var product = new Product
        {
            Price = 100m,
            DiscountPercent = 0m
        };

        // Act
        var discountedPrice = product.DiscountedPrice;

        // Assert
        Assert.Equal(100m, discountedPrice);
    }

    [Fact]
    public void Product_DiscountedPrice_FullDiscount_ReturnsZero()
    {
        // Arrange
        var product = new Product
        {
            Price = 100m,
            DiscountPercent = 100m
        };

        // Act
        var discountedPrice = product.DiscountedPrice;

        // Assert
        Assert.Equal(0m, discountedPrice);
    }

    #endregion

    #region Employee Tests

    [Fact]
    public void Employee_FullName_CombinesFirstAndLast()
    {
        // Arrange
        var employee = new Employee
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var fullName = employee.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void Employee_FullName_EmptyNames_ReturnsSpace()
    {
        // Arrange
        var employee = new Employee
        {
            FirstName = string.Empty,
            LastName = string.Empty
        };

        // Act
        var fullName = employee.FullName;

        // Assert
        Assert.Equal(" ", fullName);
    }

    #endregion

    #region PerformanceMetric Tests

    [Fact]
    public void PerformanceMetric_PercentOfTarget_CalculatesCorrectly()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 75m,
            TargetValue = 100m
        };

        // Act
        var percent = metric.PercentOfTarget;

        // Assert
        Assert.Equal(75m, percent);
    }

    [Fact]
    public void PerformanceMetric_PercentOfTarget_ExceedsTarget_ReturnsOverHundred()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 120m,
            TargetValue = 100m
        };

        // Act
        var percent = metric.PercentOfTarget;

        // Assert
        Assert.Equal(120m, percent);
    }

    [Fact]
    public void PerformanceMetric_PercentOfTarget_ZeroTarget_ReturnsZero()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 50m,
            TargetValue = 0m
        };

        // Act
        var percent = metric.PercentOfTarget;

        // Assert
        Assert.Equal(0m, percent);
    }

    [Fact]
    public void PerformanceMetric_ChangePercent_PositiveChange_CalculatesCorrectly()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 110m,
            PreviousValue = 100m
        };

        // Act
        var changePercent = metric.ChangePercent;

        // Assert
        Assert.Equal(10m, changePercent);
    }

    [Fact]
    public void PerformanceMetric_ChangePercent_NegativeChange_CalculatesCorrectly()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 90m,
            PreviousValue = 100m
        };

        // Act
        var changePercent = metric.ChangePercent;

        // Assert
        Assert.Equal(-10m, changePercent);
    }

    [Fact]
    public void PerformanceMetric_ChangePercent_ZeroPrevious_ReturnsZero()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 50m,
            PreviousValue = 0m
        };

        // Act
        var changePercent = metric.ChangePercent;

        // Assert
        Assert.Equal(0m, changePercent);
    }

    [Fact]
    public void PerformanceMetric_ChangePercent_NoChange_ReturnsZero()
    {
        // Arrange
        var metric = new PerformanceMetric
        {
            CurrentValue = 100m,
            PreviousValue = 100m
        };

        // Act
        var changePercent = metric.ChangePercent;

        // Assert
        Assert.Equal(0m, changePercent);
    }

    #endregion

    #region Transaction Tests

    [Fact]
    public void Transaction_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Description = "Test Transaction",
            Amount = 100.50m,
            Balance = 500.75m,
            Currency = "USD",
            Type = "Deposit"
        };

        // Assert
        Assert.Equal(1, transaction.Id);
        Assert.Equal("Test Transaction", transaction.Description);
        Assert.Equal(100.50m, transaction.Amount);
        Assert.Equal(500.75m, transaction.Balance);
        Assert.Equal("USD", transaction.Currency);
        Assert.Equal("Deposit", transaction.Type);
    }

    #endregion

    #region LogEntry Tests

    [Fact]
    public void LogEntry_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var timestamp = DateTime.Now;
        var log = new LogEntry
        {
            Id = 1,
            Timestamp = timestamp,
            Level = "INFO",
            Message = "Test log message",
            Source = "TestSource",
            Duration = 123.45,
            MemoryBytes = 1024
        };

        // Assert
        Assert.Equal(1, log.Id);
        Assert.Equal(timestamp, log.Timestamp);
        Assert.Equal("INFO", log.Level);
        Assert.Equal("Test log message", log.Message);
        Assert.Equal("TestSource", log.Source);
        Assert.Equal(123.45, log.Duration);
        Assert.Equal(1024, log.MemoryBytes);
    }

    #endregion
}
