using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class ClaimCheckReactionJsonRepository : IClaimCheckReactionRepository
{
    public const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\ClaimCheckReactions.json";

    public async Task<bool> CreateClaimCheckReaction(ClaimCheckReaction claimCheckReactionToCreate)
    {
        var claimCheckReactions = await JsonHelper.GetObjectFromJson<List<ClaimCheckReaction>>(JsonFilePath);

        claimCheckReactions.Add(claimCheckReactionToCreate);

        await JsonHelper.SaveToJson(claimCheckReactions, JsonFilePath);

        return true;
    }

    public async Task<List<ClaimCheckReaction>> GetClaimCheckReactions(List<string>? claimCheckReactionIds = null)
    {
        return await GetClaimCheckReactionsById(claimCheckReactionIds);
    }

    public async Task<List<string>> DeleteClaimCheckReactionsForUser(string claimCheckId, string userId)
    {
        if (string.IsNullOrWhiteSpace(claimCheckId) || string.IsNullOrWhiteSpace(userId))
            return new();

        var claimCheckReactions = await JsonHelper.GetObjectFromJson<List<ClaimCheckReaction>>(JsonFilePath);

        var foundClaimCheckReactionsForUser = claimCheckReactions.FindAll(c => userId.Equals(c.UserId));

        if (!foundClaimCheckReactionsForUser.Any())
            return new();
        
        var foundClaimCheckReactionsForUserIds = foundClaimCheckReactionsForUser.Select(c => c.Id).ToList();

        if (!await RemoveClaimCheckReactions(foundClaimCheckReactionsForUserIds))
            return new();

        return foundClaimCheckReactionsForUserIds;
    }

    public async Task<bool> RemoveClaimCheckReactions(List<string> claimCheckReactionIds)
    {
        if (claimCheckReactionIds is null || !claimCheckReactionIds.Any())
            return true;

        var claimCheckReactions = await JsonHelper.GetObjectFromJson<List<ClaimCheckReaction>>(JsonFilePath);

        var claimCheckReactionsToKeep = claimCheckReactions.Where(ccr => !claimCheckReactionIds.Contains(ccr.Id)).ToList();

        await JsonHelper.SaveToJson(claimCheckReactionsToKeep, JsonFilePath);

        return true;
    }

    private static async Task<List<ClaimCheckReaction>> GetClaimCheckReactionsById(List<string>? claimCheckReactionIds)
    {
        if (claimCheckReactionIds == null || !claimCheckReactionIds.Any())
            return new List<ClaimCheckReaction>();

        var claimCheckReactions = await JsonHelper.GetObjectFromJson<List<ClaimCheckReaction>>(JsonFilePath);

        if (claimCheckReactions is not null && claimCheckReactionIds.Any())
            claimCheckReactions = claimCheckReactions.Where(ccr => claimCheckReactionIds.Contains(ccr.Id)).ToList();

        return claimCheckReactions ?? new List<ClaimCheckReaction>();
    }
}
