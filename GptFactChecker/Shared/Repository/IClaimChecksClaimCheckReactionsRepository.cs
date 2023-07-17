using Shared.Models;

namespace Shared.Repository;

public interface IClaimChecksClaimCheckReactionsRepository
{
    public Task<bool> AddClaimCheckReactionForClaimCheck(string claimCheckId, ClaimCheckReaction claim);
    public Task<List<string>> GetClaimCheckReactionsForClaimCheck(string claimCheckId);
    public Task<bool> RemoveClaimCheckReactions(List<string> claimCheckReactionIds);
    public Task<bool> RemoveClaimCheck(string claimCheckId);
}
