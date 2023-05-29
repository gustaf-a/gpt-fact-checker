using GptFactCheckerApi.Model;
using Newtonsoft.Json;

namespace GptFactCheckerApi.Repository;

public class SourceRepository : ISourceRepository
{
    private const string SourcesJsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Sources.json";

    public async Task<bool> CreateSource(Source source)
    {
        var existingSources = await GetSources();

        if (existingSources.Any(s => s.Id == source.Id))
            return false;

        existingSources.Add(source);

        await SaveSources(existingSources);

        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existingSources = await GetSources();

        var source = existingSources.FirstOrDefault(s => s.Id == id);

        if (source is null)
            return false;

        existingSources.Remove(source);

        await SaveSources(existingSources);

        return true;
    }

    public async Task<List<Source>> GetAllAsync()
    {
        var existingSources = await GetSources();

        if(existingSources is null)
        {
            existingSources = new List<Source>();

            await SaveSources(existingSources);
        }

        return existingSources;
    }

    public async Task<Source> GetByIdAsync(string id)
    {
        var existingSources = await GetSources();

        if (existingSources is null)
        {
            existingSources = new List<Source>();

            await SaveSources(existingSources);

            return default;
        }

        var matchingSource = existingSources.Find(s => s.Id == id);

        return matchingSource;
    }

    private static async Task<List<Source>> GetSources()
    {
        var sourcesJson = await File.ReadAllTextAsync(SourcesJsonFilePath);

        return JsonConvert.DeserializeObject<List<Source>>(sourcesJson);
    }

    private static async Task SaveSources(List<Source> existingSources)
    {
        var updatedSourcesJson = JsonConvert.SerializeObject(existingSources);

        await File.WriteAllTextAsync(SourcesJsonFilePath, updatedSourcesJson);
    }
}
