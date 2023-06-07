using GptFactCheckerApi.Model;
using GptFactCheckerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/claimcheckreactions")]
public class ClaimCheckReactionController : ControllerBase
{
    private readonly IClaimCheckReactionService _claimCheckReactionService;

    public ClaimCheckReactionController(IClaimCheckReactionService claimCheckReactionService)
    {
        _claimCheckReactionService = claimCheckReactionService;
    }

    /// <summary>
    /// Creates a source
    /// </summary>
    [HttpPost("claimcheck/id")]
    public async Task<IActionResult> CreateClaimCheckReaction([FromQuery] string claimCheckId, [FromBody] ClaimCheckReaction claimCheckReaction)
    {
        var result = await _claimCheckReactionService.AddClaimCheckReaction(claimCheckReaction, claimCheckId);

        if (!result)
            return StatusCode(500);

        return Ok();
    }

    /// <summary>
    /// Returns all claim check reactions related to a claimCheckId
    /// </summary>
    [HttpGet("claimcheck/id")]
    public async Task<IActionResult> GetAllClaimCheckReactionsByClaimCheck([FromQuery] string claimCheckId)
    {
        if (string.IsNullOrWhiteSpace(claimCheckId))
            return NotFound();

        var claimCheckReactions = 
            await _claimCheckReactionService.GetClaimCheckReactions(claimCheckId);

        return Ok(claimCheckReactions);
    }

    /// <summary>
    /// Deletes a claim check reaction matching the provided claimCheckReactionId
    /// </summary>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteClaimCheckReactionById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var result = await _claimCheckReactionService.DeleteClaimCheckReactions(new List<string>() { id });

        if (!result)
            return NotFound();

        return Ok();
    }
}
