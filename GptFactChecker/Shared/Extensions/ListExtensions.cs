namespace Shared.Extensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        => list.ToList().IsNullOrEmpty();
    
    public static bool IsNullOrEmpty<T>(this List<T> list)
        => list is null || !list.Any();

    /// <summary>
    /// Splits the provided list into a specified number of parts.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="list">The list to split.</param>
    /// <param name="parts">The number of parts to split the list into.</param>
    /// <returns>A list of lists, representing the split parts of the original list.</returns>
    public static List<List<T>> SplitIntoParts<T>(this List<T> list, int parts)
    {
        if (parts <= 0)
            throw new ArgumentException("The number of parts must be positive.", nameof(parts));

        if (parts > list.Count)
            throw new ArgumentException("The number of parts cannot be greater than the number of elements in the list.", nameof(parts));

        var splitLists = new List<List<T>>(parts);

        int itemsPerPart = (int)Math.Ceiling((double)list.Count / parts);

        for (int i = 0; i < parts; i++)
        {
            int startIndex = i * itemsPerPart;

            int count = Math.Min(itemsPerPart, list.Count - startIndex);

            List<T> sublist = list.GetRange(startIndex, count);

            splitLists.Add(sublist);
        }

        return splitLists;
    }
}
