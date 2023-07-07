using Shared.Models;

namespace FactExtractionService;

public interface IFactExtractor
{
    Task<List<Fact>> ExtractFacts(Source source);
}
