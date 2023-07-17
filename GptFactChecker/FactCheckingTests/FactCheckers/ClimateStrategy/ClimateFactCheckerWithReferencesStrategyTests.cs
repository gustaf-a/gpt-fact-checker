using FactCheckingService.FactCheckers.ClimateStrategy;
using FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;
using FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;
using FactCheckingService.Models;
using FactCheckingService.Utils;
using Moq;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy;

public class ClimateFactCheckerWithReferencesStrategyTests
{
    private readonly ClimateFactCheckerWithReferencesStrategy _strategy;
    private readonly Mock<ITopicIdentifier> _mockTopicIdentifier;
    private readonly Mock<IClimateFactCheckerWithData> _mockClimateFactCheckerWithData;

    public ClimateFactCheckerWithReferencesStrategyTests()
    {
        var tagMatcher = new TagMatcher();

        _mockTopicIdentifier = new Mock<ITopicIdentifier>();
        _mockClimateFactCheckerWithData = new Mock<IClimateFactCheckerWithData>();
        _strategy = new ClimateFactCheckerWithReferencesStrategy(_mockTopicIdentifier.Object, _mockClimateFactCheckerWithData.Object, tagMatcher);
    }

    [Fact]
    public void IsCompatible_IncompatibleTags_False()
    {
        var fact = new Fact() { Tags = new string[] { "basketball", "music" } };
        
        Assert.False(_strategy.IsCompatible(fact));
    }

    [Theory]
    [InlineData("climate change")]
    [InlineData("climate crisis")]
    [InlineData("global warming")]
    [InlineData("IPCC")]
    [InlineData("CO2")]
    [InlineData("emissions")]
    [InlineData("pollution")]
    [InlineData("temperature")]
    public void IsCompatible_CompatibleTags_True(string tag)
    {
        var fact = new Fact() { Tags = new string[] { tag } };
        
        Assert.True(_strategy.IsCompatible(fact));
    }

    [Fact]
    public void IsCompatible_MixedTags_True()
    {
        var fact = new Fact() { Tags = new string[] { "climate", "music" } };
        
        Assert.True(_strategy.IsCompatible(fact));
    }

    [Fact]
    public async Task ExecuteFactCheck_should_return_fact_check_responses()
    {
        // Arrange
        var claimsWithReferences = new List<ClaimWithReferences>
        {
            new ClaimWithReferences
            {
                ClaimId = "1",
                ReferenceIds = new List<string> { "419924" }
            },
            new ClaimWithReferences
            {
                ClaimId = "3",
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

        var factCheckerResponses = new List<FactCheckResult>
        {
            new FactCheckResult
            {
                Fact = factsToCheck[0],
                IsFactChecked = true,
                FactCheck = new FactCheck
                {
                    Id = "11"
                }
            },
            new FactCheckResult
            {
                Fact = factsToCheck[2],
                IsFactChecked = true,
                FactCheck = new FactCheck
                {
                    Id = "33"
                }
            }
        };

        var climateFactCheckerWithDataMock = new Mock<IClimateFactCheckerWithData>();
        climateFactCheckerWithDataMock.Setup(c => c.GetFactCheckResponses(claimsWithReferences, It.IsAny<List<Fact>>(), It.IsAny<List<ArgumentData>>())).Returns(Task.FromResult(factCheckerResponses));

        var factChecker = new ClimateFactCheckerWithReferencesStrategy(topicIdentifierMock.Object, climateFactCheckerWithDataMock.Object, new TagMatcher());

        // Act
        var result = await factChecker.ExecuteFactCheck(factsToCheck);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(2, result.Count);
    }
}
