using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface ISourceService
{
    public Task<bool> AddSource(SourceDto source);
    public Task<List<SourceDto>> GetSources(bool includeClaims = false);
    public Task<SourceDto> GetSourceById(string sourceId, bool includeClaims = false);
    public Task<bool> DeleteSource(string sourceId);
}
