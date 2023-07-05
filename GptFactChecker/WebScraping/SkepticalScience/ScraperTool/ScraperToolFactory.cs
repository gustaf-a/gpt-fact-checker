using HtmlAgilityPack;

namespace WebScraping.SkepticalScience.ScraperTool;

internal static class ScraperToolFactory
{
    static readonly HttpClient _client = new();

    public static async Task<IScraperTool> CreateScraperTool(string url)
    {
        var content = await _client.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        var scraperBodyTool = new ScraperBodyTool(htmlDocument, url);
        
        return scraperBodyTool;
    }
}
