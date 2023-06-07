using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class ClaimsClaimCheckJsonRepository : IClaimsClaimChecksRepository
{
    private const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Claims_ClaimChecks.json";

    private readonly IParentChildrenHolderRepository _parentChildrenHolderJsonRepository;

    public ClaimsClaimCheckJsonRepository()
    {
        _parentChildrenHolderJsonRepository = new ParentChildrenHolderJsonRepository(JsonFilePath);
    }

    public async Task<bool> AddClaimChecksForClaim(string claimId, List<ClaimCheck> claimChecks)
    {
        var claimCheckIds = claimChecks.Select(c => c.Id).ToList();

        return await _parentChildrenHolderJsonRepository.AddChildrenForParent(claimId, claimCheckIds);
    }

    public async Task<List<string>> GetClaimChecksForClaim(string claimId)
    {
        return await _parentChildrenHolderJsonRepository.GetChildrenForParent(claimId);
    }

    public async Task<bool> RemoveClaimChecks(List<string> claimCheckIds)
    {
        return await _parentChildrenHolderJsonRepository.RemoveChildren(claimCheckIds);
    }

    public async Task<bool> RemoveClaim(string claimId)
    {
        return await _parentChildrenHolderJsonRepository.RemoveParent(claimId);
    }
}
