using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Services;

public interface IClaimCheckReactionService
{
    public Task<BackendResponse<bool>> AddClaimCheckReaction(ClaimCheckReaction claimCheckReactions, string claimCheckId);
    public Task<bool> DeleteClaimCheckReactions(List<string> claimCheckReactionIds);
    public Task<List<ClaimCheckReaction>> GetClaimCheckReactions(string claimCheckId);
    public Task<bool> DeleteClaimCheckReactionsByClaimCheck(string claimCheck);
}