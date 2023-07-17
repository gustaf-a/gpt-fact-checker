using GptFactCheckerApi.Model;
using Shared.Repository;

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

    public async Task<bool> AddSource(SourceDto source)
    {
        var result = await _sourceRepository.CreateSource(source.ToSource());

        return result;
    }

    public async Task<bool> DeleteSource(string sourceId)
    {
        var claims = await _sourcesClaimsRepository.GetClaimsForSource(sourceId);

        var deleteClaimsResult = await _claimService.DeleteClaims(claims);

        var deleteSourcesClaimsResult = await _sourcesClaimsRepository.RemoveSource(sourceId);

        var deleteSourceResult = await _sourceRepository.DeleteAsync(sourceId);

        return deleteSourceResult && deleteSourcesClaimsResult && deleteClaimsResult;
    }

    public async Task<SourceDto> GetSourceById(string sourceId, bool includeClaims = false)
    {
        var source = await _sourceRepository.GetByIdAsync(sourceId);

        var sourceDto = source.ToDto();

        if (includeClaims)
        {
            var claimIds = await _sourcesClaimsRepository.GetClaimsForSource(sourceId);

            sourceDto.Claims = await _claimService.GetClaims(claimIds);
        }

        return sourceDto;
    }

    public async Task<List<SourceDto>> GetSources(bool includeClaims = false)
    {
        var sources = await _sourceRepository.GetAllAsync();

        var sourceDtos = sources.ToDtos();

        if (includeClaims)
            foreach (var sourceDto in sourceDtos)
            {
                var claimIds = await _sourcesClaimsRepository.GetClaimsForSource(sourceDto.Id);

                sourceDto.Claims = await _claimService.GetClaims(claimIds);
            }

        return sourceDtos;
    }
}
