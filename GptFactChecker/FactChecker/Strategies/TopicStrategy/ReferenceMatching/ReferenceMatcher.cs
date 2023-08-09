using FactCheckingService.Extensions;
using FactCheckingService.Models;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using GptHandler.GptClient;
using Shared.Models;

namespace FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;

public class ReferenceMatcher : IReferenceMatcher
{
    private readonly ReferenceMatchingOptions _referenceMatchingOptions;
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;
    private readonly IReferenceMatcherPromptDirector _referenceMatcherPromptDirector;

    public ReferenceMatcher(IOptions<ReferenceMatchingOptions> options, IGptClient gptClient, IGptResponseParser gptResponseParser, IReferenceMatcherPromptDirector referenceMatcherPromptDirector)
    {
        _referenceMatchingOptions = options.Value;
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
        _referenceMatcherPromptDirector = referenceMatcherPromptDirector;
    }

    public string GetVersionInfo()
    {
        return $"{nameof(ReferenceMatcher)}: {_referenceMatcherPromptDirector.GetVersionInfo()}";
    }

    public async Task<List<ClaimWithReferences>> MatchFactsWithReferences(List<Fact> facts, List<Reference> references)
    {
        var factsParts = facts.SplitByMaxCount(_referenceMatchingOptions.MaxFactsCount);
        if (factsParts.IsNullOrEmpty())
        {
            Console.WriteLine("Failed to divide facts into parts for reference matching.");
            return new();
        }

        var referencesParts = references.SplitByMaxCount(_referenceMatchingOptions.MaxRefCount);
        if (referencesParts.IsNullOrEmpty())
        {
            Console.WriteLine("Failed to divide references into parts for reference matching.");
            return new();
        }

        var claimsWithReferences = new List<ClaimWithReferences>();

        foreach (var factsPart in factsParts)
        {
            foreach (var referencesPart in referencesParts)
            {
                var claimsWithPartOfReferences = await MatchFactsWithReferencesInternal(factsPart, referencesPart);
                if (claimsWithPartOfReferences.IsNullOrEmpty())
                    continue;

                claimsWithReferences = claimsWithReferences.Merge(claimsWithPartOfReferences);
            }
        }

        return claimsWithReferences;
    }

    private async Task<List<ClaimWithReferences>> MatchFactsWithReferencesInternal(List<Fact> factsPart, List<Reference> referencesPart)
    {
        if (factsPart.IsNullOrEmpty()
            || referencesPart.IsNullOrEmpty())
            return new();

        var referenceMatchingPrompt = _referenceMatcherPromptDirector.BuildPrompt(factsPart, referencesPart);

        try
        {
            var gptResponse = await _gptClient.GetCompletion(referenceMatchingPrompt);

            var referenceMatchingResponse = _gptResponseParser.ParseGptResponseFunctionCall<ReferenceMatchingResponse>(gptResponse, nameof(ReferenceMatcher));
            if (referenceMatchingResponse is null)
            {
                Console.WriteLine($"Unable to deserialize claims with references.");
                return new();
            }

            if (referenceMatchingResponse.ClaimsWithReferences.IsNullOrEmpty())
                return new();

            return referenceMatchingResponse.ClaimsWithReferences;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected exception encountered when trying to get response from GPT client. Unable to deserialize claims with references. {nameof(ReferenceMatcher)} failed.", ex);
            return new();
        }
    }
}
