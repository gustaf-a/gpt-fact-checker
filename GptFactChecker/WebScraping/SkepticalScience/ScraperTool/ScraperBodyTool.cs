using HtmlAgilityPack;
using Shared.Models;
using System.Text;

namespace WebScraping.SkepticalScience.ScraperTool;

internal class ScraperBodyTool : IScraperTool
{
    public string Name => nameof(ScraperBodyTool);

    private readonly HtmlNode _mainBody;
    private readonly string _url;

    public ScraperBodyTool(HtmlDocument doc, string url)
    {
        _mainBody = GetMainBody(doc);
        _url = url;
    }

    private const string MainBodyId = "mainbody";

    private static HtmlNode GetMainBody(HtmlDocument doc)
        => doc.DocumentNode.SelectSingleNode($"//div[@id='{MainBodyId}']");

    public ArgumentData GetArgumentData()
    {
        var climateMyth = GetClimateMythSection().CleanText();

        if (string.IsNullOrWhiteSpace(climateMyth))
        {
            Console.WriteLine($"Failed to find climate myth text for {_url}.");
            return null;
        }

        var counterArgumentSummary = GetCounterArgumentSummary().CleanText();
        if (string.IsNullOrWhiteSpace(counterArgumentSummary))
        {
            Console.WriteLine($"Failed to find counter argument summary for {_url}.");
            return null;
        }

        //var counterArgumentEntireText = GetCounterArgumentEntireText().CleanText();
        //if (string.IsNullOrWhiteSpace(counterArgumentSummary))
        //{
        //    Console.WriteLine($"Failed to load entire text for {_url}. Ignoring issue.");
        //}

        return new ArgumentData
        {
            ReferenceUrl = _url,
            ArgumentText = climateMyth,
            CounterArgumentSummary = counterArgumentSummary
            //CounterArgumentText = counterArgumentEntireText
        };
    }

    protected virtual string GetCounterArgumentSummary()
    {
        var atAGlanceSummary = GetAtAGlanceSection();
        if (!string.IsNullOrWhiteSpace(atAGlanceSummary))
            return atAGlanceSummary;

        var firstSummary = GetFirstSummarySection();
        if (!string.IsNullOrWhiteSpace(firstSummary))
            return firstSummary;

        return string.Empty;
    }

    protected const string AtAGlanceHeadingText = "At a glance";
    protected const string FirstSummarySectionHeading = "What the science says...";
    protected const string LastUpdateText = "Last updated";

    protected string GetClimateMythSection()
    {
        HtmlNodeCollection spanNodes = GetClimateMythDiv(_mainBody);

        if (!spanNodes.Any())
            return string.Empty;

        return GetInnerTextFromChildren(spanNodes);
    }

    protected string GetAtAGlanceSection()
    {
        var node = SelectSingleNodeByText(_mainBody, "h2", AtAGlanceHeadingText);

        if (node is null)
            return string.Empty;

        return GetInnerTextStartingFromNode(node, true);
    }

    private static string GetInnerTextStartingFromNode(HtmlNode node, bool ignoreFirstNode)
    {
        if (node is null)
            return string.Empty;

        var innertexts = new List<string>();

        var currentSpan = ignoreFirstNode ? node.NextSibling : node;

        GetAllInnerTextInSection(currentSpan, innertexts);

        return string.Join(" ", innertexts).CleanText();
    }

    private static void GetAllInnerTextInSection(HtmlNode node, List<string> innertexts)
    {
        while (node is not null)
        {
            if (IsTextMeaningless(node.InnerText))
            {
                node = node.NextSibling;
                continue;
            }

            if (IsNewSection(node))
                break;

            if (IsEndOfArticle(node))
            {
                Console.WriteLine("End of article encountered.");
                break;
            }

            innertexts.Add(node.InnerText);

            node = node.NextSibling;
        }
    }

