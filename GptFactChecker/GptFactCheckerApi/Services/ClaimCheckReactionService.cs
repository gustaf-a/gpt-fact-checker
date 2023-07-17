using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;
using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Extensions;

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

    public async Task<BackendResponse<bool>> AddClaimCheckReaction(ClaimCheckReaction claimCheckReaction, string claimCheckId)
    {
        var response = new BackendResponse<bool>();

        var claimCheckReactionsForClaim = await GetClaimCheckReactions(claimCheckId);

        if (claimCheckReactionsForClaim.Any())
            await RemoveClaimCheckReactionsByUser(claimCheckReactionsForClaim, claimCheckReaction.UserId);

        if (!await _claimCheckReactionRepository.CreateClaimCheckReaction(claimCheckReaction))
        {
            response.Messages.Add($"Failed to create claim check reaction: {JsonHelper.Serialize(claimCheckReaction)}");
            return response;
        }

        if(!await _claimChecksClaimCheckReactionsRepository.AddClaimCheckReactionForClaimCheck(claimCheckId, claimCheckReaction))
        {
            response.Messages.Add($"Failed to create claim check to claim check reaction relationship for ClaimCheckId {claimCheckId}: {JsonHelper.Serialize(claimCheckReaction)}");
            return response;
        }

        response.IsSuccess = true;

        return response;
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

    public async Task<bool> DeleteClaimCheckReactionsByClaimCheck(string claimCheck)
    {
        var claimCheckReactionIds = await GetClaimCheckReactionIds(claimCheck);
        if (claimCheckReactionIds.IsNullOrEmpty())
            return false;

        if (!await DeleteClaimCheckReactions(claimCheckReactionIds))
            return false;

        return true;
    }

    public async Task<List<ClaimCheckReaction>> GetClaimCheckReactions(string claimCheckId)
    {
        List<string> claimCheckReactionIds = await GetClaimCheckReactionIds(claimCheckId);

        return await _claimCheckReactionRepository.GetClaimCheckReactions(claimCheckReactionIds);
    }

    private async Task<List<string>> GetClaimCheckReactionIds(string claimCheckId)
    {
        return await _claimChecksClaimCheckReactionsRepository.GetClaimCheckReactionsForClaimCheck(claimCheckId);
    }

    public async Task<bool> DeleteClaimCheckReactions(List<string> claimCheckReactionIds)
    {
        if (!await _claimChecksClaimCheckReactionsRepository.RemoveClaimCheckReactions(claimCheckReactionIds))
            return false;

        return await _claimCheckReactionRepository.RemoveClaimCheckReactions(claimCheckReactionIds);
    }
}
