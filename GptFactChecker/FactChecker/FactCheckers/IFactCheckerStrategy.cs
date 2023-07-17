using Shared.Models;

namespace FactCheckingService.FactCheckers;

public interface IFactCheckerStrategy : IComparable<IFactCheckerStrategy>
{
    int Priority { get; }

    public bool IsCompatible(Fact fact);
    public Task<List<FactCheckResult>> ExecuteFactCheck(List<Fact> facts);
    public Author Author { get; }
}
