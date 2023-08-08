using FactCheckingService.Models;
using FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;
using JsonClient;
using Shared.GptClient;
using Shared.Models;

namespace FactCheckingService.Strategies.TopicStrategy.RefererenceFactChecking;

public class ReferenceFactChecker : IReferenceFactChecker
{
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;
    private readonly IReferenceFactCheckerPromptDirector _referenceFactCheckerPromptDirector;

    public ReferenceFactChecker(IGptClient gptClient, IGptResponseParser gptResponseParser, IReferenceFactCheckerPromptDirector referenceFactCheckerPromptDirector)
    {
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
        _referenceFactCheckerPromptDirector = referenceFactCheckerPromptDirector;
    }

    public async Task<FactCheckResponse> FactCheck(Fact fact, List<Reference> relevantReferences)
    {
        var factCheckPrompt = _referenceFactCheckerPromptDirector.BuildPrompt(fact, relevantReferences);
        if (factCheckPrompt is null)
        {
            Console.WriteLine($"Error: Failed to fact check claim: {fact.Id} using argument references: {JsonHelper.Serialize(relevantReferences)}");
            return null;
        }

        var factCheckResponse = await DoFactCheck(factCheckPrompt);
        if (factCheckResponse is null)
        {
            Console.WriteLine($"Error: Failed to parse fact check from GPT response for: {fact.Id} using argument references: {JsonHelper.Serialize(relevantReferences)}");
            return null;
        }

        return factCheckResponse;
    }

    public string GetVersionInfo()
    {
        return $"{nameof(ReferenceFactChecker)}: {_referenceFactCheckerPromptDirector.GetVersionInfo()}";
    }

    private async Task<FactCheckResponse> DoFactCheck(Prompt factCheckPrompt)
    {
        try
        {
            var gptResponse = await _gptClient.GetCompletion(factCheckPrompt);

            var factCheckResponse = _gptResponseParser.ParseGptResponseFunctionCall<FactCheckResponse>(gptResponse, nameof(ReferenceFactChecker));

            return factCheckResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected exception encountered when trying to get response from GPT client. Unable to deserialize claims with references. {nameof(ReferenceMatcher)} failed.", ex);
            return null;
        }
    }
}
