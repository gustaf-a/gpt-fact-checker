using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository;

public interface IClaimCheckRepository
{
    public Task<bool> CreateClaimChecks(List<ClaimCheck> claimChecksToCreate);
    public Task<List<ClaimCheck>> GetClaimChecks(List<string>? claimCheckIds, bool includeClaimCheckReactions = false);
    public Task<bool> DeleteClaimChecks(List<string> claimCheckIds);
}
