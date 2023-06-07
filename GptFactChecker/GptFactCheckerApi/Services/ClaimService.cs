using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;

namespace GptFactCheckerApi.Services;

public class ClaimService : IClaimService
{
    private readonly IClaimRepository _claimRepository;
    private readonly ISourcesClaimsRepository _sourcesClaimsRepository;

    private readonly IClaimCheckService _claimCheckService;

    public ClaimService(IClaimRepository claimRepository, ISourcesClaimsRepository sourcesClaimsRepository, IClaimCheckService claimCheckService)
    {
        _claimRepository = claimRepository;
        _sourcesClaimsRepository = sourcesClaimsRepository;
        _claimCheckService = claimCheckService;
    }

    public async Task<bool> AddClaims(List<ClaimDto> claimDtos, string sourceId)
    {
        var claims = claimDtos.ToClaims();

        if(!await _claimRepository.CreateClaims(claims))
            return false;

        await _sourcesClaimsRepository.AddClaimsForSource(sourceId, claims);

        return true;
    }

    public async Task<List<ClaimDto>> GetAllClaims(bool includeClaimChecks = false)
    {
        List<ClaimDto> claimDtos = (await GetClaims(new List<string>()));

        if (includeClaimChecks)
            foreach (var claimDto in claimDtos)
                claimDto.ClaimChecks = await _claimCheckService.GetClaimChecks(claimDto.Id, true);

        return claimDtos;
    }

    public async Task<List<ClaimDto>> GetClaims(string sourceId, bool includeClaimChecks = false)
    {
        var claimIds = await _sourcesClaimsRepository.GetClaimsForSource(sourceId);

        var claimDtos = (await _claimRepository.GetClaims(claimIds)).ToDtos();

        if (includeClaimChecks)
            foreach (var claimDto in claimDtos)
                claimDto.ClaimChecks = await _claimCheckService.GetClaimChecks(claimDto.Id, true);

        return claimDtos;
    }

    public async Task<List<ClaimDto>> GetClaims(List<string> claimIds, bool includeClaimChecks = false)
    {
        var claimDtos = (await _claimRepository.GetClaims(claimIds)).ToDtos();

        if (includeClaimChecks)
            foreach (var claimDto in claimDtos)
                claimDto.ClaimChecks = await _claimCheckService.GetClaimChecks(claimDto.Id, true);

        return claimDtos;
    }

    public async Task<bool> DeleteClaims(List<string> claimIds)
    {
        return await _claimRepository.RemoveClaims(claimIds);
    }
}
