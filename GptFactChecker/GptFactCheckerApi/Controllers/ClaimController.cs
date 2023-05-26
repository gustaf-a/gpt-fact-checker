using GptFactCheckerApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/facts")]
public class ClaimController : ControllerBase
{
    //get all facts

    /// <summary>
    /// Returns all facts related to a sourceId
    /// </summary>
    [HttpGet("source/id")]
    public async Task<IActionResult> GetAllFactsBySource([FromQuery] string sourceId)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
            return NotFound();

        var facts = new List<Claim>
        {
            //new Claim {
            //    Id = "1",
            //    ClaimText = "The planet will soon cool down.",
            //    SourceId = sourceId,
            //    FactChecks = new []{ new ClaimCheck{
            //        Id = "abcd",
            //        CreatorId = "1000",
            //        Label = "False",
            //        ClaimCheckText = "This is false."
            //    } }
            //},
            //new Claim {
            //    Id = "2",
            //    ClaimText = "The scientists don't know.",
            //    SourceId = sourceId,
            //    FactChecks = new []{ new ClaimCheck{
            //        Id = "hhhh",
            //        CreatorId = "2000",
            //        Label = "Misleading",
            //        ClaimCheckText = "This not really true."
            //    } }
            //},
            //new Claim {
            //    Id = "5",
            //    ClaimText = "A warming earth is not a bad thing.",
            //    SourceId = sourceId,
            //    FactChecks = new []{ new ClaimCheck{
            //        Id = "kkkkk",
            //        CreatorId = "3000",
            //        Label = "Exaggerated",
            //        ClaimCheckText = "This is very false."
            //    } }
            //},
        };

        return Ok(facts);
    }

}
