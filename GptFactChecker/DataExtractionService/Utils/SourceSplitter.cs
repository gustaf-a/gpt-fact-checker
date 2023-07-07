using System.Text.RegularExpressions;

namespace FactExtractionService.Utils;

public class SourceSplitter : ISourceSplitter
{
    private const double SearchAreaSize = 0.2;

    public List<string> SplitSourceText(string text, int characterLimit)
    {
        if (IsInputInvalid(text, characterLimit))
            return new List<string> { text };

        var resultTextParts = new List<string>();

        int numberOfPartsNeeded = GetNumberOfPartsNecessary(text, characterLimit);

        for (int i = 0; i < numberOfPartsNeeded - 1; i++)
        {
            (string textPart, string remainingText) = GetTextPart(text, numberOfPartsNeeded - i);
            if (string.IsNullOrEmpty(textPart))
            {
                textPart = GetTextPartByLastSpace(text, characterLimit);
                remainingText = text.Substring(textPart.Length);
            }

            resultTextParts.Add(textPart);
            text = remainingText;
        }

        if (!string.IsNullOrWhiteSpace(text))
            resultTextParts.Add(text);

        return resultTextParts;
    }

    private static bool IsInputInvalid(string text, int characterLimit)
    {
        return string.IsNullOrWhiteSpace(text) || characterLimit <= 0 || text.Length <= characterLimit;
    }

    private (string, string) GetTextPart(string text, int partsRemaining)
    {
        int estimatedCharactersPerPart = GetCharactersPerPart(text, partsRemaining);
        int startSearch = (int)Math.Max(estimatedCharactersPerPart * (1 - SearchAreaSize), 0);
        int endSearch = (int)Math.Min(estimatedCharactersPerPart * (1 + SearchAreaSize), text.Length);

        foreach (var regex in _splitterRegexes)
        {
            string searchSpace = text.Substring(startSearch, endSearch - startSearch);
            Match match = Regex.Match(searchSpace, regex.Regex);
            if (match.Success)
            {
                int splitPoint = startSearch + match.Index + regex.SubstringIndexModification;
                return (text.Substring(0, splitPoint), text.Substring(splitPoint));
            }
        }

        return (null, text);
    }

    private static string GetTextPartByLastSpace(string text, int estimatedCharactersPerPart)
    {
        int splitPoint = text.LastIndexOf(' ', estimatedCharactersPerPart);
        return text.Substring(0, splitPoint);
    }

    private static int GetCharactersPerPart(string text, int parts)
    {
        return (int)Math.Floor((double)text.Length / parts);
    }

    private static int GetNumberOfPartsNecessary(string text, int characterLimit)
    {
        return (int)Math.Ceiling((double)text.Length / characterLimit);
    }

    private readonly List<SplitterRegex> _splitterRegexes = new List<SplitterRegex>
    {
        new SplitterRegex
        {
            Regex = @"\r?\n\r?\n",
            Description = "Matches double newlines for ending of a paragraph",
        },

        new SplitterRegex
        {
            Regex = @"\r?\n",
            Description = "Matches newlines for ending of a paragraph",
        },

        new SplitterRegex
        {
            Regex = @"\.\s{2,}[A-Z]",
            Description = "Matches a period followed by 2 or more whitespace characters and then an uppercase letter.",
            SubstringIndexModification = 2
        },

        new SplitterRegex
        {
            Regex = @"\.\s[A-Z]",
            Description = "Matches a period followed by a single whitespace character and then an uppercase letter.",
            SubstringIndexModification = 2
        },

        new SplitterRegex
        {
            Regex = @"\.[A-Z]",
            Description = "Matches a period followed by an uppercase letter.",
            SubstringIndexModification = 1
        },

        new SplitterRegex
        {
            Regex = @"\.\s{2,}",
            Description = "Matches a period followed by 2 or more whitespace characters.",
            SubstringIndexModification = 2
        },

        new SplitterRegex
        {
            Regex = @"\.\s",
            Description = "Matches a period followed by a single whitespace character.",
            SubstringIndexModification = 2
        },

        new SplitterRegex
        {
            Regex = @"\.[a-zA-Z]",
            Description = "Matches a period followed by any single alphabetical character, either uppercase or lowercase.",
            SubstringIndexModification = 1
        },

        new SplitterRegex
        {
            Regex = @"[A-Z]",
            Description = "Matches any single uppercase letter.",
        },

        new SplitterRegex
        {
            Regex = @"\.",
            Description = "Matches a period.",
        },
    };
}

internal class SplitterRegex
{
    public string Regex { get; set; }
    public string Description { get; set; }
    public int SubstringIndexModification = 0;
}

