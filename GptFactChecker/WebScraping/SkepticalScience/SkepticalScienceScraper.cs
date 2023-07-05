using Shared.Models;
using Shared.Utils;
using WebScraping.SkepticalScience.ScraperTool;

namespace WebScraping.SkepticalScience;

internal class SkepticalScienceScraper
{
    public async Task<List<ArgumentData>> ScrapeUrls(List<string> urls)
    {
        Console.WriteLine($"Found {urls.Count} urls to scrape.");

        var argumentDataList = new List<ArgumentData>();

        var failureCounter = 0;

        foreach (var url in urls)
        {
            try
            {
                var scraperTool = await ScraperToolFactory.CreateScraperTool(url);
                
                Console.WriteLine($"Using {scraperTool.Name} to scrape {url}.");

                var argumentData = scraperTool.GetArgumentData();

                if(argumentData is null)
                {
                    Console.WriteLine($"ScraperTool '{scraperTool.Name}' failed to return valid {typeof(ArgumentData)} for {url}. Skipping URL.");

                    failureCounter++;

                    if (TooManySuccessiveFailures(failureCounter))
                        break;

                    continue;
                }

                argumentData.Id = IdGeneration.GenerateIntegerId();

                argumentDataList.Add(argumentData);

                failureCounter = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error encountered while scraping '{url}' for data: '{e.Message}'", e);

                failureCounter++;

                if (TooManySuccessiveFailures(failureCounter))
                    break;
            }
        }

        return argumentDataList;
    }

    private static bool TooManySuccessiveFailures(int failureCounter)
    {
        if (failureCounter > 4)
        {
            Console.WriteLine("Too many successive errors encountered.");
            return true;
        }

        return false;
    }
}
