using FactExtractionService.Models;
using Shared.Models;

namespace FactExtractionService.FactExtractors;

public interface IFactExtractionStrategy
{
    Task<List<ExtractedClaims>> ExtractFacts(Source source);
}
