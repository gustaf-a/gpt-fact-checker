using GptFactCheckerApi.Model;
using GptFactCheckerApiController.Model;

namespace GptFactCheckerApi.Services;

public interface ISourceExtractorService
{
    public Task<BackendResponse<SourceCollectingResponseDto>> CreateSourceFromUrl(string sourceUrl);
    public Task<BackendResponse<SourceCollectingResponseDto>> ExtractSource(string id);
}
