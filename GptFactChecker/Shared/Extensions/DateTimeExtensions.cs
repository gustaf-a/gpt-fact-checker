namespace Shared.Extensions;

public static class DateTimeExtensions
{
    public static string ToIsoString(this DateTimeOffset dateTimeOffset)
    {
        if(dateTimeOffset == DateTimeOffset.MinValue)
            return string.Empty;

        return dateTimeOffset.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'");
    }
}
