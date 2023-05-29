using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;

namespace GptFactCheckerApi.Services;

public class ClaimService : IClaimService
{
    private readonly IClaimRepository _claimRepository;
    private readonly ISourcesClaimsRepository _sourcesClaimsRepository;

    public ClaimService(IClaimRepository claimRepository, ISourcesClaimsRepository sourcesClaimsRepository)
    {
        _claimRepository = claimRepository;
        _sourcesClaimsRepository = sourcesClaimsRepository;
    }

    public async Task<bool> AddClaims(List<Claim> claims, string sourceId)
    {
        if(!await _claimRepository.CreateClaims(claims))
            return false;

        await _sourcesClaimsRepository.AddClaimsForSource(sourceId, claims);

        return true;
    }

    public async Task<List<Claim>> GetClaims(string sourceId)
    {
        var claimIds = await _sourcesClaimsRepository.GetClaimsForSource(sourceId);

        return await _claimRepository.GetClaims(claimIds);
    }

    public async Task<List<Claim>> GetClaims(List<string> claimIds)
    {
        return await _claimRepository.GetClaims(claimIds);
    }

    public async Task<bool> RemoveClaims(List<string> claimIds)
    {
        return await _claimRepository.RemoveClaims(claimIds);
    }
}
