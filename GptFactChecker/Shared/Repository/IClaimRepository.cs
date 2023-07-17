using Shared.Models;

namespace Shared.Repository;

public interface IClaimRepository
{
    public Task<bool> CreateClaims(List<Fact> claimsToCreate);
    public Task<List<Fact>> GetAllClaims();
    public Task<List<Fact>> GetClaims(List<string> claimIds);
    public Task<bool> RemoveClaims(List<string> claimIds);
}
