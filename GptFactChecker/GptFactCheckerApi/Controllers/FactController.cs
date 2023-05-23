using GptFactCheckerApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/facts")]
public class FactController : ControllerBase
{
    //get all facts

    //get facts from one source
    [HttpGet("id")]
    public async Task<IActionResult> GetAllFactsBySource([FromQuery] string sourceId)
    {
        var facts = new List<Fact>
        {
            new Fact {
                Id = "1",
                FactText = "The planet will soon cool down.",
                SourceId = sourceId
            },
            new Fact {
                Id = "2",
                FactText = "The scientists don't know.",
                SourceId = sourceId
            },
            new Fact {
                Id = "5",
                FactText = "A warming earth is not a bad thing.",
                SourceId = sourceId
            },
        };

        return Ok(facts);
    }

}
