namespace Shared.Extensions;

public static class DateTimeExtensions
{
    public static string ToIsoString(this DateTimeOffset dateTimeOffset, bool includeTime = true)
    {
        if (dateTimeOffset == DateTimeOffset.MinValue)
            return string.Empty;

        if (includeTime)
            return dateTimeOffset.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'");

        return dateTimeOffset.ToUniversalTime().ToString("yyyy-MM-dd");
    }
}
