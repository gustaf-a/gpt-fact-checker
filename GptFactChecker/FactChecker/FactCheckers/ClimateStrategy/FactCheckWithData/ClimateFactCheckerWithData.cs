﻿using FactCheckingService.FactCheckers.ClimateStrategy.Models;
using Shared.GptClient;
using Shared.Models;
using Shared.Extensions;

namespace FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;

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

    public async Task<List<FactCheckResponse>> GetFactCheckResponses(List<ClaimWithReferences> claimsWithReferences, List<Fact> claimsToCheck, List<ArgumentData> argumentDataList)
    {
        var results = new List<FactCheckResponse>();

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
                Console.WriteLine($"Error: Failed parse fact check from GPT response for: {claimWithRefs.ClaimId} using argument references: {string.Join(",", claimWithRefs.ReferenceIds)}");
                continue;
            }

            results.Add(new FactCheckResponse
            {
                Fact = claim,
                FactCheck = factCheck,
                IsChecked = true,
                Messages = CreateFactCheckMessage()
            });
        }

        return results;
    }

    private async Task<FactCheck> DoFactCheck(Prompt factCheckPrompt)
    {
        var factCheckResponse = await _gptClient.GetCompletion(factCheckPrompt);

        var climateFactCheck = _gptResponseParser.ParseGptResponseFunctionCall<GptResponseFunctionCallFactCheck>(factCheckResponse, nameof(ClimateFactCheckerWithData));

        var factCheck = climateFactCheck.ConvertToFactCheck();

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