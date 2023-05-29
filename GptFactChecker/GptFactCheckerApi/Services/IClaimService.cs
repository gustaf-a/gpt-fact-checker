using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IClaimService
{
    public Task<bool> AddClaims(List<Claim> claims, string sourceId);
    public Task<List<Claim>> GetClaims(string sourceId);
    public Task<List<Claim>> GetClaims(List<string> claimIds);
    public Task<bool> RemoveClaims(List<string> claimIds);
}
