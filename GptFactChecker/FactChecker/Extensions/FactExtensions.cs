using Shared.Models;

namespace FactCheckingService.Extensions;

public static class FactExtensions
{
    public static List<Fact> RemoveCheckedFacts(this List<Fact> facts, List<FactCheckResult> factCheckResults)
    {
        var checkedFactIds = factCheckResults
            .Where(r => r.IsFactChecked)
            .Select(r => r.Fact.Id)
            .ToList();

        facts.RemoveAll(fact => checkedFactIds.Contains(fact.Id));

        return facts;
    }
}
