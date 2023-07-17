using GptFactCheckerApi.Model;
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
        var backendResponse = await _factExtractorService.ExtractFactsForSource(id);

        if (!backendResponse.IsSuccess)
            return StatusCode(500, backendResponse);

        return Ok(backendResponse);
    }
}
