using Shared.Models;

namespace FactCheckingService.Services;

public interface ITopicService
{
    public Task<List<Topic>> GetAllTopics(bool includeReferences);
}
