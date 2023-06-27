using HtmlAgilityPack;
using Shared.Models;
using System.Text.RegularExpressions;

namespace WebScraping.SkepticalScience;

internal class SkepticalScienceScraper
{
    static readonly HttpClient _client = new HttpClient();

    public async Task<List<ArgumentData>> ScrapeUrls(List<string> urls)
    {
        Console.WriteLine($"Found {urls.Count} urls to scrape.");

        var argumentData = new List<ArgumentData>();

        var exceptionCounter = 0;

        foreach (var url in urls)
        {
            try
            {
                Console.WriteLine($"Scraping {url}");

                var content = await _client.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                var mainBody = GetMainBody(doc);

                var climateMyth = GetClimateMythSection(doc);
                var counterArgumentSummary = GetCounterArgumentSummary(doc);

                //TODO
                //var entireText = GetEntireCounterArgumentText(doc);

                argumentData.Add(new ArgumentData
                {
                    ReferenceUrl = url,
                    ArgumentText = climateMyth,
                    CounterArgumentSummary = counterArgumentSummary
                });

                exceptionCounter = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error encountered while scraping '{url}' for data: '{e.Message}'", e);

                exceptionCounter++;

                if(exceptionCounter == 5)
                {
                    Console.WriteLine("Too many successive errors encountered.");
                    break;
                }
            }
        }

        return argumentData;
    }

    private const string MainBodyId = "mainbody";

    private static HtmlNode GetMainBody(HtmlDocument doc)
    {
        return doc.DocumentNode.SelectSingleNode($"//div[@id='{MainBodyId}']");
    }

    private static string GetCounterArgumentSummary(HtmlDocument doc)
    {
        var atAGlanceSummary = GetAtAGlanceSection(doc);
        if (!string.IsNullOrWhiteSpace(atAGlanceSummary))
            return atAGlanceSummary;

        var firstSummary = GetFirstSummarySection(doc);
        if (!string.IsNullOrWhiteSpace(firstSummary))
            return firstSummary;

        return string.Empty;
    }

    private static string GetClimateMythSection(HtmlDocument doc)
    {
        var spanNodes = doc.DocumentNode.SelectNodes("//div[@class='comment myth']");

        return GetInnerTextStartingFromNode(spanNodes, false);
    }

    private const string AtAGlanceHeadingText = "At a glance";

    private static string GetAtAGlanceSection(HtmlDocument doc)
    {
        var spanNodes = SelectNodesByText(doc, "h2", AtAGlanceHeadingText);

        return GetInnerTextStartingFromNode(spanNodes, true);
    }

    private const string FirstSummarySectionHeading = "What the science says...";

    private static string GetFirstSummarySection(HtmlDocument doc)
    {
        var spanNodes = SelectNodesByText(doc, "h2", FirstSummarySectionHeading);

        return GetInnerTextStartingFromNode(spanNodes, true);
    }

    private static string GetInnerTextStartingFromNode(HtmlNodeCollection? spanNodes, bool ignoreFirstNode)
    {
        if (spanNodes is null || !spanNodes.Any())
            return string.Empty;

        if (spanNodes.Count > 1)
            Console.WriteLine($"Found multiple nodes. Will ignore all but the first.");

        var innertexts = new List<string>();

        var currentSpan = ignoreFirstNode ? spanNodes[0].NextSibling : spanNodes[0];

        GetAllInnerTextInSection(currentSpan, innertexts);

        return CleanText(string.Join(" ", innertexts));
    }

    private static void GetAllInnerTextInSection(HtmlNode currentSpan, List<string> innertexts)
    {
        while (currentSpan is not null)
        {
            if (IsInnerTextUseLess(currentSpan))
            {
                currentSpan = currentSpan.NextSibling;
                continue;
            }

            if (IsNewSection(currentSpan))
            {
                break;
            }

            if (IsEndOfArticle(currentSpan))
            {
                Console.WriteLine("Failed to stop before end of entire article");
            }

            innertexts.Add(currentSpan.InnerText);

            currentSpan = currentSpan.NextSibling;
        }

    }

    private static bool IsNewSection(HtmlNode currentSpan)
    {
        return currentSpan == null
                || currentSpan.Name == "hr"
                || currentSpan.Name == "h2"
                || (currentSpan.Name == "p" && currentSpan.GetAttributeValue("class", "").Contains("bluebox"))
                || (currentSpan.Name == "p" && currentSpan.GetAttributeValue("class", "").Contains("bluebox"))
                ;
    }

    private static bool IsEndOfArticle(HtmlNode currentSpan)
    {
        throw new NotImplementedException();
    }

    private static bool IsInnerTextUseLess(HtmlNode currentSpan)
    {
        return string.IsNullOrWhiteSpace(currentSpan.InnerText)
            || currentSpan.InnerText.Equals("\r\n");
    }

    private static HtmlNodeCollection SelectNodesByText(HtmlDocument doc, string element, string atAGlanceHeadingText)
    {
        return doc.DocumentNode.SelectNodes($"//{element}[text()='{AtAGlanceHeadingText}']");
    }

    private static string CleanText(string text)
    {
        if(string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.Replace("\n", " "); // Replaces newline characters with spaces
        text = text.Replace("&nbsp;", " "); // Replaces HTML non-breaking spaces with spaces
        text = Regex.Replace(text, @"\s+", " "); // Removes excessive spaces

        return text.Trim(); // Removes leading and trailing spaces
    }
}
