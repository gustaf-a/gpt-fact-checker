using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class TopicsReferencesJsonRepository : ITopicReferencesRepository
{
    private const string FileName = "Topic_References.json";

    private readonly string JsonFilePath;

    private readonly IParentChildrenHolderRepository _parentChildrenHolderRepositoryJson;

    public TopicsReferencesJsonRepository(IOptions<RepositoryJsonOptions> options)
    {
        JsonFilePath = options.Value.DataFolder + FileName;

        _parentChildrenHolderRepositoryJson = new ParentChildrenHolderRepositoryJson(JsonFilePath);
    }

    public async Task<bool> AddReferencesForTopic(string parentId, List<Reference> childIds)
    {
        var ids = childIds.Select(c => c.Id).ToList();

        return await _parentChildrenHolderRepositoryJson.AddChildrenForParent(parentId, ids);
    }

    public async Task<List<string>> GetReferencesForTopic(string topicId)
    {
        return await _parentChildrenHolderRepositoryJson.GetChildrenForParent(topicId);
    }

    public async Task<bool> RemoveReferences(List<string> referenceIds)
    {
        return await _parentChildrenHolderRepositoryJson.RemoveChildren(referenceIds);
    }

    public async Task<bool> RemoveTopic(string topicId)
    {
        return await _parentChildrenHolderRepositoryJson.RemoveParent(topicId);
    }
}
