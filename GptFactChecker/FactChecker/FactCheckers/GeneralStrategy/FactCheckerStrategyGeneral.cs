using FactCheckingService.FactCheckers.ClimateStrategy;
using FactCheckingService.FactCheckers.GeneralStrategy.FactCheckPrompt;
using Shared.Extensions;
using Shared.GptClient;
using Shared.Models;

namespace FactCheckingService.FactCheckers.GeneralStrategy;

public class FactCheckerStrategyGeneral : IFactCheckerStrategy
{
    private readonly IGeneralFactCheckPrompt _generalFactCheckPrompt;
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;

    public FactCheckerStrategyGeneral(IGeneralFactCheckPrompt generalFactCheckPrompt, IGptClient gptClient, IGptResponseParser gptResponseParser)
    {
        _generalFactCheckPrompt = generalFactCheckPrompt;
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
    }

    public int Priority => 100;

    private const string VersionNumber = "1.0";

    public Author Author => new()
    {
        Id = $"FC-General-{VersionNumber}",
        Name = $"General Fact Checker {VersionNumber}",
        IsSystem = true,
        IsVerified = true
    };

    public int CompareTo(IFactCheckerStrategy? other)
    {
        if (other == null)
            return -1;

        return Priority.CompareTo(other.Priority);
    }

    public async Task<List<FactCheckResponse>> ExecuteFactCheck(List<Fact> facts)
    {
        var results = new List<FactCheckResponse>();

        if (facts.IsNullOrEmpty())
            return results;

        foreach (Fact fact in facts)
        {
            var factCheckPrompt = await _generalFactCheckPrompt.GetPrompt(fact);
            if (factCheckPrompt is null)
            {
                Console.WriteLine($"Error: Failed to fact check claim: {fact.Id}. Failed to create prompt in {nameof(FactCheckerStrategyGeneral)}.");
                continue;
            }

            var factCheck = await DoFactCheck(factCheckPrompt);
            if (factCheck is null)
            {
                Console.WriteLine($"Error: Failed to retrieve fact check from GPT response in {nameof(FactCheckerStrategyGeneral)} for fact: {fact.Id}");
                continue;
            }

            results.Add(new FactCheckResponse
            {
                Fact = fact,
                FactCheck = factCheck,
                IsChecked = true,
                Author = Author,
                Messages = CreateFactCheckMessage()
            });
        }
        
        return results;
    }

    private async Task<FactCheck> DoFactCheck(Prompt factCheckPrompt)
    {
        var factCheckGptResponse = await _gptClient.GetCompletion(factCheckPrompt);

        var gptResponseFunctionCallFactCheck = _gptResponseParser.ParseGptResponseFunctionCall<GptResponseFunctionCallFactCheck>(factCheckGptResponse, nameof(FactCheckerStrategyGeneral));

        var factCheck = gptResponseFunctionCallFactCheck.ConvertToFactCheck();

        return factCheck;
    }

    private List<string> CreateFactCheckMessage()
    {
        return new() { $"Fact checked automatically using: {nameof(ClimateFactCheckerWithReferencesStrategy)}." };
    }

    public bool IsCompatible(Fact fact)
    {
        return true;
    }
}
