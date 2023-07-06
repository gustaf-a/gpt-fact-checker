using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/factchecker")]
public class FactCheckerController : ControllerBase
{
    private readonly IFactCheckingService _factCheckingService;

    public FactCheckerController(IFactCheckingService factCheckingService)
    {
        _factCheckingService = factCheckingService;
    }

    /// <summary>
    /// Fact checks claims automatically
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> FactCheckClaims([FromBody] IEnumerable<ClaimDto> claims)
    {
        if (claims.IsNullOrEmpty())
            return StatusCode(400);

        var result = await _factCheckingService.CheckFacts(claims.ToList());

        if (result.IsNullOrEmpty())
            return StatusCode(500);

        return Ok(result);
    }
}
