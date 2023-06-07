using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IClaimCheckService
{
    public Task<bool> AddClaimChecks(List<ClaimCheckDto> claimCheckDtos, string claimId);
    public Task<bool> DeleteClaimChecks(List<string> list);
    public Task<List<ClaimCheckDto>> GetAllClaimChecks(bool includeClaimCheckReactions = false);
    public Task<List<ClaimCheckDto>> GetClaimChecks(string claimId, bool includeClaimCheckReactions = false);
}
