using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface ISourceService
{
    public Task<bool> AddSource(Source source, List<Claim> claims);
    public Task<List<SourceDto>> GetSources();
    public Task<SourceDto> GetSourceById(string sourceId);
    public Task<bool> DeleteSource(string sourceId);
}
