using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class SourceJsonRepository : ISourceRepository
{
    private const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Sources.json";

    public async Task<bool> CreateSource(Source source)
    {
        var existingSources = await JsonHelper.GetObjectFromJson<List<Source>>(JsonFilePath);

        if (existingSources.Any(s => s.Id == source.Id))
            return false;

        existingSources.Add(source);

        await JsonHelper.SaveToJson(existingSources, JsonFilePath);

        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existingSources = await JsonHelper.GetObjectFromJson<List<Source>>(JsonFilePath);

        var source = existingSources.FirstOrDefault(s => s.Id == id);

        if (source is null)
            return false;

        existingSources.Remove(source);

        await JsonHelper.SaveToJson(existingSources, JsonFilePath);

        return true;
    }

    public async Task<List<Source>> GetAllAsync()
    {
        var existingSources = await JsonHelper.GetObjectFromJson<List<Source>>(JsonFilePath);

        if (existingSources is null)
        {
            existingSources = new List<Source>();

            await JsonHelper.SaveToJson(existingSources, JsonFilePath);
        }

        return existingSources;
    }

    public async Task<Source> GetByIdAsync(string id)
    {
        var existingSources = await JsonHelper.GetObjectFromJson<List<Source>>(JsonFilePath);

        if (existingSources is null)
        {
            existingSources = new List<Source>();

            await JsonHelper.SaveToJson(existingSources, JsonFilePath);

            return default;
        }

        var matchingSource = existingSources.Find(s => s.Id == id);

        return matchingSource;
    }
}
