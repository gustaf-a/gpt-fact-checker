namespace RepositoryJson;

public interface IParentChildrenHolderRepository
{
    public Task<bool> AddChildrenForParent(string parentId, List<string> childIds);
    public Task<List<string>> GetChildrenForParent(string parentId);
    public Task<bool> RemoveChildren(List<string> childIds);
    public Task<bool> RemoveParent(string parentId);
}
