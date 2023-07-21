using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimsClaimCheckRepositoryJson : IClaimsClaimChecksRepository
{
    private const string FileName = "Claims_ClaimChecks.json";

    private readonly string JsonFilePath;

    private readonly IParentChildrenHolderRepository _parentChildrenHolderRepositoryJson;

    public ClaimsClaimCheckRepositoryJson(IOptions<RepositoryJsonOptions> options)
    {
        JsonFilePath = options.Value.DataFolder + FileName;

        _parentChildrenHolderRepositoryJson = new ParentChildrenHolderRepositoryJson(JsonFilePath);
    }

    public async Task<bool> AddClaimChecksForClaim(string claimId, List<ClaimCheck> claimChecks)
    {
        var claimCheckIds = claimChecks.Select(c => c.Id).ToList();

        return await _parentChildrenHolderRepositoryJson.AddChildrenForParent(claimId, claimCheckIds);
    }

    public async Task<List<string>> GetClaimChecksForClaim(string claimId)
    {
        return await _parentChildrenHolderRepositoryJson.GetChildrenForParent(claimId);
    }

    public async Task<bool> RemoveClaimChecks(List<string> claimCheckIds)
    {
        return await _parentChildrenHolderRepositoryJson.RemoveChildren(claimCheckIds);
    }

    public async Task<bool> RemoveClaim(string claimId)
    {
        return await _parentChildrenHolderRepositoryJson.RemoveParent(claimId);
    }
}
