using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;

namespace GptFactCheckerApi.Services;

public class SourceService : ISourceService
{
    private readonly IClaimService _claimService;
    private readonly ISourceRepository _sourceRepository;
    private readonly ISourcesClaimsRepository _sourcesClaimsRepository;

    public SourceService(ISourceRepository sourceRepository, IClaimService claimService, ISourcesClaimsRepository sourcesClaimsRepository)
    {
        _sourceRepository = sourceRepository;
        _claimService = claimService;
        _sourcesClaimsRepository = sourcesClaimsRepository;
    }

    public async Task<bool> AddSource(Source source, List<Claim> claims)
    {
        var result = await _sourceRepository.CreateSource(source);

        if (claims.Any())
            await _claimService.AddClaims(claims, source.Id);

        return result;
    }

    public async Task<bool> DeleteSource(string sourceId)
    {
        var claims = await _sourcesClaimsRepository.GetClaimsForSource(sourceId);

        var deleteClaimsResult = await _claimService.RemoveClaims(claims);

        var deleteSourcesClaimsResult = await _sourcesClaimsRepository.RemoveSource(sourceId);

        var deleteSourceResult = await _sourceRepository.DeleteAsync(sourceId);

        return deleteSourceResult && deleteSourcesClaimsResult && deleteClaimsResult;
    }

    public async Task<SourceDto> GetSourceById(string sourceId)
    {
        var source = await _sourceRepository.GetByIdAsync(sourceId);

        var claimIds = await _sourcesClaimsRepository.GetClaimsForSource(sourceId);

        var claims = await _claimService.GetClaims(claimIds);

        var sourceDto = source.ToDto();

        sourceDto.Claims = claims.ToDtos();

        return sourceDto;
    }

    public async Task<List<SourceDto>> GetSources()
    {
        var sources = await _sourceRepository.GetAllAsync();

        return sources.ToDtos();
    }
}
