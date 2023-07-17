using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimsClaimCheckJsonRepository : IClaimsClaimChecksRepository
{
    private const string FileName = "Claims_ClaimChecks.json";

    private readonly string JsonFilePath;

    private readonly IParentChildrenHolderRepository _parentChildrenHolderJsonRepository;

    public ClaimsClaimCheckJsonRepository(IOptions<RepositoryJsonOptions> options)
    {
        JsonFilePath = options.Value.DataFolder + FileName;

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
