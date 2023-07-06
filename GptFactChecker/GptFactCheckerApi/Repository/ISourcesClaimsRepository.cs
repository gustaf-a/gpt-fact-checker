using Shared.Models;

namespace GptFactCheckerApi.Repository;

public interface ISourcesClaimsRepository
{
    public Task<bool> AddClaimsForSource(string sourceId, List<Fact> claims);
    public Task<List<string>> GetClaimsForSource(string sourceId);
    public Task<bool> RemoveClaims(List<string> claimIds);
    public Task<bool> RemoveSource(string sourceId);
}
