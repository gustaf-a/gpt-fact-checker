using Shared.Models;

namespace FactCheckingService.Strategies;

public abstract class FactCheckerStrategyBase : IFactCheckerStrategy
{
    public abstract int Priority { get; }

    public int CompareTo(IFactCheckerStrategy? other)
    {
        if (other == null)
            return -1;

        return Priority.CompareTo(other.Priority);
    }

    public abstract Task<List<FactCheckResult>> ExecuteFactCheck(List<Fact> facts);
}
