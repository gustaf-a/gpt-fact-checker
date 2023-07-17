using Shared.Models;

namespace Shared.Repository;

public interface IClaimCheckReactionRepository
{
    public Task<bool> CreateClaimCheckReaction(ClaimCheckReaction claimCheckReactionToCreate);
    public Task<List<string>> DeleteClaimCheckReactionsForUser(string claimCheckId, string userId);
    public Task<List<ClaimCheckReaction>> GetClaimCheckReactions(List<string>? claimCheckReactionIds);
    public Task<bool> RemoveClaimCheckReactions(List<string> claimCheckReactionIds);
}
