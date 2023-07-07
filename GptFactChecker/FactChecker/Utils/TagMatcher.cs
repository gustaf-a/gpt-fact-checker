namespace FactCheckingService.Utils;

public class TagMatcher : ITagMatcher
{
    public bool IsMatch(List<string> baseTags, List<string> potentiallyCompatibleTags)
    {
        foreach (var potentiallyCompatibleTag in potentiallyCompatibleTags)
            if (IsMatch(baseTags, potentiallyCompatibleTag))
                return true;

        return false;
    }

    public bool IsMatch(List<string> baseTags, string potentiallyCompatibleTag)
    {
        if (string.IsNullOrWhiteSpace(potentiallyCompatibleTag))
            return false;

        foreach (var baseTag in baseTags)
            if (IsMatch(baseTag, potentiallyCompatibleTag))
                return true;

        return false;
    }

    public bool IsMatch(string baseTag, string potentiallyCompatibleTag)
    {
        if (string.IsNullOrWhiteSpace(baseTag))
            return false;

        if (potentiallyCompatibleTag.Contains(baseTag.Trim(), StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
