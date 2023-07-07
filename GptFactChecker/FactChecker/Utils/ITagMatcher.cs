namespace FactCheckingService.Utils;

public interface ITagMatcher
{
    bool IsMatch(List<string> baseTags, List<string> potentiallyCompatibleTags);
    bool IsMatch(List<string> baseTags, string potentiallyCompatibleTag);
    bool IsMatch(string baseTag, string potentiallyCompatibleTag);
}
