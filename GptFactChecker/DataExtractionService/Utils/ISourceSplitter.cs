namespace FactExtractionService.Utils;

public interface ISourceSplitter
{
    List<string> SplitSourceText(string text, int characterLimit);
}
