using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IClaimService
{
    public Task<bool> AddClaims(List<ClaimDto> claims, string sourceId);
    public Task<List<ClaimDto>> GetAllClaims(bool includeClaimChecks = false);
    public Task<List<ClaimDto>> GetClaims(string sourceId, bool includeClaimChecks = false);
    public Task<List<ClaimDto>> GetClaims(List<string> claimIds, bool includeClaimChecks = false);
    public Task<bool> DeleteClaims(List<string> claimIds);
}
