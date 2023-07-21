using Shared.Models;

namespace Shared.Repository;

public interface ITopicReferencesRepository
{
    public Task<bool> AddReferencesForTopic(string topicId, List<Reference> references);
    public Task<List<string>> GetReferencesForTopic(string topicId);
    public Task<bool> RemoveReferences(List<string> referenceIds);
    public Task<bool> RemoveTopic(string topicId);
}
