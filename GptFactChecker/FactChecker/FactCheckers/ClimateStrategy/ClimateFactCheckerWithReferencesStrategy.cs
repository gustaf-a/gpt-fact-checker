using FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;
using FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;
using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Extensions;
using Shared.Models;

namespace FactCheckingService.FactCheckers.ClimateStrategy;

public class ClimateFactCheckerWithReferencesStrategy : IFactCheckerStrategy
{
    private const string ArgumentDataFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData.json";

    private readonly static List<string> CompatibleTags = new() { "climate", "environment", "sustainability", "global warming" };

    private readonly ITopicIdentifier _topicIdentifier;
    private readonly IClimateFactCheckerWithData _climateFactCheckerWithData;

    public ClimateFactCheckerWithReferencesStrategy(ITopicIdentifier topicIdentifier, IClimateFactCheckerWithData climateFactCheckerWithData)
    {
        _topicIdentifier = topicIdentifier;
        _climateFactCheckerWithData = climateFactCheckerWithData;
    }

    public int Priority => 1;

    public int CompareTo(IFactCheckerStrategy? other)
    {
        if (other == null)
            return -1;

        return Priority.CompareTo(other.Priority);
    }

    public async Task<List<FactCheckResponse>> ExecuteFactCheck(List<Fact> facts)
    {
        var compatibleFacts = facts.Where(IsCompatible).ToList();
        if (compatibleFacts.IsNullOrEmpty())
            return new();

        var result = await DoFactCheck(compatibleFacts);

        return result;
    }

    private async Task<List<FactCheckResponse>> DoFactCheck(List<Fact> compatibleFacts)
    {
        var argumentDataList = await GetArgumentDataList();
        if (argumentDataList.IsNullOrEmpty())
        {
            Console.WriteLine($"Failed to find valid references. {nameof(ClimateFactCheckerWithReferencesStrategy)} failed.");
            return new();
        }

        var claimsWithReferences = await _topicIdentifier.GetClaimsWithReferences(compatibleFacts, argumentDataList);
        if (claimsWithReferences.IsNullOrEmpty())
        {
            Console.WriteLine($"Failed to find references relevant to claims. {nameof(ClimateFactCheckerWithReferencesStrategy)} failed.");
            return new();
        }

        var factCheckResponses = await _climateFactCheckerWithData.GetFactCheckResponses(claimsWithReferences, compatibleFacts, argumentDataList);
        if (factCheckResponses.IsNullOrEmpty())
        {
            Console.WriteLine($"Failed to fact check any of the claims using identified references. {nameof(ClimateFactCheckerWithReferencesStrategy)} failed.");
            return new();
        }

        return factCheckResponses;
    }

    private static async Task<List<ArgumentData>> GetArgumentDataList()
        => await JsonHelper.GetObjectFromJson<List<ArgumentData>>(ArgumentDataFilePath);

    public bool IsCompatible(Fact fact)
    {
        if (!fact.Tags.Any())
            return false;

        return AllTagsAreCompatible(fact.Tags);
    }

    private static bool AllTagsAreCompatible(string[] tags)
    {
        foreach (var tag in tags)
            if (!CompatibleTags.Any(ct => tag.Contains(ct)))
                return false;

        return true;
    }
}
