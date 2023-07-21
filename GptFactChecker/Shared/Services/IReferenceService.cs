using Shared.Models;

namespace Shared.Services;

public interface IReferenceService
{
    public Task<bool> CreateReference(Reference reference, string topicId);
    public Task<bool> Delete(string id);
    public Task<List<Reference>> GetReferencesByTopic(string topicId);
}
