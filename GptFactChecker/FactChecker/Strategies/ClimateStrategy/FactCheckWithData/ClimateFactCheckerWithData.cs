using Shared.GptClient;
using Shared.Models;
using Shared.Extensions;
using FactCheckingService.Models;
using FactCheckingService.Extensions;

namespace FactCheckingService.Strategies.ClimateStrategy.FactCheckWithData;

public class ClimateFactCheckerWithData : IClimateFactCheckerWithData
{
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;
    private readonly IClimateFactCheckWithDataPrompt _climateFactCheckWithDataPrompt;

    public ClimateFactCheckerWithData(IGptClient gptClient, IGptResponseParser gptResponseParser, IClimateFactCheckWithDataPrompt climateFactCheckWithDataPrompt)
    {
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
        _climateFactCheckWithDataPrompt = climateFactCheckWithDataPrompt;
    }

    public async Task<List<FactCheckResult>> GetFactCheckResponses(List<ClaimWithReferences> claimsWithReferences, List<Fact> claimsToCheck, List<ArgumentData> argumentDataList)
    {
        var results = new List<FactCheckResult>();

        foreach (var claimWithRefs in claimsWithReferences)
        {
            if (claimWithRefs.ReferenceIds.IsNullOrEmpty())
                continue;

            var claim = GetCompleteClaim(claimsToCheck, claimWithRefs);
            if (claim is null)
            {
                Console.WriteLine("Error: ClaimId received from GPT Client without corresponding Claim.");
                continue;
            }

            var relevantArguments = GetRelevantReferences(argumentDataList, claimWithRefs);
            if (relevantArguments.IsNullOrEmpty())
            {
                Console.WriteLine($"Error: Failed to find relevant arguments for claim: {claimWithRefs.ClaimId}. Expected argument references: {string.Join(",", claimWithRefs.ReferenceIds)}");
                continue;
            }

            var factCheckPrompt = await _climateFactCheckWithDataPrompt.GetPrompt(claim, relevantArguments);
            if (factCheckPrompt is null)
            {
                Console.WriteLine($"Error: Failed to fact check claim: {claimWithRefs.ClaimId} using argument references: {string.Join(",", claimWithRefs.ReferenceIds)}");
                continue;
            }

            var factCheck = await DoFactCheck(factCheckPrompt);
            if (factCheck is null)
            {
                Console.WriteLine($"Error: Failed to parse fact check from GPT response for: {claimWithRefs.ClaimId} using argument references: {string.Join(",", claimWithRefs.ReferenceIds)}");
                continue;
            }

            factCheck.References = claimWithRefs.ReferenceIds;

            results.Add(new FactCheckResult
            {
                Fact = claim,
                FactCheck = factCheck,
                IsFactChecked = true,
                Messages = CreateFactCheckMessage()
            });
        }

        return results;
    }

    private async Task<FactCheck> DoFactCheck(Prompt factCheckPrompt)
    {
        var gptResponse = await _gptClient.GetCompletion(factCheckPrompt);

        var factCheckResponse = _gptResponseParser.ParseGptResponseFunctionCall<FactCheckResponse>(gptResponse, nameof(ClimateFactCheckerWithData));

        var factCheck = factCheckResponse.ConvertToFactCheck();

        return factCheck;
    }

    private List<string> CreateFactCheckMessage()
    {
        return new() { $"Fact checked automatically using: {nameof(ClimateFactCheckerWithReferencesStrategy)}." };
    }

    private static List<ArgumentData> GetRelevantReferences(List<ArgumentData> argumentDataList, ClaimWithReferences claimWithRefs)
    {
        return argumentDataList.Where(a => claimWithRefs.ReferenceIds.Contains(a.Id.ToString())).ToList();
    }

    private static Fact? GetCompleteClaim(List<Fact> completeClaims, ClaimWithReferences claimWithRefs)
    {
        return completeClaims.FirstOrDefault(f => f.Id == claimWithRefs.ClaimId.ToString());
    }
}
