using FactCheckingService;
using GptFactCheckerApi.Model;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public class FactCheckService : IFactCheckService
{
    private readonly FactCheckerOptions _factCheckerOptions;
    private readonly IFactChecker _factChecker;
    private readonly IClaimService _claimService;
    private readonly IClaimCheckService _claimCheckService;

    public FactCheckService(IOptions<FactCheckerOptions> options, IFactChecker factChecker, IClaimService claimService, IClaimCheckService claimCheckService)
    {
        _factCheckerOptions = options.Value;
        _factChecker = factChecker;
        _claimService = claimService;
        _claimCheckService = claimCheckService;
    }

    public async Task<BackendResponse<List<ClaimCheckResultsDto>>> CheckFacts(List<string> claimIds)
    {
        var response = new BackendResponse<List<ClaimCheckResultsDto>>();

        if (_factCheckerOptions.ReturnTestData)
            return GetFakeData();

        if (claimIds.IsNullOrEmpty())
        {
            LogError(response, "No Claim IDs received.");
            return response;
        }

        var claimDtos = await _claimService.GetClaims(claimIds);
        if (claimDtos.IsNullOrEmpty())
        {
            LogError(response, "Failed to retrieve claims.");
            return response;
        }

        if (claimDtos.Count != claimIds.Count)
            LogError(response, $"Retrieved only {claimDtos.Count} claims from {claimIds.Count} IDs.");

        var factCheckResults = await DoFactChecks(claimDtos.ToClaims());
        if (factCheckResults.IsNullOrEmpty())
        {
            LogError(response, "All attempts at fact checking failed.");
            return response;
        }

        if (_factCheckerOptions.SaveFactChecksDirectlyAfterCreation)
            await StoreFactChecks(factCheckResults);

        response.IsSuccess = true;
        response.Data = factCheckResults.ToDtos();

        return response;
    }

    private static void LogError<T>(BackendResponse<T> response, string message)
    {
        Console.WriteLine(message);

        response.Messages.Add(message);
    }

    private async Task<List<FactCheckResult>> DoFactChecks(List<Fact> facts)
    {
        var factCheckResponses = await _factChecker.CheckFacts(facts);

        var validFactCheckResponses = new List<FactCheckResult>();

        foreach (var factCheckResponse in factCheckResponses.Where(FactCheckIsValid))
            validFactCheckResponses.Add(factCheckResponse);

        return validFactCheckResponses;
    }

    private static bool FactCheckIsValid(FactCheckResult? factCheckResponse)
        => factCheckResponse is not null && factCheckResponse.IsFactChecked && factCheckResponse.FactCheck is not null;

    private async Task StoreFactChecks(List<FactCheckResult> factCheckResponses)
    {
        foreach (var factCheckResponse in factCheckResponses)
        {
            if (!(await StoreFactCheck(factCheckResponse)))
            {
                factCheckResponse.Messages.Add("Failed to store fact check");
                continue;
            }

            factCheckResponse.IsStored = true;
        }
    }

    private async Task<bool> StoreFactCheck(FactCheckResult factCheckResponse)
    {
        if (!factCheckResponse.IsFactChecked)
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

    private static ClaimCheck ConvertToClaimCheck(FactCheckResult factCheckResponse)
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

    private BackendResponse<List<ClaimCheckResultsDto>> GetFakeData()
    {
        var data = new List<FactCheckResult>();

        data.Add(new FactCheckResult
        {
            Fact = new Fact
            {
                Id = "123",
                ClaimRawText = "Test 123",
                ClaimSummarized = "Test 123 sum",
            },
            FactCheck = new FactCheck
            {
                Id = "123461",
                Label = "True",
                FactCheckText = "It's true",
                DateCreated = DateTime.Now,
                References = new() { "11", "22" }
            },
            IsFactChecked = true
        });

        var result = new BackendResponse<List<ClaimCheckResultsDto>>
        {
            Data = data.ToDtos(),
            IsSuccess = true
        };

        return result;
    }

}
