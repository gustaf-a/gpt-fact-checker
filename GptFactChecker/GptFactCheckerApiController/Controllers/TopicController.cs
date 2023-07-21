using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/topics")]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    /// <summary>
    /// Creates a topic
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] Topic topic)
    {
        var result = await _topicService.Add(topic);

        if (!result)
            return NotFound();

        return Ok();
    }

    /// <summary>
    /// Returns all topics as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAlltopics()
    {
        var topics = await _topicService.GetAll(includeReferences: false);

        return Ok(topics);
    }

    /// <summary>
    /// Returns a topic matching the provided topicId
    /// </summary>
    [HttpGet("id")]
    public async Task<IActionResult> GetTopicById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var topic = await _topicService.GetById(id, true);

        if (topic is null)
            return NotFound();

        return Ok(topic);
    }

    /// <summary>
    /// Deletes a topic matching the provided topicId
    /// </summary>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteTopicById([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return NotFound();

        var result = await _topicService.Delete(id);

        if (!result)
            return NotFound();

        return Ok();
    }
}
