using FactCheckingService;
using GptFactCheckerApi.Model;
using Shared.Extensions;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public class FactCheckService : IFactCheckService
{
    private readonly IFactChecker _factChecker;

    private readonly IClaimCheckService _claimCheckService;

    public FactCheckService(IFactChecker factChecker, IClaimCheckService claimCheckService)
    {
        _factChecker = factChecker;
        _claimCheckService = claimCheckService;
    }

    public async Task<List<FactCheckResponse>> CheckFacts(List<ClaimDto> claimDtos)
    {
        var facts = claimDtos.ToClaims();
        if (facts.IsNullOrEmpty())
        {
            Console.WriteLine("Received empty list of claims");
            return null;
        }

        var factCheckResponses = await DoFactChecks(facts);
        if (factCheckResponses.IsNullOrEmpty())
        {
            Console.WriteLine("All attempts at fact checking failed.");
            return null;
        }

        await StoreFactChecks(factCheckResponses);

        return factCheckResponses;
    }

    private async Task<List<FactCheckResponse>> DoFactChecks(List<Fact> facts)
    {
        var factCheckResponses = await _factChecker.CheckFacts(facts);

        var validFactCheckResponses = new List<FactCheckResponse>();

        foreach (var factCheckResponse in factCheckResponses.Where(FactCheckIsValid))
            validFactCheckResponses.Add(factCheckResponse);

        return validFactCheckResponses;
    }

    private static bool FactCheckIsValid(FactCheckResponse? factCheckResponse)
        => factCheckResponse is not null && factCheckResponse.IsChecked && factCheckResponse.FactCheck is not null;

    private async Task StoreFactChecks(List<FactCheckResponse> factCheckResponses)
    {
        foreach (var factCheckResponse in factCheckResponses)
            if (!(await StoreFactCheck(factCheckResponse)))
            {
                factCheckResponse.IsStored = false;
                factCheckResponse.Messages.Add("Failed to store fact check");
                continue;
            }
    }
    
    private async Task<bool> StoreFactCheck(FactCheckResponse factCheckResponse)
    {
        if (!factCheckResponse.IsChecked)
            return false;

        if (factCheckResponse?.FactCheck is null)
        {
            Console.WriteLine($"Invalid FactCheckResponse received. IsChecked is true, but FactCheck is null.");
            return false;
        }

        if (factCheckResponse.Fact is null)
        {
            Console.WriteLine($"Missing Fact in FactCheckResponse. Impossible to store fact check.");
            return false;
        }

        var claimCheck = ConvertToClaimCheck(factCheckResponse);
        var claimChecks = new List<ClaimCheck> { claimCheck };

        if (!await _claimCheckService.AddClaimChecks(claimChecks.ToDtos(), factCheckResponse.Fact.Id))
        {
            Console.WriteLine("Failed to store ClaimCheck");
            return false;
        }

        return true;
    }

    private static ClaimCheck ConvertToClaimCheck(FactCheckResponse factCheckResponse)
    {
        if (factCheckResponse.Author is null)
            Console.WriteLine("Invalid author registered with created fact checks.");

        var factCheck = factCheckResponse.FactCheck;

        return new ClaimCheck
        {
            Id = factCheck.Id,
            UserId = factCheckResponse.Author is null ? "" : factCheckResponse.Author.Id,
            ClaimCheckText = factCheck.FactCheckText,
            Label = factCheck.Label,
            References = factCheck.References,
            DateCreated = factCheck.DateCreated.ToIsoString(),
        };
    }
}
