using GptFactCheckerApi.Model;
using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/claimchecks")]
public class ClaimCheckController : ControllerBase
{
    private readonly IClaimCheckService _claimCheckService;

    public ClaimCheckController(IClaimCheckService claimService)
    {
        _claimCheckService = claimService;
    }

    /// <summary>
    /// Creates ClaimChecks for a claim
    /// </summary>
    [HttpPost("claim/id")]
    public async Task<IActionResult> CreateClaimChecks([FromQuery] string claimId, [FromBody] ClaimCheckDto[] claimChecksDtos)
    {
        var result = await _claimCheckService.AddClaimChecks(claimChecksDtos.ToList(), claimId);

        if (!result)
            return StatusCode(500);

        return Ok();
    }

    /// <summary>
    /// Creates ClaimChecks for multiple claims
    /// </summary>
    [HttpPost("claimcheckresults")]
    public async Task<IActionResult> CreateClaimCheckFromResults([FromBody] ClaimCheckResultsDto[] claimCheckResultsDtos)
    {
        var result = await _claimCheckService.AddClaimCheckResults(claimCheckResultsDtos.ToList());

        if (!result.IsSuccess)
            return StatusCode(500);

        return Ok();
    }

    /// <summary>
    /// Returns all claims as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllClaimChecks()
    {
        var claims = await _claimCheckService.GetAllClaimChecks();

        return Ok(claims);
    }

    /// <summary>
    /// Returns all claims related to a claimId
    /// </summary>
    [HttpGet("claim/id")]
    public async Task<IActionResult> GetAllClaimChecksBySource([FromQuery] string claimId)
    {
        if (string.IsNullOrWhiteSpace(claimId))
            return NotFound();

        var claims = await _claimCheckService.GetClaimChecks(claimId, true);

        return Ok(claims);
    }

    /// <summary>
    /// Deletes a claim matching the provided claimId
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteClaimCheckById([FromQuery] string id)
    {
        var result = await _claimCheckService.DeleteClaimChecks(new List<string>() { id });

        if (!result.IsSuccess)
            return StatusCode(500);

        return Ok();
    }
}