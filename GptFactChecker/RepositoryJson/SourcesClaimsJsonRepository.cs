using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class SourcesClaimsRepositoryJson : ISourcesClaimsRepository
{
    private const string FileName = "Sources_Claims.json";

    private readonly string JsonFilePath;

    private readonly IParentChildrenHolderRepository _parentChildrenHolderRepositoryJson;

    public SourcesClaimsRepositoryJson(IOptions<RepositoryJsonOptions> options)
    {
        JsonFilePath = options.Value.DataFolder + FileName;

        _parentChildrenHolderRepositoryJson = new ParentChildrenHolderRepositoryJson(JsonFilePath);
    }

    public async Task<bool> AddClaimsForSource(string sourceId, List<Fact> claims)
    {
        var claimsIds = claims.Select(c => c.Id).ToList();

        return await _parentChildrenHolderRepositoryJson.AddChildrenForParent(sourceId, claimsIds);
    }

    public async Task<List<string>> GetClaimsForSource(string sourceId)
    {
        return await _parentChildrenHolderRepositoryJson.GetChildrenForParent(sourceId);
    }

    public async Task<bool> RemoveClaims(List<string> claimIds)
    {
        return await _parentChildrenHolderRepositoryJson.RemoveChildren(claimIds);
    }

    public async Task<bool> RemoveSource(string sourceId)
    {
        return await _parentChildrenHolderRepositoryJson.RemoveParent(sourceId);
    }
}
