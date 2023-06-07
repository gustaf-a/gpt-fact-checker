using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;

namespace GptFactCheckerApi.Services;

public class ClaimCheckReactionService : IClaimCheckReactionService
{
    private readonly IClaimCheckReactionRepository _claimCheckReactionRepository;
    private readonly IClaimChecksClaimCheckReactionsRepository _claimChecksClaimCheckReactionsRepository;

    public ClaimCheckReactionService(IClaimCheckReactionRepository claimCheckReactionRepository, IClaimChecksClaimCheckReactionsRepository claimChecksClaimCheckReactionsRepository)
    {
        _claimCheckReactionRepository = claimCheckReactionRepository;
        _claimChecksClaimCheckReactionsRepository = claimChecksClaimCheckReactionsRepository;
    }

    public async Task<bool> AddClaimCheckReaction(ClaimCheckReaction claimCheckReaction, string claimCheckId)
    {
        var claimCheckReactionsForClaim = await GetClaimCheckReactions(claimCheckId);

        if (claimCheckReactionsForClaim.Any())
            await RemoveClaimCheckReactionsByUser(claimCheckReactionsForClaim, claimCheckReaction.UserId);

        if (!await _claimCheckReactionRepository.CreateClaimCheckReaction(claimCheckReaction))
            return false;

        await _claimChecksClaimCheckReactionsRepository.AddClaimCheckReactionForClaimCheck(claimCheckId, claimCheckReaction);

        return true;
    }

    private async Task RemoveClaimCheckReactionsByUser(List<ClaimCheckReaction> claimCheckReactionsForClaim, string userId)
    {
        var reactionsByUserForClaim = claimCheckReactionsForClaim.FindAll(c => userId.Equals(c.UserId));

        if (claimCheckReactionsForClaim.Any())
        {
            var reactionsByUserForClaimIds = reactionsByUserForClaim.Select(r => r.Id).ToList();

            await DeleteClaimCheckReactions(reactionsByUserForClaimIds);
        }
    }

    public async Task<List<ClaimCheckReaction>> GetClaimCheckReactions(string claimCheckId)
    {
        var claimCheckReactionIds 
            = await _claimChecksClaimCheckReactionsRepository.GetClaimCheckReactionsForClaimCheck(claimCheckId);

        return await _claimCheckReactionRepository.GetClaimCheckReactions(claimCheckReactionIds);
    }

    public async Task<bool> DeleteClaimCheckReactions(List<string> claimCheckReactionIds)
    {
        if (!await _claimChecksClaimCheckReactionsRepository.RemoveClaimCheckReactions(claimCheckReactionIds))
            return false;

        return await _claimCheckReactionRepository.RemoveClaimCheckReactions(claimCheckReactionIds);
    }
}
