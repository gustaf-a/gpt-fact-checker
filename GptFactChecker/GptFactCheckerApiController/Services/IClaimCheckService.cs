using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IClaimCheckService
{
    public Task<bool> AddClaimChecks(List<ClaimCheckDto> claimCheckDtos, string claimId);
    public Task<BackendResponse<bool>> AddClaimCheckResults(List<ClaimCheckResultsDto> claimCheckResultsDtos);
    public Task<BackendResponse<bool>> DeleteClaimChecks(List<string> claimCheckIds);
    public Task<List<ClaimCheckDto>> GetAllClaimChecks(bool includeClaimCheckReactions = false);
    public Task<List<ClaimCheckDto>> GetClaimChecks(string claimId, bool includeClaimCheckReactions = false);
}
