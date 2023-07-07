using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GptFactCheckerApi.Repository;

public interface ISourceRepository
{
    public Task<bool> CreateSource(Source source);
    public Task<Source> GetByIdAsync(string id);
    public Task<List<Source>> GetAllAsync();
    public Task<bool> DeleteAsync(string id);
}
