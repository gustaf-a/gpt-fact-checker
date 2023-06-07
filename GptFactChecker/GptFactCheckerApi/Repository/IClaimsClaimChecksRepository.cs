using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository;

public interface IClaimsClaimChecksRepository
{
    public Task<bool> AddClaimChecksForClaim(string claimId, List<ClaimCheck> claims);
    public Task<List<string>> GetClaimChecksForClaim(string claimId);
    public Task<bool> RemoveClaimChecks(List<string> claimCheckIds);
    public Task<bool> RemoveClaim(string claimId);
}
