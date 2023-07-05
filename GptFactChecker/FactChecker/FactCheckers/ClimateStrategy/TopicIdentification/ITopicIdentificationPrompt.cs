using Shared.Models;

namespace FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;

public interface ITopicIdentificationPrompt
{
    public Task<Prompt> GetPrompt(List<Fact> factClaims, List<ArgumentData> argumentData);
}
