using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository;

public interface IClaimRepository
{
    public Task<bool> CreateClaims(List<Claim> claimsToCreate);
    public Task<List<Claim>> GetClaims(List<string>? claimIds);
    public Task<bool> RemoveClaims(List<string> claimIds);
}
