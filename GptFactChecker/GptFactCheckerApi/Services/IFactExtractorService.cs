using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IFactExtractorService
{
    Task<FactExtractionResponse> ExtractFactsForSource(string id);
}
