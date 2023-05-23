using GptFactCheckerApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GptFactCheckerApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/sources")]
public class SourceController : ControllerBase
{
    public SourceController()
    {

    }

    //create source

    /// <summary>
    /// Returns all sources as an unsorted JSON collection
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSources()
    {
        var result = new List<Source>
        {
            new Source
            {
                Id = "aaaa-bbbb",
                Name = "President Trump on Climate Change | AXIOS on HBO",
                Url = "https://www.youtube.com/watch?v=UZq2L_49PBQ",
                SourceType = "video",
                RawText = "",
                ImageUrl = "https://media.npr.org/assets/img/2023/03/31/gettyimages-1413330959-2d4290d11d1e440db6a494c6bf4a76a3caf60727.jpg"
            },
            new Source
            {
                Id = "cccc-dddd",
                Name = "What does Trump actually believe on climate change?",
                Url = "https://www.bbc.com/news/world-us-canada-51213003",
                SourceType = "article",
                RawText = "US President Donald Trump's position on climate change has been in the spotlight again, after he criticised \"prophets of doom\" at the World Economic Forum in Davos.\r\n\r\nAt the event, which had sustainability as its main theme, and activist Greta Thunberg as its star guest, Mr Trump dismissed \"alarmists\" who wanted to \"control every aspect of our lives\" - while also expressing the US's support for an initiative to plant one trillion trees.\r\n\r\nIf you judge the president based on his words alone, his views on climate change appear contradictory - and confusing.\r\n\r\nHe has called climate change \"mythical\", \"nonexistent\", or \"an expensive hoax\" - but also subsequently described it as a \"serious subject\" that is \"very important to me\".\r\n\r\nStill - if you sift through his multitude of tweets and statements, a number of themes emerge.\r\n\r\nIn 2009, Mr Trump actually signed a full-page advert in the New York Times, along with dozens of other business leaders, expressing support for legislation combating climate change.\r\n\r\n\"If we fail to act now, it is scientifically irrefutable that there will be catastrophic and irreversible consequences for humanity and our planet,\" the statement said.\r\n\r\nTrump's speech fact-checked\r\nWhat is climate change?\r\nWhere we are in seven charts\r\nBut in the years that followed, he took an opposite approach on Twitter, with more than 120 posts questioning or making light of climate change.\r\n\r\nIn 2012, he famously said climate change was \"created by and for the Chinese in order to make US manufacturing non-competitive\" - something he later claimed was a joke.\r\n\r\nHe regularly repeated claims that scientists had rebranded global warming as climate change because \"the name global warming wasn't working\" (in fact, both terms are used, but experts at Nasa have argued that climate change is the more scientifically accurate term).\r\n\r\nAnd he also has dozens of tweets suggesting that cold weather disproves climate change - despite the World Meteorological Organization saying that the 20 warmest years on record have been in the past 22 years.",
                ImageUrl = "https://e3.365dm.com/22/11/2048x1152/skynews-donald-trump-midterm-elections_5959207.jpg"
            },
        };

        return Ok(result);
    }

    //get all facts related to a source-id
}
