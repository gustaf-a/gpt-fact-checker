using FactExtractionService;
using GptFactCheckerApi.Model;
using Shared.Extensions;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public class FactExtractorService : IFactExtractorService
{
    private readonly IFactExtractor _factExtractor;

    private readonly IClaimService _claimService;
    private readonly ISourceService _sourceService;

    public FactExtractorService(IFactExtractor factExtractor, IClaimService claimService, ISourceService sourceService)
    {
        _factExtractor = factExtractor;
        _claimService = claimService;
        _sourceService = sourceService;
    }

    public async Task<FactExtractionResponse> ExtractFactsForSource(string id)
    {
        var sourceDto = await _sourceService.GetSourceById(id, false);
        if (sourceDto is null)
        {
            Console.WriteLine($"Failed to retrieve source with id {id}");
            return new FactExtractionResponse
            {
                Message = $"Failed to retrieve source with id {id}",
            };
        }

        var source = sourceDto.ToSource();

        var extractedFacts = await _factExtractor.ExtractFacts(source);
        if (extractedFacts.IsNullOrEmpty())
        {
            Console.WriteLine($"Failed to extract facts from source {source.Id}");
            return null;
        }

        var claimDtos = extractedFacts.ToDtos();

        ComplementClaimsWithInformationFromSource(claimDtos, source);

        var factExtractionResponse = new FactExtractionResponse
        {
            SourceId = source.Id,
            ExtractedClaims = claimDtos
        };

        var storeFactsResult = await StoreFacts(claimDtos, source.Id);
        if (!storeFactsResult)
        {
            factExtractionResponse.IsStored = false;
            factExtractionResponse.Message = $"Failed to save extracted claims to source {source.Id}";
        }

        return factExtractionResponse;
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
