using FactCheckingService.Extensions;
using FactCheckingService.Strategies.ClimateStrategy;
using FactCheckingService.Strategies.GeneralStrategy.FactCheckPrompt;
using FactCheckingService.Models;
using Shared.Extensions;
using Shared.Models;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using GptHandler.GptClient;

namespace FactCheckingService.Strategies.GeneralStrategy;

public class FactCheckerStrategyGeneral : FactCheckerStrategyBase
{
    private readonly FactCheckerOptions _factCheckerOptions;

    private readonly IGeneralFactCheckPrompt _generalFactCheckPrompt;
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;

    public FactCheckerStrategyGeneral(IOptions<FactCheckerOptions> options,IGeneralFactCheckPrompt generalFactCheckPrompt, IGptClient gptClient, IGptResponseParser gptResponseParser)
    {
        _factCheckerOptions = options.Value;

        _generalFactCheckPrompt = generalFactCheckPrompt;
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
    }

    public override int Priority => 100;

    private const string VersionNumber = "1.0";

    public Author Author => new()
    {
        Id = $"FC-General-{VersionNumber}",
        Name = $"General Fact Checker {VersionNumber}",
        IsSystem = true,
        IsVerified = true
    };

    public override async Task<List<FactCheckResult>> ExecuteFactCheck(List<Fact> facts)
    {
        var results = new List<FactCheckResult>();

        if (facts.IsNullOrEmpty())
            return results;

        if (!_factCheckerOptions.AllowGeneralFactCheck)
            return results;

        foreach (Fact fact in facts)
        {
            var factCheckPrompt = _generalFactCheckPrompt.GetPrompt(fact);
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

            results.Add(new FactCheckResult
            {
                Fact = fact,
                FactCheck = factCheck,
                IsFactChecked = true,
                Author = Author,
                Messages = CreateFactCheckMessage()
            });
        }
        
        return results;
    }

    private async Task<FactCheck> DoFactCheck(Prompt factCheckPrompt)
    {
        var factCheckGptResponse = await _gptClient.GetCompletion(factCheckPrompt);

        var gptResponseFunctionCallFactCheck = _gptResponseParser.ParseGptResponseFunctionCall<FactCheckResponse>(factCheckGptResponse, nameof(FactCheckerStrategyGeneral));

        var factCheck = gptResponseFunctionCallFactCheck.ConvertToFactCheck();

        return factCheck;
    }

    private List<string> CreateFactCheckMessage()
    {
        return new() { $"Fact checked automatically using: {nameof(ClimateFactCheckerWithReferencesStrategy)}." };
    }
}
