using FactCheckingService.Models;
using Shared.Models;

namespace FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;

public interface IClimateFactCheckerWithData
{
    public Task<List<FactCheckResult>> GetFactCheckResponses(List<ClaimWithReferences> claimsWithReferences, List<Fact> claimsToCheck, List<ArgumentData> argumentDataList);
}
