using Shared.Models;

namespace FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;

public interface ITopicIdentificationPrompt
{
    public Task<Prompt> GetPrompt(List<Fact> factClaims, List<ArgumentData> argumentData);
}
