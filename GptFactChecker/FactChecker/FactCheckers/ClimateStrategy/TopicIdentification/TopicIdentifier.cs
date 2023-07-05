using FactCheckingService.FactCheckers.ClimateStrategy.Models;
using Shared.GptClient;
using Shared.Models;
using Shared.Extensions;

namespace FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;

public class TopicIdentifier : ITopicIdentifier
{
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;
    private readonly ITopicIdentificationPrompt _topicIdentificationPrompt;

    public TopicIdentifier(IGptClient gptClient, IGptResponseParser gptResponseParser, ITopicIdentificationPrompt topicIdentificationPrompt)
    {
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
        _topicIdentificationPrompt = topicIdentificationPrompt;
    }

    public async Task<List<ClaimWithReferences>> GetClaimsWithReferences(List<Fact> compatibleFacts, List<ArgumentData> argumentDataList)
    {
        var topicIdentificationPrompt = await _topicIdentificationPrompt.GetPrompt(compatibleFacts, argumentDataList);

        var topicIdentificationResponse = await _gptClient.GetCompletion(topicIdentificationPrompt);

        var allReturnedClaims = ExtractClaimsWithReferences(topicIdentificationResponse);

        return allReturnedClaims;
    }

    private List<ClaimWithReferences> ExtractClaimsWithReferences(string topicIdentificationResponse)
    {
        var topicIdentificationArguments = _gptResponseParser.ParseGptResponseFunctionCall<TopicIdentificationFunctionCallArguments>(topicIdentificationResponse, nameof(ClimateFactCheckerWithReferencesStrategy));
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
