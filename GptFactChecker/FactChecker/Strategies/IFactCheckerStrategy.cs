using Shared.Models;

namespace FactCheckingService.Strategies;

public interface IFactCheckerStrategy : IComparable<IFactCheckerStrategy>
{
    int Priority { get; }

    public Task<List<FactCheckResult>> ExecuteFactCheck(List<Fact> facts);
}
