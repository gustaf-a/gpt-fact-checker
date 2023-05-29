using GptFactCheckerApi.Model;
using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/claims")]
public class ClaimController : ControllerBase
{
    private readonly IClaimService _claimService;

    public ClaimController(IClaimService claimService)
    {
        _claimService = claimService;
    }

    /// <summary>
    /// Creates a source
    /// </summary>
    [HttpPost("source/id")]
    public async Task<IActionResult> CreateClaims([FromQuery] string sourceId, [FromBody] ClaimDto[] claimsDtosToCreate)
    {
        var result =  await _claimService.AddClaims(claimsDtosToCreate.ToClaims(), sourceId);

        if(!result)
            return StatusCode(500);

        return Ok();
    }

    /// <summary>
    /// Returns all claims related to a sourceId
    /// </summary>
    [HttpGet("source/id")]
    public async Task<IActionResult> GetAllClaimsBySource([FromQuery] string sourceId)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
            return NotFound();

        var claims = await _claimService.GetClaims(sourceId);

        return Ok(claims);
    }

}
