using Shared.Models;

namespace FactCheckingService;

public interface IFactChecker
{
    public Task<List<FactCheckResult>> CheckFacts(List<Fact> facts);
}
