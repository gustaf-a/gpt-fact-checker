using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/references")]
public class ReferenceController : ControllerBase
{
    private readonly IReferenceService _referenceService;

    public ReferenceController(IReferenceService referenceService)
    {
        _referenceService = referenceService;
    }

    /// <summary>
    /// Creates a reference related to a topic
    /// </summary>
    [HttpPost("topic/id")]
    public async Task<IActionResult> CreateReferenceForTopic([FromQuery] string topicId, [FromBody] Reference reference)
    {
        var response = await _referenceService.CreateReference(reference, topicId);

        if (!response)
            return StatusCode(500);

        return Ok();
    }

    /// <summary>
    /// Deletes a reference matching the provided ID
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteReferenceById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var result = await _referenceService.Delete(id);

        if (!result)
            return NotFound();

        return Ok();
    }
}
