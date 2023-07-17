using Shared.Models;
using FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;
using JsonClient;

namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy.FactCheckWithData;

public class ClimateFactCheckWithDataPromptTests
{
    private const string ArgumentsJson =
"""
[
{
    "Id": 458097,
    "ArgumentText": "Climate's changed before Climate is always changing. We have had ice ages and warmer periods when alligators were found in Spitzbergen. Ice ages have occurred in a hundred thousand year cycle for the last 700 thousand years, and there have been previous periods that appear to have been warmer than the present despite CO2 levels being lower than they are now. More recently, we have had the medieval warm period and the little ice age. (Richard Lindzen)",
    "CounterArgumentSummary": "Claiming 'the climate's changed before' is not a useful statement, as it lacks specifics about how it changed and the impacts. Rapid climate changes, like those caused by human carbon emissions, can lead to severe and potentially disastrous consequences for ecosystems, including humans. Hence, such dismissive statements either stem from ignorance or intentional deception.",
    "CounterArgumentText": "Just imagine for a moment. You fancy having a picnic tomorrow, or you're a farmer needing a dry day to harvest a ripe crop. So naturally, you tune in for a weather-forecast. But what you get is:\u201cHere is the weather forecast. There will be weather today and tomorrow. Good morning.\u201dThat's a fat lot of use, isn't it? The same applies to, \u201cthe climate's changed before\u201d. It's a useless statement. Why? Because it omits details. It doesn't tell you what happened.Climate has indeed changed in the past with various impacts depending on the speed and type of that change. Such results have included everything from slow changes to ecosystems over millions of years - through to sudden mass-extinctions. Rapid climate change, of the type we're causing through our enormous carbon dioxide emissions, falls into the very dangerous camp. That's because the faster the change, the harder it is for nature to cope. We are part of nature so if it goes down, it takes us with it. So anyone who dismissively tells you, \u201cthe climate has always changed\u201d, either does not know what they are talking about or they are deliberately trying to mislead you.",
    "ReferenceUrl": "http://sks.to/past"
},
{
    "Id": 419924,
    "ArgumentText": "It's the sun 'Over the past few hundred years, there has been a steady increase in the numbers of sunspots, at the time when the Earth has been getting warmer. The data suggests solar activity is influencing the global climate causing the world to get warmer.' (BBC)",
    "CounterArgumentSummary": "Our Sun's stability and the relatively minimal fluctuations in solar irradiance demonstrate that it cannot be the primary cause of global warming. Data shows that while solar radiation has slightly declined since the 1970s, global temperatures have continued to rise, indicating other factors, like greenhouse gases, are at play. Claims attributing global warming to the Sun often involve 'cherry-picking' data and ignoring conflicting evidence.",
    "CounterArgumentText": "Thankfully for us, our Sun is a very average kind of star. That means it behaves stably over billions of years, steadily consuming its hydrogen fuel in the nuclear reaction that produces sunshine. Solar stability, along with the Greenhouse Effect, combine to give our planet a habitable range of surface temperatures. In contrast, less stable stars can vary a lot in their radiation output. That lack of stability can prevent life, as we know it, from evolving on any planets that might orbit such stars. That the Sun is a stable type of star is clearly demonstrated by the amount of Solar energy reaching Earth's average orbital position: it varies very little at all. This quantity, called the Total Solar Irradiance, has been measured for around forty years with high accuracy by sensitive instruments aboard satellites. Its average value is 1,362 watts per square metre. Irradiance fluctuates by about a watt either way, depending on where we are within the 11-year long sunspot cycle. That's a variation of no more than 0.15%. From the early 1970s until today, the Solar radiation reaching the top of Earth's atmosphere has in fact shown a very slight decline. Through that same period, global temperatures have continued to increase. The two data records, incoming Solar energy and global temperature, have diverged. That means they have gone in opposite directions. If incoming Solar energy has decreased while the Earth continues to warm up, the Sun cannot be the control-knob of that warming. Attempts to blame the sun for the rise in global temperatures have had to involve taking the data but selecting only the time periods that support such an argument. The remaining parts of the information - showing that divergence - have had to be ditched. Proper science study requires that all the available data be considered, not just a part of it. This particular sin is known as \u201ccherry-picking\u201d.",
    "ReferenceUrl": "http://sks.to/sun"
},
{
    "Id": 654985,
    "ArgumentText": "There is no consensus. 97% of scientists is based on one discredited study.",
    "CounterArgumentSummary": "Scientific consensus, when the majority of experts agree on a principle, is the product of thorough research and examination. With regards to climate change, there's a strong consensus (greater than 97%) that carbon dioxide traps heat and its increase warms the planet. Public misunderstanding of this consensus is often due to the spread of misinformation.",
    "CounterArgumentText": "What is consensus? In science, it's when the vast majority of specialists agree about a basic principle. Thus, astronomers agree that the Earth orbits around the Sun. Biologists accept that tadpoles hatch out from frog-spawn and grow into adult frogs. Almost all geologists agree that plate tectonics is real and you'd be hard-placed to find a doctor who thinks smoking is harmless. In each above case, something has been so thoroughly looked into that those who specialise in its study have stopped arguing about its basic explanation. Nevertheless, the above examples were all once argued about, often passionately. That's how progress works. The reaching of scientific consensus is the product of an often lengthy time-line. It starts with something being observed and ends with it being fully explained. Let's look at a classic and highly relevant example. In the late 1700s, the Earth-Sun distance was calculated. The value obtained was 149 million kilometres. That's incredibly close to modern measurements. It got French physicist Joseph Fourier thinking. He innocently asked, in the 1820s, something along these lines: 'Why is Planet Earth such a warm place? It should be an ice-ball at this distance from the Sun.' Such fundamental questions about our home planet are as attractive to inquisitive scientists as ripened fruit is to wasps. Fourier's initial query set in motion a process of research. Within a few decades, that research had experimentally shown that carbon dioxide has heat-trapping properties. Through the twentieth century the effort intensified, particularly during the Cold War. At that time there was great interest in the behaviour of infra-red (IR) radiation in the atmosphere. Why? Because heat-seeking missiles home in on jet exhausts which are IR hotspots. Their invention involved understanding what makes IR tick. That research led to the publication of a landmark 1956 paper by Gilbert Plass. The paper's title was, \u201cThe Carbon Dioxide Theory of Climatic Change\u201d. It explained in detail how CO2 traps heat in our atmosphere. Note in passing that Plass used the term 'Climatic Change' all the way back then. That's contrary to the deniers' frequent claim that it is used nowadays because of a recent and motivated change in terminology. From observation to explanation, this is a classic illustration of the scientific method at work. Fourier gets people thinking, experiments are designed and performed. In time, a hypothesis emerges. That is a proposed explanation. It is made on the basis of limited evidence as a starting point for further investigation. Once a hypothesis is proposed, it becomes subject to rigorous testing within the relevant specialist science groups. Testing ensures that incorrect hypotheses fall by the wayside, because they don't stand up to scrutiny. But some survive such interrogation. As their supporting evidence mounts up over time, they eventually graduate to become theories. Theories are valid explanations for things that are supported by an expert consensus of specialists. Gravity, jet aviation, electronics, you name it, all are based on solid theories. They are known to work because they have stood the test of time and prolonged scientific inquiry. In climate science today, there is overwhelming (greater than 97%) expert consensus that CO2 traps heat and adding it to the atmosphere warms the planet. Whatever claims are made to the contrary, that principle has been established for almost seventy years, since the publication of that 1955 landmark paper. Expert consensus is a powerful thing. None of us have the time or ability to learn about everything/ That's why we frequently defer to experts, such as consulting doctors when we\u2019re ill. The public often underestimate the degree of expert consensus that our vast greenhouse gas emissions trap heat and warm the planet. That is because alongside information, we have misinformation. Certain sections of the mass-media are as happy to trot out the latter as the former.",
    "ReferenceUrl": "http://sks.to/consensus"
}
]
""";

    [Fact]
    public async Task GetPrompt_should_return_correct_prompt()
    {
        // Arrange
        var climateFactCheckDataPrompt = new ClimateFactCheckWithDataPrompt();

        var fact = new Fact
        {
            Id = "50",
            ClaimRawText = "The sun is getting warmer and causing climate change.",
            Tags = new[] { "climate" }
        };

        var relevantArguments = JsonHelper.Deserialize<List<ArgumentData>>(ArgumentsJson);

        // Act
        
        var result = await climateFactCheckDataPrompt.GetPrompt(fact, relevantArguments);

        // Assert
        Assert.NotNull(result);

        var serialized = JsonHelper.Serialize(result, includeNullValues: false);

        Assert.NotNull(serialized);
    }
}
