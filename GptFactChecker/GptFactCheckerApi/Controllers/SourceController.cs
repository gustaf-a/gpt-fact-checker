using GptFactCheckerApi.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/sources")]
public class SourceController : ControllerBase
{
    private const string SourcesJsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Sources.json";

    public SourceController()
    {

    }

    /// <summary>
    /// Creates a source
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateSource([FromBody] SourceDto sourceDto)
    {
        var existingSources = await GetSources();

        existingSources.Add(sourceDto);

        await SaveSources(existingSources);
        
        return Ok();
    }

    /// <summary>
    /// Returns all sources as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSources()
    {
        //Should include claims too?

        var sources = await GetSources();

        return Ok(sources);
    }

    /// <summary>
    /// Returns a source matching the provided sourceId
    /// </summary>
    [HttpGet("id")]
    public async Task<IActionResult> GetSourceById([FromQuery] string id)
    {
        //Should include claims too?

        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var sources = await GetSources();

        var matchingSource = sources.Find(s => s.Id == id);

        if (matchingSource == null)
            return NotFound();

        return Ok(matchingSource);
    }

    /// <summary>
    /// Deletes a source matching the provided sourceId
    /// </summary>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteSourceById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var sources = await GetSources();

        var matchingSource = sources.Find(s => s.Id == id);

        if (matchingSource == null)
            return NotFound();

        sources.Remove(matchingSource);

        await SaveSources(sources);

        return Ok();
    }

    private static async Task<List<SourceDto>> GetSources()
    {
        var sourcesJson = await System.IO.File.ReadAllTextAsync(SourcesJsonFilePath);

        return JsonConvert.DeserializeObject<List<SourceDto>>(sourcesJson);
    }

    private async Task SaveSources(List<SourceDto> existingSources)
    {
        var updatedSourcesJson = JsonConvert.SerializeObject(existingSources);

        await System.IO.File.WriteAllTextAsync(SourcesJsonFilePath, updatedSourcesJson);
    }
}
