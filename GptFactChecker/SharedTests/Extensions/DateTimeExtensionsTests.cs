using Shared.Extensions;

namespace SharedTests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void TestToIsoString_WithMinValue_ReturnsEmptyString()
    {
        var dateTime = DateTimeOffset.MinValue;
        var result = dateTime.ToIsoString();

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TestToIsoString_WithNonMinValue_ReturnsExpectedFormat()
    {
        var dateTime = new DateTimeOffset(2023, 7, 5, 12, 34, 56, TimeSpan.Zero);
        var result = dateTime.ToIsoString();

        Assert.Equal("2023-07-05T12:34:56.000Z", result);
    }

    [Fact]
    public void TestToIsoString_WithDifferentTimeZones_ReturnsUTCFormat()
    {
        var dateTime = new DateTimeOffset(2023, 7, 5, 12, 34, 56, TimeSpan.FromHours(-7)); // PDT
        var result = dateTime.ToIsoString();

        Assert.Equal("2023-07-05T19:34:56.000Z", result); // Corresponding UTC
    }

    [Fact]
    public void TestToIsoString_WithMaxValue_ReturnsExpectedFormat()
    {
        var dateTime = DateTimeOffset.MaxValue;
        var result = dateTime.ToIsoString();

        Assert.Equal("9999-12-31T23:59:59.999Z", result);
    }

    [Fact]
    public void TestToIsoString_WithCurrentDateTime_ReturnsExpectedFormat()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var result = dateTime.ToIsoString();

        Assert.Equal(dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'"), result);
    }
}