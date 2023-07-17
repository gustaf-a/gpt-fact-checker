using FactCheckingService.FactCheckers;
using Shared.Extensions;
using Shared.Models;

namespace FactCheckingService;

public class FactChecker : IFactChecker
{
    private readonly IEnumerable<IFactCheckerStrategy> _factCheckers;

    public FactChecker(IEnumerable<IFactCheckerStrategy> factCheckers)
    {
        _factCheckers = factCheckers;

        _factCheckers = _factCheckers.OrderBy(f => f.Priority);
    }

    public async Task<List<FactCheckResult>> CheckFacts(List<Fact> facts)
    {
        if (facts.IsNullOrEmpty())
            return new();

        var factsToFactCheck = new List<Fact>(facts);

        var factCheckResponses = new List<FactCheckResult>();

        foreach (var factChecker in _factCheckers)
        {
            var responses = await factChecker.ExecuteFactCheck(factsToFactCheck);

            factCheckResponses.AddRange(responses);

            RemoveCheckedFacts(factsToFactCheck, responses);

            if (!factsToFactCheck.Any())
                break;
        }

        return factCheckResponses;
    }

    private static void RemoveCheckedFacts(List<Fact> facts, List<FactCheckResult> responses)
    {
        var checkedFactIds = responses
            .Where(r => r.IsFactChecked)
            .Select(r => r.Fact.Id)
            .ToList();

        facts.RemoveAll(fact => checkedFactIds.Contains(fact.Id));
    }
}
