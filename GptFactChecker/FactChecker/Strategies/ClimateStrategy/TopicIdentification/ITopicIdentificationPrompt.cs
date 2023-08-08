using Shared.Models;

namespace FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;

public interface ITopicIdentificationPrompt
{
    public Prompt GetPrompt(List<Fact> factClaims, List<ArgumentData> argumentData);
}