    private static bool IsNewSection(HtmlNode node)
    {
        return node == null
                || node.Name == "hr"
                || node.Name == "h2"
                || (node.Name == "p" && node.GetAttributeValue("class", "").Contains("bluebox"));
    }

    private static bool IsEndOfArticle(HtmlNode node)
    {
        return node == null
        || (node.Name == "p"
            && node.GetAttributeValue("class", "").Contains("greenbox")
            && node.InnerText.Contains(LastUpdateText));
    }

    protected string GetFirstSummarySection()
    {
        var spanNode = SelectSingleNodeByText(_mainBody, "h2", FirstSummarySectionHeading);

        if (spanNode is null)
            return string.Empty;

        var nextMeaningfulElement = GetNextMeaningfulElement(spanNode.NextSibling);

        if (nextMeaningfulElement.IsName("table"))
            return GetInnerTextFromTableRowNumber(2, nextMeaningfulElement);

        var nextDiv = FindNextDiv(nextMeaningfulElement);

        if (nextDiv is not null)
            return GetInnerTextFromChildren(nextDiv);
            
        return string.Empty;
    }

    private static string GetInnerTextFromTableRowNumber(int rowNumber, HtmlNode spanNode)
    {
        if(spanNode is null)
            throw new ArgumentNullException(nameof(spanNode));

        var correctRow = GetRowNumber(rowNumber, spanNode);
        if(correctRow is null)
        {
            Console.WriteLine("Failed to find correct row number in table.");
            return string.Empty;
        }

        return correctRow.InnerText;
    }

    private static HtmlNode GetRowNumber(int rowNumber, HtmlNode node)
    {
        var rowCounter = 0;

        foreach (var childNode in node.ChildNodes)
        {
            if (childNode.IsName("tr"))
            {
                rowCounter++;

                if(rowCounter == rowNumber)
                    return childNode;
            }
        }

        return null;
    }

    // ----- Helper methods

    private static string GetInnerTextFromChildren(HtmlNode node)
    {
        var sb = new StringBuilder();

        AppendChildrenInnerText(node, sb);

        return sb.ToString();
    }

    private static string GetInnerTextFromChildren(HtmlNodeCollection spanNodes)
    {
        var sb = new StringBuilder();

        foreach (var spanNode in spanNodes)
            AppendChildrenInnerText(spanNode, sb);

        return sb.ToString();
    }

    private static void AppendChildrenInnerText(HtmlNode node, StringBuilder sb)
    {
        if (IsTextMeaningless(node.InnerText))
            return;

        sb.AppendLine(node.InnerText);
    }

    protected static bool IsTextMeaningless(string text)
    {
        if (text is null)
            return true;

        var trimmedText = text.Trim();

        if (string.IsNullOrWhiteSpace(trimmedText
                                        .Replace("\r\n", "")
                                        .Replace("\n", "")
                                        ))
        {
            return true;
        }

        return false;
    }

    // --------- HtmlAgilityPack methods

    protected static HtmlNodeCollection GetClimateMythDiv(HtmlNode node)
    {
        return node.SelectNodes("//div[@class='comment myth']");
    }

    protected static HtmlNode SelectSingleNodeByText(HtmlNode parentNode, string element, string nodeText)
    {
        return parentNode.SelectSingleNode($"//{element}[text()='{nodeText}']");
    }


    private HtmlNode GetNextRow(HtmlNode spanNode, int index)
    {
        if (spanNode.IsName("tr"))
            return GetNextMeaningfulElement(spanNode.NextSibling);

        return spanNode;
    }

    private HtmlNode GetNextMeaningfulElement(HtmlNode spanNode)
    {
        if (IsTextMeaningless(spanNode.InnerText))
            return GetNextMeaningfulElement(spanNode.NextSibling);

        return spanNode;
    }

    private static HtmlNode FindNextDiv(HtmlNode node)
    {
        if (node is null)
            return null;

        HtmlNode currentNode = node;

        while (!node.IsName("div"))
            currentNode = currentNode.NextSibling;

        return currentNode;
    }
}
