using QuickGridTest01.FormattedValue.Formatters;
using Xunit;

namespace QuickGridTest01.Tests.FormattedValue.Formatters;

/// <summary>
/// Tests for DateTimeFormatters
/// </summary>
public class DateTimeFormattersTests
{
    #region ShortDate() Tests

    [Fact]
    public void ShortDate_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.ShortDate();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ShortDate_WithDateTime_FormatsShort()
    {
        // Arrange
        var formatter = DateTimeFormatters.ShortDate();
        var value = new DateTime(2025, 1, 15);

        // Act
        var result = formatter(value);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("1", result);
        Assert.Contains("15", result);
        Assert.Contains("2025", result);
    }

    [Fact]
    public void ShortDate_WithNonDateTime_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.ShortDate();

        // Act
        var result = formatter("not a date");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region LongDate() Tests

    [Fact]
    public void LongDate_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.LongDate();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void LongDate_WithDateTime_FormatsLong()
    {
        // Arrange
        var formatter = DateTimeFormatters.LongDate();
        var value = new DateTime(2025, 1, 15);

        // Act
        var result = formatter(value);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("January", result);
        Assert.Contains("15", result);
        Assert.Contains("2025", result);
    }

    #endregion

    #region DateTime() Tests

    [Fact]
    public void DateTime_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.DateTime();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void DateTime_WithCustomFormat_UsesFormat()
    {
        // Arrange
        var formatter = DateTimeFormatters.DateTime("yyyy-MM-dd HH:mm");
        var value = new DateTime(2025, 1, 15, 14, 30, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("2025-01-15 14:30", result);
    }

    [Fact]
    public void DateTime_WithDefaultFormat_FormatsGeneralShort()
    {
        // Arrange
        var formatter = DateTimeFormatters.DateTime();
        var value = new DateTime(2025, 1, 15, 14, 30, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("1", result);
        Assert.Contains("15", result);
    }

    #endregion

    #region IsoDate() Tests

    [Fact]
    public void IsoDate_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.IsoDate();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void IsoDate_WithDateTime_FormatsIso8601()
    {
        // Arrange
        var formatter = DateTimeFormatters.IsoDate();
        var value = new DateTime(2025, 1, 15);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("2025-01-15", result);
    }

    [Fact]
    public void IsoDate_WithSingleDigitMonth_PadsWithZero()
    {
        // Arrange
        var formatter = DateTimeFormatters.IsoDate();
        var value = new DateTime(2025, 3, 5);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("2025-03-05", result);
    }

    #endregion

    #region RelativeDate() Tests

    [Fact]
    public void RelativeDate_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RelativeDate_WithToday_ReturnsToday()
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();
        var value = DateTime.Now;

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("Today", result);
    }

    [Fact]
    public void RelativeDate_WithYesterday_ReturnsYesterday()
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();
        var value = DateTime.Now.AddDays(-1);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("Yesterday", result);
    }

    [Fact]
    public void RelativeDate_WithTomorrow_ReturnsTomorrow()
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();
        var value = DateTime.Now.AddDays(1);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("Tomorrow", result);
    }

    [Theory]
    [InlineData(-3, "3 days ago")]
    [InlineData(-5, "5 days ago")]
    public void RelativeDate_WithRecentPast_ReturnsDaysAgo(int daysAgo, string expected)
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();
        var value = DateTime.Now.AddDays(daysAgo);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3, "In 3 days")]
    [InlineData(5, "In 5 days")]
    public void RelativeDate_WithNearFuture_ReturnsInDays(int daysAhead, string expected)
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();
        var value = DateTime.Now.AddDays(daysAhead);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RelativeDate_WithDistantPast_ReturnsFormattedDate()
    {
        // Arrange
        var formatter = DateTimeFormatters.RelativeDate();
        var value = new DateTime(2024, 1, 15);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("Jan", result);
        Assert.Contains("15", result);
        Assert.Contains("2024", result);
    }

    #endregion

    #region Duration() Tests

    [Fact]
    public void Duration_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.Duration();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Duration_WithTimeSpan_FormatsAsHHMMSS()
    {
        // Arrange
        var formatter = DateTimeFormatters.Duration();
        var value = new TimeSpan(2, 30, 45);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("02:30:45", result);
    }

    [Fact]
    public void Duration_WithShortTimeSpan_PadsWithZeros()
    {
        // Arrange
        var formatter = DateTimeFormatters.Duration();
        var value = new TimeSpan(0, 5, 10);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("00:05:10", result);
    }

    [Fact]
    public void Duration_WithNonTimeSpan_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.Duration();

        // Act
        var result = formatter("not a timespan");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region HumanDuration() Tests

    [Fact]
    public void HumanDuration_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.HumanDuration();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void HumanDuration_WithDays_ShowsDaysAndHours()
    {
        // Arrange
        var formatter = DateTimeFormatters.HumanDuration();
        var value = new TimeSpan(1, 2, 30, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1d 2h", result);
    }

    [Fact]
    public void HumanDuration_WithHours_ShowsHoursAndMinutes()
    {
        // Arrange
        var formatter = DateTimeFormatters.HumanDuration();
        var value = new TimeSpan(1, 23, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("1h 23m", result);
    }

    [Fact]
    public void HumanDuration_WithMinutes_ShowsMinutesAndSeconds()
    {
        // Arrange
        var formatter = DateTimeFormatters.HumanDuration();
        var value = new TimeSpan(0, 5, 10);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("5m 10s", result);
    }

    [Fact]
    public void HumanDuration_WithSeconds_ShowsSecondsOnly()
    {
        // Arrange
        var formatter = DateTimeFormatters.HumanDuration();
        var value = new TimeSpan(0, 0, 45);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("45s", result);
    }

    #endregion

    #region Time12Hour() Tests

    [Fact]
    public void Time12Hour_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.Time12Hour();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Time12Hour_WithAfternoon_ShowsPM()
    {
        // Arrange
        var formatter = DateTimeFormatters.Time12Hour();
        var value = new DateTime(2025, 1, 15, 14, 30, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("2:30", result);
        Assert.Contains("PM", result);
    }

    [Fact]
    public void Time12Hour_WithMorning_ShowsAM()
    {
        // Arrange
        var formatter = DateTimeFormatters.Time12Hour();
        var value = new DateTime(2025, 1, 15, 9, 15, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Contains("9:15", result);
        Assert.Contains("AM", result);
    }

    #endregion

    #region Time24Hour() Tests

    [Fact]
    public void Time24Hour_WithNull_ReturnsEmpty()
    {
        // Arrange
        var formatter = DateTimeFormatters.Time24Hour();

        // Act
        var result = formatter(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Time24Hour_WithAfternoon_Shows24Hour()
    {
        // Arrange
        var formatter = DateTimeFormatters.Time24Hour();
        var value = new DateTime(2025, 1, 15, 14, 30, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("14:30", result);
    }

    [Fact]
    public void Time24Hour_WithMorning_PadsWithZero()
    {
        // Arrange
        var formatter = DateTimeFormatters.Time24Hour();
        var value = new DateTime(2025, 1, 15, 9, 15, 0);

        // Act
        var result = formatter(value);

        // Assert
        Assert.Equal("09:15", result);
    }

    #endregion
}
