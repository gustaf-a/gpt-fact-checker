using FactCheckingService.Models;
using Shared.Models;

namespace FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;

public interface ITopicIdentifier
{
    public Task<List<ClaimWithReferences>> GetClaimsWithReferences(List<Fact> compatibleFacts, List<ArgumentData> argumentDataList);
}
