namespace Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Retrieves the first non-null and non-whitespace string from the given collection.
    /// If the collection is null or doesn't contain any valid strings, returns an empty string.
    /// </summary>
    /// <param name="strings">The collection of strings to search.</param>
    /// <returns>The first non-null and non-whitespace string, or an empty string if no such string is found.</returns>
    public static string GetFirstNonEmptyString(this IEnumerable<string> strings)
    {
        if (strings is null)
            return string.Empty;

        foreach (string s in strings)
            if (!string.IsNullOrWhiteSpace(s))
                return s;

        return string.Empty;
    }
}
