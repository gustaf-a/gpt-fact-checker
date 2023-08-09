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
        var result =  await _claimService.AddClaims(claimsDtosToCreate.ToList(), sourceId);

        if(!result)
            return StatusCode(500);

        return Ok();
    }

    /// <summary>
    /// Returns all claims as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllClaims()
    {
        var claims = await _claimService.GetAllClaims();

        return Ok(claims);
    }

    /// <summary>
    /// Returns all claims related to a sourceId
    /// </summary>
    [HttpGet("source/id")]
    public async Task<IActionResult> GetAllClaimsBySource([FromQuery] string sourceId)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
            return NotFound();

        var claims = await _claimService.GetClaims(sourceId, true);

        return Ok(claims);
    }

    /// <summary>
    /// Updates a claim
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateClaim([FromBody] ClaimDto claimDto)
    {
        var result = await _claimService.UpdateClaim(claimDto);

        if (!result)
            return NotFound();

        return Ok();
    }

    /// <summary>
    /// Deletes a claim matching the provided claimId
    /// </summary>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteClaimById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var result = await _claimService.DeleteClaims(new List<string>(){ id });

        if (!result)
            return NotFound();

        return Ok();
    }
}