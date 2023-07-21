using Shared.GptClient;
using Shared.Models;
using Shared.Extensions;
using Shared.Configuration;
using Microsoft.Extensions.Options;
using FactCheckingService.Models;

namespace FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;

public class TopicIdentifier : ITopicIdentifier
{
    private readonly FactCheckerOptions _factCheckerOptions;
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;
    private readonly ITopicIdentificationPrompt _topicIdentificationPrompt;

    public TopicIdentifier(IOptions<FactCheckerOptions> factCheckerOptions, IGptClient gptClient, IGptResponseParser gptResponseParser, ITopicIdentificationPrompt topicIdentificationPrompt)
    {
        _factCheckerOptions = factCheckerOptions.Value;
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
        _topicIdentificationPrompt = topicIdentificationPrompt;
    }

    public async Task<List<ClaimWithReferences>> GetClaimsWithReferences(List<Fact> compatibleFacts, List<ArgumentData> argumentDataList)
    {
        List<ClaimWithReferences> claimsWithReferences = new();

        var argumentDataParts = argumentDataList.SplitIntoParts(_factCheckerOptions.TopicIdentificationCalls);
        if (argumentDataParts.IsNullOrEmpty())
        {
            Console.WriteLine("Failed to divide facts into parts for topic identification.");
            return claimsWithReferences;
        }

        for (int i = 0; i < _factCheckerOptions.TopicIdentificationCalls; i++)
        {
            var claimsWithPartOfReferences = await GetClaimsWithReferencesInternal(compatibleFacts, argumentDataParts[i]);

            if (claimsWithReferences.IsNullOrEmpty())
            {
                claimsWithReferences = claimsWithPartOfReferences;
                continue;
            }

            foreach (var claimWithPartOfReference in claimsWithPartOfReferences)
            {
                var matchingClaim = claimsWithReferences.FirstOrDefault(c => c.ClaimId == claimWithPartOfReference.ClaimId);

                if (matchingClaim is null)
                    claimsWithReferences.Add(claimWithPartOfReference);
                else
                    matchingClaim.ReferenceIds.AddRange(claimWithPartOfReference.ReferenceIds);
            }
        }

        return claimsWithReferences;
    }

    private async Task<List<ClaimWithReferences>> GetClaimsWithReferencesInternal(List<Fact> compatibleFacts, List<ArgumentData> argumentDataList)
    {
        var topicIdentificationPrompt = await _topicIdentificationPrompt.GetPrompt(compatibleFacts, argumentDataList);

        var topicIdentificationResponse = await _gptClient.GetCompletion(topicIdentificationPrompt);

        var allReturnedClaims = ExtractClaimsWithReferences(topicIdentificationResponse);

        return allReturnedClaims ?? new();
    }

    private List<ClaimWithReferences>? ExtractClaimsWithReferences(string topicIdentificationResponse)
    {
        var topicIdentificationArguments = _gptResponseParser.ParseGptResponseFunctionCall<ReferenceMatchingResponse>(topicIdentificationResponse, nameof(ClimateFactCheckerWithReferencesStrategy));
        if (topicIdentificationArguments is null)
            return null;

        if (topicIdentificationArguments.ClaimsWithReferences.IsNullOrEmpty())
        {
            Console.WriteLine($"Bad response from GPT Client. Unable to deserialize claims with references. {nameof(ClimateFactCheckerWithReferencesStrategy)} failed.");
            return null;
        }

        return topicIdentificationArguments.ClaimsWithReferences;
    }
}
