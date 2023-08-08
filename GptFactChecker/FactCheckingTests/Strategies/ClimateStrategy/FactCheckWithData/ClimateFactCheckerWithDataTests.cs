using FactCheckingService.Strategies.ClimateStrategy.FactCheckWithData;
using FactCheckingService.Models;
using Moq;
using Shared.GptClient;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy.FactCheckWithData;

public class ClimateFactCheckerWithDataTests
{
    private const string FactCheckResponse =
"""

""";

    [Fact]
    public async Task GetFactCheckResponses_should_parse_gpt_response()
    {
        // Arrange
        var testPrompt = new Prompt();

        var gptClientMock = new Mock<IGptClient>();
        gptClientMock.Setup(g => g.GetCompletion(It.IsAny<Prompt>(), It.IsAny<double>())).Returns(Task.FromResult(FactCheckResponse));

        var climateFactCheckWithDataPromptMock = new Mock<IClimateFactCheckWithDataPrompt>();
        climateFactCheckWithDataPromptMock.Setup(c => c.GetPrompt(It.IsAny<Fact>(), It.IsAny<List<ArgumentData>>())).Returns(testPrompt);

        var climateFactChecker = new ClimateFactCheckerWithData(gptClientMock.Object, new GptResponseParser(), climateFactCheckWithDataPromptMock.Object);

        var claimsWithReferences = new List<ClaimWithReferences>
        {
            new ClaimWithReferences
            {
                ClaimId = "1",
                ReferenceIds = new List<string> { "51" }
            },
            new ClaimWithReferences
            {
                ClaimId = "3",
                ReferenceIds = new List<string> { "53", "54" }
            }
        };

        var claimsToCheck = new List<Fact>
        {
            new Fact
            {
                Id = "1",
                Tags = new[] { "climate change" },
                ClaimSummarized = "The sun is warming."
            },
            new Fact
            {
                Id = "2",
                Tags = new[] { "nutrition" },
                ClaimSummarized = "Test fact 2"
            },
            new Fact
            {
                Id = "3",
                Tags = new[] { "climate change" },
                ClaimSummarized = "The scientists don't agree."
            }
        };

        var argumentData = new List<ArgumentData>
        {
            new ArgumentData
            {
                Id = 51,
                ArgumentText = "test 51",
                CounterArgumentSummary = "test counter argument 51 ",
                ReferenceUrl = "www.test51.com"
            },
            new ArgumentData
            {
                Id = 53,
                ArgumentText = "test 53 ",
                CounterArgumentSummary = "test counter argument 53 ",
                ReferenceUrl = "www.test53.com"
            },
            new ArgumentData
            {
                Id = 54,
                ArgumentText = "test 54",
                CounterArgumentSummary = "test counter argument 54",
                ReferenceUrl = "www.test54.com"
            }
        };

        // Act
        var result = await climateFactChecker.GetFactCheckResponses(claimsWithReferences, claimsToCheck, argumentData);


        // Assert
        Assert.NotNull(result);

    }
}
