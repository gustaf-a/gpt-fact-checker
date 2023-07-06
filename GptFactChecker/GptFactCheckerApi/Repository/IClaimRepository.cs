using Shared.Models;

namespace GptFactCheckerApi.Repository;

public interface IClaimRepository
{
    public Task<bool> CreateClaims(List<Fact> claimsToCreate);
    public Task<List<Fact>> GetClaims(List<string>? claimIds);
    public Task<bool> RemoveClaims(List<string> claimIds);
}
