using Shared.Models;

namespace Shared.Repository;

public interface ISourceRepository
{
    public Task<bool> CreateSource(Source source);
    public Task<Source> GetByIdAsync(string id);
    public Task<List<Source>> GetAllAsync();
    public Task<bool> DeleteAsync(string id);
}
