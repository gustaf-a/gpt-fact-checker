using Shared.Extensions;
using Shared.Models;
using Shared.Repository;

namespace Shared.Services;

public class ReferenceService : IReferenceService
{
    private readonly IReferenceRepository _referenceRepository;
    private readonly ITopicReferencesRepository _topicReferencesRepository;

    public ReferenceService(IReferenceRepository referenceRepository, ITopicReferencesRepository topicReferencesRepository)
    {
        _referenceRepository = referenceRepository;
        _topicReferencesRepository = topicReferencesRepository;
    }

    public async Task<bool> CreateReference(Reference reference, string topicId)
    {
        if (!await _referenceRepository.Create(reference))
            return false;

        return true;
    }

    public async Task<bool> Delete(string id)
    {
        return await _referenceRepository.Delete(id);
    }

    public async Task<List<Reference>> GetReferencesByTopic(string topicId)
    {
        var referenceIds = await _topicReferencesRepository.GetReferencesForTopic(topicId);
        if (referenceIds.IsNullOrEmpty())
            return new();

        var references = await _referenceRepository.Get(referenceIds);

        if (references is null)
            return new();

        return references;
    }
}
