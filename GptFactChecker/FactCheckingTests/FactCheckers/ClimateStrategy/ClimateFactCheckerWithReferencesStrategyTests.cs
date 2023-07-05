using FactCheckingService.FactCheckers.ClimateStrategy;
using FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;
using FactCheckingService.FactCheckers.ClimateStrategy.Models;
using FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;
using Moq;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy;

public class ClimateFactCheckerWithReferencesStrategyTests
{
    [Fact]
    public async Task ExecuteFactCheck_should_return_fact_check_responses()
    {
        // Arrange
        var claimsWithReferences = new List<ClaimWithReferences>
        {
            new ClaimWithReferences
            {
                ClaimId = 1,
                ReferenceIds = new List<string> { "419924" }
            },
            new ClaimWithReferences
            {
                ClaimId = 3,
                ReferenceIds = new List<string> { "654985" }
            }
        };

        var topicIdentifierMock = new Mock<ITopicIdentifier>();
        topicIdentifierMock.Setup(t => t.GetClaimsWithReferences(It.IsAny<List<Fact>>(), It.IsAny<List<ArgumentData>>())).Returns(Task.FromResult(claimsWithReferences));

        var factsToCheck = new List<Fact>
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

        var factCheckerResponses = new List<FactCheckResponse>
        {
            new FactCheckResponse
            {
                Fact = factsToCheck[0],
                IsChecked = true,
                FactCheck = new FactCheck
                {
                    Id = "11"
                }
            },
            new FactCheckResponse
            {
                Fact = factsToCheck[2],
                IsChecked = true,
                FactCheck = new FactCheck
                {
                    Id = "33"
                }
            }
        };

        var climateFactCheckerWithDataMock = new Mock<IClimateFactCheckerWithData>();
        climateFactCheckerWithDataMock.Setup(c => c.GetFactCheckResponses(claimsWithReferences, It.IsAny<List<Fact>>(), It.IsAny<List<ArgumentData>>())).Returns(Task.FromResult(factCheckerResponses));

        var factChecker = new ClimateFactCheckerWithReferencesStrategy(topicIdentifierMock.Object, climateFactCheckerWithDataMock.Object);

        // Act
        var result = await factChecker.ExecuteFactCheck(factsToCheck);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(2, result.Count);
    }
}
