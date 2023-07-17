using GptFactCheckerApi.Model;
using JsonClient;

namespace RepositoryJson;

public class ParentChildrenHolderJsonRepository : IParentChildrenHolderRepository
{
    private readonly string _jsonFilePath;

    public ParentChildrenHolderJsonRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }

    public async Task<bool> AddChildrenForParent(string parentId, List<string> childIds)
    {
        var parentChildrenHolders = await JsonHelper.GetObjectFromJson<List<ParentChildrenHolder>>(_jsonFilePath);

        var parentChildrenHolder = GetParentChildrenHolder(parentId, parentChildrenHolders);

        if (parentChildrenHolder is null)
        {
            parentChildrenHolder = new ParentChildrenHolder
            {
                ParentId = parentId,
                ChildIds = new()
            };

            parentChildrenHolders.Add(parentChildrenHolder);
        }

        foreach (var childId in childIds)
            if (!parentChildrenHolder.ChildIds.Contains(childId))
                parentChildrenHolder.ChildIds.Add(childId);

        await JsonHelper.SaveToJson(parentChildrenHolders, _jsonFilePath);

        return true;
    }

    public async Task<List<string>> GetChildrenForParent(string parentId)
    {
        var parentChildrenHolders = await JsonHelper.GetObjectFromJson<List<ParentChildrenHolder>>(_jsonFilePath);

        var parentChildrenHolder = GetParentChildrenHolder(parentId, parentChildrenHolders);

        return parentChildrenHolder?.ChildIds ?? new List<string>();
    }

    public async Task<bool> RemoveChildren(List<string> childIds)
    {
        var parentChildrenHolders = await JsonHelper.GetObjectFromJson<List<ParentChildrenHolder>>(_jsonFilePath);

        foreach (var parentChildrenHolder in parentChildrenHolders)
            parentChildrenHolder.ChildIds.RemoveAll(childIds.Contains);

        await JsonHelper.SaveToJson(parentChildrenHolders, _jsonFilePath);

        return true;
    }

    public async Task<bool> RemoveParent(string parentId)
    {
        var parentChildrenHolders = await JsonHelper.GetObjectFromJson<List<ParentChildrenHolder>>(_jsonFilePath);

        var parentChildrenHolder = GetParentChildrenHolder(parentId, parentChildrenHolders);

        if (parentChildrenHolder is null)
            return true;

        parentChildrenHolders.Remove(parentChildrenHolder);

        await JsonHelper.SaveToJson(parentChildrenHolders, _jsonFilePath);

        return true;
    }

    private static ParentChildrenHolder? GetParentChildrenHolder(string parentId, List<ParentChildrenHolder> parentChildrenHolders)
    {
        if (string.IsNullOrWhiteSpace(parentId))
            throw new ArgumentNullException(nameof(parentId), "invalid argument for parentId");

        if (parentChildrenHolders is null)
            return null;

        return parentChildrenHolders.Find(cc => parentId.Equals(cc.ParentId));
    }
}
