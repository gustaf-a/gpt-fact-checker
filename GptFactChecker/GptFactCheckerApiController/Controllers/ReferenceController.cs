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
    /// Creates references related to a topic
    /// </summary>
    [HttpPost("topic/id/batch")]
    public async Task<IActionResult> CreateReferenceForTopic([FromQuery] string topicId, [FromBody] List<Reference> references)
    {
        var response = await _referenceService.CreateReferences(references, topicId);

        if (!response)
            return StatusCode(500);

        return Ok();
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
    /// Returns all topics as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var references = await _referenceService.GetAll();

        return Ok(references);
    }

    /// <summary>
    /// Returns a reference matching the provided id
    /// </summary>
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var reference = await _referenceService.GetById(id);

        if (reference is null)
            return NotFound();

        return Ok(reference);
    }

    /// <summary>
    /// Returns a reference matching the provided id
    /// </summary>
    [HttpGet("topic/id")]
    public async Task<IActionResult> GetByTopicId([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var reference = await _referenceService.GetReferencesByTopic(id);

        if (reference is null)
            return NotFound();

        return Ok(reference);
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
