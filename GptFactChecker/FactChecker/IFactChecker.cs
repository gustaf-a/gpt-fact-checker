using Shared.Models;

namespace FactCheckingService;

public interface IFactChecker
{
    public Task<List<FactCheckResponse>> CheckFacts(List<Fact> facts);
}
