using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IFactExtractorService
{
    Task<BackendResponse<FactExtractionResponse>> ExtractFactsForSource(string id);
}
