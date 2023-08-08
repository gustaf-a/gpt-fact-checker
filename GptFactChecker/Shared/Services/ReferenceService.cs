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

        await _topicReferencesRepository.AddReferencesForTopic(topicId, new() { reference });

        return true;
    }

    public async Task<bool> CreateReferences(List<Reference> references, string topicId)
    {
        if (!await _referenceRepository.Create(references))
            return false;

        await _topicReferencesRepository.AddReferencesForTopic(topicId, references);

        return true;
    }

    public async Task<bool> Delete(string id)
    {
        return await _referenceRepository.Delete(id);
    }

    public async Task<List<Reference>> GetAll()
    {
        var references = await _referenceRepository.GetAll();
        if (references.IsNullOrEmpty())
            return new();

        return references;
    }

    public async Task<Reference> GetById(string id)
    {
        var refererence = await _referenceRepository.Get(id);

        return refererence;
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
