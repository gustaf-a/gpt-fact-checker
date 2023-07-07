using FactCheckingService.Models;
using Shared.Models;

namespace FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;

public interface ITopicIdentifier
{
    public Task<List<ClaimWithReferences>> GetClaimsWithReferences(List<Fact> compatibleFacts, List<ArgumentData> argumentDataList);
}
