using GptFactCheckerApi.Model;
using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/sources")]
public class SourceController : ControllerBase
{
    private readonly ISourceService _sourceService;

    public SourceController(ISourceService sourceService)
    {
        _sourceService = sourceService;
    }

    /// <summary>
    /// Creates a source
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateSource([FromBody] SourceDto sourceDto)
    {
        var result = await _sourceService.AddSource(sourceDto.ToSource(), sourceDto.Claims.ToClaims());

        if (!result)
            return NotFound();

        return Ok();
    }

    /// <summary>
    /// Returns all sources as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSources()
    {
        var sources = await _sourceService.GetSources();

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

        var source = await _sourceService.GetSourceById(id);

        if (source is null)
            return NotFound();

        return Ok(source);
    }

    /// <summary>
    /// Deletes a source matching the provided sourceId
    /// </summary>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteSourceById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var result = await _sourceService.DeleteSource(id);

        if (!result)
            return NotFound();

        return Ok();
    }
}
