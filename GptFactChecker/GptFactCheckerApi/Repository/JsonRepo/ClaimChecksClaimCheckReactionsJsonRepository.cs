using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class ClaimChecksClaimCheckReactionsJsonRepository : IClaimChecksClaimCheckReactionsRepository
{
    private const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\ClaimChecks_ClaimCheckReactions.json";

    private readonly IParentChildrenHolderRepository _parentChildrenHolderJsonRepository;

    public ClaimChecksClaimCheckReactionsJsonRepository()
    {
        _parentChildrenHolderJsonRepository = new ParentChildrenHolderJsonRepository(JsonFilePath);
    }

    public async Task<bool> AddClaimCheckReactionForClaimCheck(string claimCheckId, ClaimCheckReaction claimCheckReaction)
    {
        return await _parentChildrenHolderJsonRepository.AddChildrenForParent(claimCheckId, new List<string> { claimCheckReaction.Id });
    }

    public async Task<List<string>> GetClaimCheckReactionsForClaimCheck(string claimCheckId)
    {
        return await _parentChildrenHolderJsonRepository.GetChildrenForParent(claimCheckId);
    }

    public async Task<bool> RemoveClaimCheck(string claimCheckId)
    {
        return await _parentChildrenHolderJsonRepository.RemoveParent(claimCheckId);
    }

    public async Task<bool> RemoveClaimCheckReactions(List<string> claimCheckReactionIds)
    {
        return await _parentChildrenHolderJsonRepository.RemoveChildren(claimCheckReactionIds);
    }
}
