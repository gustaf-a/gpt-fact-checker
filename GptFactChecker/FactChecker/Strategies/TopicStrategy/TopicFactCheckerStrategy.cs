using FactCheckingService.Extensions;
using FactCheckingService.Strategies.ClimateStrategy;
using FactCheckingService.Models;
using FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;
using FactCheckingService.Strategies.TopicStrategy.RefererenceFactChecking;
using FactCheckingService.Utils;
using JsonClient;
using Shared.Extensions;
using Shared.Models;
using Shared.Services;

namespace FactCheckingService.Strategies.TopicStrategy;

public class TopicFactCheckerStrategy : FactCheckerStrategyBase
{
    private readonly ITopicService _topicService;
    private readonly ITagMatcher _tagMatcher;
    private readonly IReferenceMatcher _referenceMatcher;
    private readonly IReferenceFactChecker _referenceFactChecker;

    private List<Topic> _allTopics;

    public TopicFactCheckerStrategy(ITopicService topicService, ITagMatcher tagMatcher, IReferenceMatcher referenceMatcher, IReferenceFactChecker referenceFactChecker)
    {
        _topicService = topicService;
        _tagMatcher = tagMatcher;
        _referenceMatcher = referenceMatcher;
        _referenceFactChecker = referenceFactChecker;
    }

    public override int Priority => 25;
    private const string VersionNumber = "1.0";

    private const string AuthorId = $"TFC-{VersionNumber}";
    private const string TopicFactCheckerVersionInfo = $"Topic Fact Checker {VersionNumber}";

    public override async Task<List<FactCheckResult>> ExecuteFactCheck(List<Fact> facts)
    {
        _allTopics ??= await _topicService.GetAll(includeReferences: true);
        if (_allTopics.IsNullOrEmpty())
            return new();

        _allTopics.Sort();

        var factsToFactCheck = new List<Fact>(facts);
        var factCheckResult = new List<FactCheckResult>();

        foreach (var topic in _allTopics)
        {
            var compatibleFacts = facts.Where(f => IsCompatible(topic, f)).ToList();
            if (compatibleFacts.IsNullOrEmpty())
                continue;

            var factCheckResults = await DoFactCheck(topic, compatibleFacts);

            factCheckResult.AddRange(factCheckResults);
            factsToFactCheck = factsToFactCheck.RemoveCheckedFacts(factCheckResults);

            if (!factsToFactCheck.Any())
                break;
        }

        return factCheckResult;
    }

    private bool IsCompatible(Topic topic, Fact fact)
    {
        return _tagMatcher.IsMatch(topic.Tags, fact.Tags.ToList());
    }

    private async Task<List<FactCheckResult>> DoFactCheck(Topic topic, List<Fact> claimsToCheck)
    {
        if (!TopicIsValid(topic))
        {
            Console.WriteLine($"Invalid topic encountered: {JsonHelper.Serialize(topic)}");
            return new();
        }

        var claimsWithReferences = await _referenceMatcher.MatchFactsWithReferences(claimsToCheck, topic.References);
        if (claimsWithReferences.IsNullOrEmpty())
        {
            Console.WriteLine($"Failed to find references relevant to claims. {topic.Name} failed.");
            return new();
        }

        var factCheckResults = await FactCheckWithReferences(topic, claimsToCheck, claimsWithReferences);

        return factCheckResults;
    }

    private async Task<List<FactCheckResult>> FactCheckWithReferences(Topic topic, List<Fact> claimsToCheck, List<ClaimWithReferences> claimsWithReferences)
    {
        var factCheckResults = new List<FactCheckResult>();

        foreach (var claimWithReferences in claimsWithReferences)
        {
            var fact = GetClaim(claimsToCheck, claimWithReferences);
            if (fact is null)
            {
                Console.WriteLine("Error: ClaimId received from GPT Client without corresponding Claim.");
                continue;
            }

            var relevantReferences = GetRelevantReferences(topic, claimWithReferences);
            if (relevantReferences.IsNullOrEmpty())
            {
                Console.WriteLine($"Error: Failed to find relevant arguments for claim: {claimWithReferences.ClaimId}. Expected argument references: {string.Join(",", claimWithReferences.ReferenceIds)}");
                continue;
            }

            var factCheckResponse = await _referenceFactChecker.FactCheck(fact, relevantReferences);
            if (factCheckResponse is null)
            {
                Console.WriteLine($"Failed to fact check any of the claims using identified references. {topic.Name} failed.");
                continue;
            }

            var factCheck = factCheckResponse.ConvertToFactCheck();

            var factCheckResult = new FactCheckResult
            {
                Fact = fact,
                FactCheck = factCheck,
                IsFactChecked = true,
                Author = GetAuthor(topic)
            };

            factCheckResult.Messages.Add($"Fact checked automatically using: {nameof(ClimateFactCheckerWithReferencesStrategy)}.");
            //factCheckResult.Author = GetAuthor(topic);

            factCheckResults.Add(factCheckResult);
        }

        return factCheckResults;
    }

    private Author GetAuthor(Topic topic)
    {
        var referenceMatchingVersionInfo = _referenceMatcher.GetVersionInfo();
        var referenceFactCheckerVersionInfo = _referenceFactChecker.GetVersionInfo();

        return new()
        {
            Id = AuthorId,
            Name = $"{TopicFactCheckerVersionInfo} - {referenceMatchingVersionInfo} - {referenceFactCheckerVersionInfo}",
            IsSystem = true,
            IsVerified = true,
            Description = CreateAuthorDescription(topic),
        };
    }

    private static string CreateAuthorDescription(Topic topic)
    {
        return $"Topic Fact Checker using topic: {topic.Name}, {topic.Id}, {topic.Description}.";
    }

    private static bool TopicIsValid(Topic topic)
    {
        if (topic is null)
            return false;

        if (topic.References.IsNullOrEmpty())
            return false;

        if (topic.Tags.IsNullOrEmpty())
            return false;

        if (string.IsNullOrWhiteSpace(topic.Name))
            return false;

        return true;
    }

    private static List<Reference> GetRelevantReferences(Topic topic, ClaimWithReferences claimWithReferences)
    {
        return topic.References.Where(r => claimWithReferences.ReferenceIds.Contains(r.Id)).ToList();
    }

    private static Fact? GetClaim(List<Fact> claimsToCheck, ClaimWithReferences claimWithReferences)
    {
        return claimsToCheck.Where(c => claimWithReferences.ClaimId.Equals(c.Id)).FirstOrDefault();
    }
}
