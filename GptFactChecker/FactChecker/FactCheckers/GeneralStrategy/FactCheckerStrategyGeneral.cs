using Shared.Models;

namespace FactCheckingService.FactCheckers.GeneralStrategy;

public class FactCheckerStrategyGeneral : IFactCheckerStrategy
{
    public int Priority => 100;

    public int CompareTo(IFactCheckerStrategy? other)
    {
        if (other == null)
            return -1;

        return Priority.CompareTo(other.Priority);
    }

    public async Task<List<FactCheckResponse>> ExecuteFactCheck(List<Fact> facts)
    {
        throw new NotImplementedException();
    }

    public bool IsCompatible(Fact fact)
    {
        return true;
    }
}
