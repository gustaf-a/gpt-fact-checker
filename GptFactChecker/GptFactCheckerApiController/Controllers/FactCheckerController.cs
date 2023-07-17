using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/factchecker")]
public class FactCheckerController : ControllerBase
{
    private readonly IFactCheckService _factCheckingService;

    public FactCheckerController(IFactCheckService factCheckingService)
    {
        _factCheckingService = factCheckingService;
    }

    /// <summary>
    /// Fact checks claims automatically
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> FactCheckClaims([FromBody] IEnumerable<string> claimIds)
    {
        var backendResponse = await _factCheckingService.CheckFacts(claimIds.ToList());

        if (!backendResponse.IsSuccess)
            return StatusCode(500, backendResponse);

        return Ok(backendResponse);
    }
}
