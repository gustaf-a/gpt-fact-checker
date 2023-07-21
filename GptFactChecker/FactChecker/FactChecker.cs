using FactCheckingService.Extensions;
using FactCheckingService.Strategies;
using Shared.Extensions;
using Shared.Models;

namespace FactCheckingService;

public class FactChecker : IFactChecker
{
    private readonly IEnumerable<IFactCheckerStrategy> _factCheckerStrategies;

    public FactChecker(IEnumerable<IFactCheckerStrategy> factCheckerStrategies)
    {
        _factCheckerStrategies = factCheckerStrategies;

        _factCheckerStrategies = _factCheckerStrategies.OrderBy(f => f.Priority);
    }

    public async Task<List<FactCheckResult>> CheckFacts(List<Fact> facts)
    {
        if (facts.IsNullOrEmpty())
            return new();

        var factsToFactCheck = new List<Fact>(facts);

        var factCheckResponses = new List<FactCheckResult>();

        foreach (var factChecker in _factCheckerStrategies)
        {
            var factCheckResults = await factChecker.ExecuteFactCheck(factsToFactCheck);

            factCheckResponses.AddRange(factCheckResults);

            factsToFactCheck.RemoveCheckedFacts(factCheckResults);

            if (!factsToFactCheck.Any())
                break;
        }

        return factCheckResponses;
    }
}
