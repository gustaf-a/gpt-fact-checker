using FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;
using FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;
using FactCheckingService.Utils;
using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Extensions;
using Shared.Models;

namespace FactCheckingService.FactCheckers.ClimateStrategy;

public class ClimateFactCheckerWithReferencesStrategy : IFactCheckerStrategy
{
    private const string ArgumentDataFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData.json";

    private readonly static List<string> CompatibleTags = new() { "climate", "environment", "sustainability", "global warming", "ipcc", "pollution", "emissions", "co2", "temperature", "weather" };

    private readonly ITopicIdentifier _topicIdentifier;
    private readonly IClimateFactCheckerWithData _climateFactCheckerWithData;
    private readonly ITagMatcher _tagMatcher;
    private static List<ArgumentData> _cachedArgumentData;

    public ClimateFactCheckerWithReferencesStrategy(ITopicIdentifier topicIdentifier, IClimateFactCheckerWithData climateFactCheckerWithData, ITagMatcher tagMatcher)
    {
        _topicIdentifier = topicIdentifier;
        _climateFactCheckerWithData = climateFactCheckerWithData;
        _tagMatcher = tagMatcher;
    }

    public int Priority => 1;

    private const string VersionNumber = "1.0";

    public Author Author => new()
    {
        Id = $"FC-ClimateWithReferences-{VersionNumber}",
        Name = $"Climate Fact Checker with references {VersionNumber}",
        IsSystem = true,
        IsVerified = true
    };

    public int CompareTo(IFactCheckerStrategy? other)
    {
        if (other == null)
            return -1;

        return Priority.CompareTo(other.Priority);
    }

    public async Task<List<FactCheckResult>> ExecuteFactCheck(List<Fact> facts)
    {
        var compatibleFacts = facts.Where(IsCompatible).ToList();
        if (compatibleFacts.IsNullOrEmpty())
            return new();

        var result = await DoFactCheck(compatibleFacts);

        return result;
    }

    private async Task<List<FactCheckResult>> DoFactCheck(List<Fact> compatibleFacts)
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

        var factCheckResults = await _climateFactCheckerWithData.GetFactCheckResponses(claimsWithReferences, compatibleFacts, argumentDataList);
        if (factCheckResults.IsNullOrEmpty())
        {
            Console.WriteLine($"Failed to fact check any of the claims using identified references. {nameof(ClimateFactCheckerWithReferencesStrategy)} failed.");
            return new();
        }

        AddAuthorIfChecked(factCheckResults);

        return factCheckResults;
    }

    private void AddAuthorIfChecked(List<FactCheckResult> factCheckResponses)
    {
        foreach (var factCheckResponse in factCheckResponses)
            if (factCheckResponse.IsFactChecked)
                factCheckResponse.Author = Author;
    }

    private static async Task<List<ArgumentData>> GetArgumentDataList()
    {
        if (_cachedArgumentData.IsNullOrEmpty())
            _cachedArgumentData = await JsonHelper.GetObjectFromJson<List<ArgumentData>>(ArgumentDataFilePath);

        return _cachedArgumentData;
    }

    public bool IsCompatible(Fact fact)
    {
        return _tagMatcher.IsMatch(CompatibleTags, fact.Tags.ToList());
    }
}
