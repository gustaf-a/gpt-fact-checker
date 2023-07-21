using Shared.Models;

namespace Shared.Services;

public interface ITopicService
{
    public Task<bool> Add(Topic topic);
    public Task<List<Topic>> GetAll(bool includeReferences = false);
    public Task<Topic> GetById(string id, bool includeReferences = false);
    public Task<bool> Delete(string id);

}
