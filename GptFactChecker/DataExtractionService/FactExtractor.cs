using FactExtractionService.FactExtractors;
using FactExtractionService.Models;
using Shared.Extensions;
using Shared.Models;
using Shared.Utils;

namespace FactExtractionService;

public class FactExtractor : IFactExtractor
{
    private readonly IFactExtractionStrategy _factExtractionStrategy;

    public FactExtractor(IFactExtractionStrategy factExtractionStrategy)
    {
        _factExtractionStrategy = factExtractionStrategy;
    }

    public async Task<List<Fact>> ExtractFacts(Source source)
    {
        if(source is null)
            throw new ArgumentNullException("source");

        List<ExtractedClaims> extractedClaims = await _factExtractionStrategy.ExtractFacts(source);

        var facts = ConvertToFacts(extractedClaims);

        return facts;
    }

    private static List<Fact> ConvertToFacts(List<ExtractedClaims> extractedClaims)
    {
        if (extractedClaims.IsNullOrEmpty())
            return new();

        return extractedClaims.Select(claim => new Fact
        {
            Id = IdGeneration.GenerateStringId(),
            ClaimRawText = claim.ClaimRawText,
            ClaimSummarized = claim.ClaimSummarized,
            Tags = claim.Tags
        }).ToList();
    }
}
