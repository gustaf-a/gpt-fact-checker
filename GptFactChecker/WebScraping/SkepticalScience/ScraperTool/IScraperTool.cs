using Shared.Models;

namespace WebScraping.SkepticalScience.ScraperTool;

internal interface IScraperTool
{
    string Name { get; }
    public ArgumentData GetArgumentData();
}
