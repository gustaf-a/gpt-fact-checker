using FactExtractionService;
using GptFactCheckerApi.Model;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public class FactExtractorService : IFactExtractorService
{
    private readonly FactExtractionOptions _factExtractionOptions;

    private readonly IFactExtractor _factExtractor;
    private readonly IClaimService _claimService;
    private readonly ISourceService _sourceService;

    public FactExtractorService(IOptions<FactExtractionOptions> options, IFactExtractor factExtractor, IClaimService claimService, ISourceService sourceService)
    {
        _factExtractionOptions = options.Value;
        _factExtractor = factExtractor;
        _claimService = claimService;
        _sourceService = sourceService;
    }

    public async Task<BackendResponse<FactExtractionResponse>> ExtractFactsForSource(string id)
    {
        var response = new BackendResponse<FactExtractionResponse>
        {
            Data = new FactExtractionResponse()
            {
                SourceId = id
            }
        };

        var sourceDto = await _sourceService.GetSourceById(id, false);
        if (sourceDto is null)
        {
            LogError(response, $"Failed to retrieve source with id {id}");
            return response;
        }

        var source = sourceDto.ToSource();

        var extractedFacts = await _factExtractor.ExtractFacts(source);
        if (extractedFacts.IsNullOrEmpty())
        {
            LogError(response, $"Failed to extract facts from source {source.Id}");
            return response;
        }

        var claimDtos = extractedFacts.ToDtos();
        ComplementClaimsWithInformationFromSource(claimDtos, source);
        if (claimDtos.IsNullOrEmpty())
        {
            LogError(response, $"Failed to convert extracted facts to claims");
            return response;
        }

        response.Data.ExtractedClaims = claimDtos;
        response.IsSuccess = true;

        if (_factExtractionOptions.SaveExtractedFactsDirectlyAfterExtraction)
        {
            var storeFactsResult = await StoreFacts(claimDtos, source.Id);
            if (!storeFactsResult)
                LogError(response, $"Failed to save extracted claims to source {source.Id}");
            else
            {
                LogError(response, $"Saved extracted claims to source {source.Id}");
                response.Data.IsStored = true;
            }
        }

        return response;
    }

    private static void LogError<T>(BackendResponse<T> response, string message)
    {
        Console.WriteLine(message);

        response.Messages.Add(message);
    }

    private static void ComplementClaimsWithInformationFromSource(List<ClaimDto> claimDtos, Source source)
    {
        foreach (var claimDto in claimDtos)
        {
            claimDto.DateCreated = DateTimeOffset.Now.ToIsoString();

            if (claimDto.Tags.IsNullOrEmpty())
                claimDto.Tags = source.Tags.ToArray();
        }
    }

    private async Task<bool> StoreFacts(List<ClaimDto> extractedFacts, string sourceId)
    {
        try
        {
            return await _claimService.AddClaims(extractedFacts, sourceId);
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to save extracted claims to source {sourceId}");
            return false;
        }
    }
}
