using Shared.Extensions;
using Shared.Models;
using Shared.Repository;

namespace Shared.Services;

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IReferenceService _referenceService;
    private readonly ITopicReferencesRepository _topicReferencesRepository;

    public TopicService(ITopicRepository topicRepository, IReferenceService referenceService, ITopicReferencesRepository topicReferencesRepository)
    {
        _topicRepository = topicRepository;
        _referenceService = referenceService;
        _topicReferencesRepository = topicReferencesRepository;
    }

    public async Task<List<Topic>> GetAll(bool includeReferences)
    {
        var topics = await _topicRepository.GetAll();

        if (includeReferences)
            foreach (var topic in topics)
                topic.References = await GetReferencesForTopic(topic);

        return topics;
    }

    private async Task<List<Reference>> GetReferencesForTopic(Topic topic)
    {
        return await _referenceService.GetReferencesByTopic(topic.Id);
    }

    public async Task<bool> Add(Topic topic)
    {
        if (!await _topicRepository.Create(topic))
        {
            Console.WriteLine($"Failed to create topic: {topic.Id}.");
            return false;
        }

        if (topic.References.IsNullOrEmpty())
            return true;

        if (!await _topicReferencesRepository.AddReferencesForTopic(topic.Id, topic.References))
        {
            Console.WriteLine($"Failed to create references for topic: {topic.Id}.");
            return false;
        }

        return true;
    }

    public async Task<Topic> GetById(string id, bool includeReferences = false)
    {
        var topic = await _topicRepository.Get(id);

        if (includeReferences)
            topic.References = await GetReferencesForTopic(topic);

        return topic;
    }

    public async Task<bool> Delete(string id)
    {
        return await _topicRepository.Delete(id);
    }
}
