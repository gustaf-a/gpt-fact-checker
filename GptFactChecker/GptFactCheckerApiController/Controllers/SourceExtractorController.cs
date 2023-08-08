using GptFactCheckerApi.Model;
using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/sourceextractor")]
public class SourceExtractorController : ControllerBase
{
    private readonly ISourceExtractorService _sourceExtractorService;

    public SourceExtractorController(ISourceExtractorService sourceExtractorService)
    {
        _sourceExtractorService = sourceExtractorService;
    }

    /// <summary>
    /// Returns a source object from an URL
    /// </summary>
    [HttpGet("sourceUrl")]
    public async Task<IActionResult> CreateSourceFromUrl([FromQuery] string sourceUrl)
    {
        var backendResponse = await _sourceExtractorService.CreateSourceFromUrl(sourceUrl);

        if (!backendResponse.IsSuccess)
            return StatusCode(500, backendResponse);

        return Ok(backendResponse);
    }

    /// <summary>
    /// Returns the raw text extracted from the source
    /// </summary>
    [HttpGet("id")]
    public async Task<IActionResult> ExtractRawTextFromSourceById([FromQuery] string id)
    {
        var backendResponse = await _sourceExtractorService.ExtractSource(id);

        if (!backendResponse.IsSuccess)
            return StatusCode(500, backendResponse);

        return Ok(backendResponse);
    }
}
