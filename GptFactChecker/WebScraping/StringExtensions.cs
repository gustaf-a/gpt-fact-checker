using System.Net;
using System.Text.RegularExpressions;

namespace WebScraping;

internal static class StringExtensions
{
    public static string CleanText(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.Replace("\n", " "); // Replaces newline characters with spaces

        text = WebUtility.HtmlDecode(text);

        text = Regex.Replace(text, @"\s+", " "); // Removes excessive spaces

        return text.Trim(); // Removes leading and trailing spaces
    }
}
