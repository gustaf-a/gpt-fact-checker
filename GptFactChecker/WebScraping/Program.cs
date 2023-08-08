using JsonClient;
using Shared.Models;
using Shared.Utils;
using WebScraping.SkepticalScience;

namespace WebScraping
{
    internal class Program
    {
        private const string ClimateArgumentDataJsonFolder = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\";
        private const string ClimateArgumentDataFileName = @"climateArgumentData";
        private const string FileExtension = @".json";

        private static string ClimateArgumentDataJsonFilePath { get => ClimateArgumentDataJsonFolder + ClimateArgumentDataFileName + FileExtension; }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting scraping");

            var scraper = new SkepticalScienceScraper();

            var argumentData = await scraper.ScrapeUrls(Urls);

            Console.WriteLine("Finished scraping. Saving results");

            await SaveArgumentDataToFile(argumentData);

            Console.WriteLine("Results saved in: " + ClimateArgumentDataJsonFilePath);
        }

        private static async Task SaveArgumentDataToFile(List<ArgumentData> arguments)
        {
            try
            {
                var existingArgumentData = await JsonHelper.GetObjectFromJson<List<ArgumentData>>(ClimateArgumentDataJsonFilePath);

                foreach (var argument in arguments)
                {
                    var existingArgument = existingArgumentData
                        .FirstOrDefault(a => a.ArgumentText == argument.ArgumentText || a.ReferenceUrl == argument.ReferenceUrl);

                    if (existingArgument != null)
                        existingArgumentData.Remove(existingArgument);

                    existingArgumentData.Add(argument);
                }

                await JsonHelper.SaveToJson(existingArgumentData, ClimateArgumentDataJsonFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception encountered when trying to save data obtained through scraping. Saving data separately");

                await SaveDataInSeparateFile(arguments);
            }
        }

        private static async Task SaveDataInSeparateFile(List<ArgumentData> arguments)
        {
            var newPath = ClimateArgumentDataJsonFolder + ClimateArgumentDataFileName + "temp" + FileExtension;

            await JsonHelper.SaveToJson(arguments, newPath);
        }

        private static readonly List<string> Urls = new()
        {
            "http://sks.to/past",
            "http://sks.to/sun",
            "http://sks.to/consensus",
            "http://sks.to/cooling",
            "http://sks.to/model",
            "http://sks.to/temp",
            "http://sks.to/1998",
            "http://sks.to/1970s",
            "http://sks.to/iceage",
            "http://sks.to/antarctica",
            "http://sks.to/lag",
            "http://sks.to/impacts",
            "http://sks.to/gore",
            "http://sks.to/cosmic",
            "http://sks.to/cold",
            "http://sks.to/hurricane",
            "http://sks.to/1934",
            "http://sks.to/hockey",
            "http://sks.to/mars",
            "http://sks.to/uhi",
            "http://sks.to/1500cycle",
            "http://sks.to/arcticcycle",
            "http://sks.to/sealevel",
            "http://sks.to/vapor",
            "http://sks.to/planets",
            "http://sks.to/green",
            "http://sks.to/oceanheat",
            "http://sks.to/climategate",
            "http://sks.to/co2",
            "http://sks.to/sensitivity",
            "http://sks.to/greenhouse",
            "http://sks.to/lia",
            "http://sks.to/midcentury",
            "http://sks.to/glacier",
            "http://sks.to/pre1940",
            "http://sks.to/evidence",
            "http://sks.to/bear",
            "http://sks.to/troposphere",
            "http://sks.to/kilimanjaro",
            "http://sks.to/underestimat",
            "http://sks.to/extreme",
            "http://sks.to/pollutant",
            "http://sks.to/correlate",
            "http://sks.to/greenland",
            "http://sks.to/pastco2",
            "http://sks.to/weather",
            "http://sks.to/hotspot",
            "http://sks.to/neptune",
            "http://sks.to/jupiter",
            "http://sks.to/pdo",
            "http://sks.to/saturate",
            "http://sks.to/pluto",
            "http://sks.to/icecollapse",
            "http://sks.to/volcano",
            "http://sks.to/species",
            "http://sks.to/mwp",
            "http://sks.to/co2data",
            "http://sks.to/ocean",
            "http://sks.to/elnino",
            "http://sks.to/arctic",
            "http://sks.to/schulte",
            "http://sks.to/dropped",
            "http://sks.to/scl",
            "http://sks.to/aerosols",
            "http://sks.to/microsite",
            "http://sks.to/residence",
            "http://sks.to/shift",
            "http://sks.to/thermo",
            "http://sks.to/significant",
            "http://sks.to/warming",
            "http://sks.to/falsify",
            "http://sks.to/landuse",
            "http://sks.to/settled",
            "http://sks.to/winter",
            "http://sks.to/methane",
            "http://sks.to/500",
            "http://sks.to/decline",
            "http://sks.to/himalaya",
            "http://sks.to/acid",
            "http://sks.to/seapredict",
            "http://sks.to/oreskes",
            "http://sks.to/1995",
            "http://sks.to/icemelt",
            "http://sks.to/oceanco2",
            "http://sks.to/agw",
            "http://sks.to/co2up",
            "http://sks.to/albedo",
            "http://sks.to/snowfall",
            "http://sks.to/hansen1988",
            "http://sks.to/lindzenchoi",
            "http://sks.to/acrim",
            "http://sks.to/solarcycle",
            "http://sks.to/spring",
            "http://sks.to/oism",
            "http://sks.to/watersun",
            "http://sks.to/amazon",
            "http://sks.to/chaos",
            "http://sks.to/stratosphere",
            "http://sks.to/searetract",
            "http://sks.to/mauna",
            "http://sks.to/driver",
            "http://sks.to/trenberth",
            "http://sks.to/ordovician",
            "http://sks.to/southice",
            "http://sks.to/amplify",
            "http://sks.to/stopped",
            "http://sks.to/diverge",
            "http://sks.to/hulme",
            "http://sks.to/cfc",
            "http://sks.to/volcanodrop",
            "http://sks.to/co2conc",
            "http://sks.to/bright",
            "http://sks.to/warmco2",
            "http://sks.to/ozone",
            "http://sks.to/microwave",
            "http://sks.to/industrial",
            "http://sks.to/icefraction",
            "http://sks.to/toocold",
            "http://sks.to/waste",
            "http://sks.to/plant",
            "http://sks.to/greatlake",
            "http://sks.to/runaway",
            "http://sks.to/trace",
            "http://sks.to/overestimate",
            "http://sks.to/breath",
            "http://sks.to/iceloss",
            "http://sks.to/royalsoc",
            "http://sks.to/dmi",
            "http://sks.to/35percent",
            "http://sks.to/humidity",
            "http://sks.to/cycle",
            "http://sks.to/fewdegrees",
            "http://sks.to/toohard",
            "http://sks.to/economy",
            "http://sks.to/baseload",
            "http://sks.to/cloud",
            "http://sks.to/tamper",
            "http://sks.to/peerreview",
            "http://sks.to/ipccskeptic",
            "http://sks.to/foi",
            "http://sks.to/bleach",
            "http://sks.to/futurecool",
            "http://sks.to/stomata",
            "http://sks.to/name",
            "http://sks.to/soares",
            "http://sks.to/seaice",
            "http://sks.to/linear",
            "http://sks.to/snowcover",
            "http://sks.to/atoll",
            "http://sks.to/inertia",
            "http://sks.to/trends",
            "http://sks.to/10000",
            "http://sks.to/ipccmwp",
            "http://sks.to/ljungqvist",
            "http://sks.to/limits",
            "http://sks.to/highway",
            "http://sks.to/removeco2",
            "http://sks.to/urgent",
            "http://sks.to/soot",
            "http://sks.to/poor",
            "http://sks.to/arctic1940",
            "http://sks.to/expensive",
            "http://sks.to/sealevelrise",
            "http://sks.to/venus",
            "http://sks.to/variable",
            "http://sks.to/limitscool",
            "http://sks.to/searise",
            "http://sks.to/jobs",
            "http://sks.to/gbr",
            "http://sks.to/negspencer",
            "http://sks.to/loehle",
            "http://sks.to/salby",
            "http://sks.to/co2increase",
            "http://sks.to/postma",
            "http://sks.to/CERN",
            "http://sks.to/slr2010",
            "http://sks.to/underground",
            "http://sks.to/galileo",
            "http://sks.to/northwest",
            "http://sks.to/stepfunction",
            "http://sks.to/tuvalu",
            "http://sks.to/best",
            "http://sks.to/planetary",
            "http://sks.to/schmittner",
            "http://sks.to/iris",
            "http://sks.to/uah",
            "http://sks.to/santer",
            "http://sks.to/accelerate",
            "http://sks.to/ipccnatural",
            "http://sks.to/thermostat",
            "http://sks.to/survived",
            "http://sks.to/himalayagrow",
            "http://sks.to/pal",
            "http://sks.to/nuclear",
            "http://sks.to/seaice1940",
            "http://sks.to/pastarctic",
            "http://sks.to/arcticstorm",
            "http://sks.to/sandy",
            "http://sks.to/moncktonIPCC",
            "http://sks.to/16years",
            "http://sks.to/longtail",
            "http://sks.to/97percent",
            "http://sks.to/solarminimum",
            "http://sks.to/robust97",
            "http://sks.to/akasofu",
            "http://sks.to/confidence",
            "http://sks.to/projections",
            "http://sks.to/ipccpause",
            "http://sks.to/heatwave",
            "http://sks.to/fringe",
            "http://sks.to/damagecosts",
            "http://sks.to/costs",
            "http://sks.to/meat",
            "http://sks.to/satellite",
            "http://sks.to/money",
            "http://sks.to/holistic",
            "http://sks.to/wildfires",
            "http://sks.to/trees"
        };
    }
}