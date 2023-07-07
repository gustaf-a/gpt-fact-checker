using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/factextractor")]
public class FactExtractorController : ControllerBase
{
    private readonly IFactExtractorService _factExtractorService;

    public FactExtractorController(IFactExtractorService factExtractorService)
    {
        _factExtractorService = factExtractorService;
    }

    /// <summary>
    /// Extract claims from a source matching the provided sourceId
    /// </summary>
    [HttpGet("source/id")]
    public async Task<IActionResult> ExtractClaims([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var factExtractionResponse = await _factExtractorService.ExtractFactsForSource(id);
        if (factExtractionResponse is null)
            return StatusCode(500);

        return Ok(factExtractionResponse);
    }
}
