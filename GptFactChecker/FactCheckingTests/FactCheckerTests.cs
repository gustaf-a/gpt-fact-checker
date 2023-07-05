using FactCheckingService;
using FactCheckingService.FactCheckers;
using Moq;
using Shared.Models;

namespace FactCheckingTests;

public class FactCheckerTests
{
    private readonly IFactChecker _factChecker;

    private readonly Mock<IFactCheckerStrategy> _mockedFackCheckerStrategyOne;
    private readonly Mock<IFactCheckerStrategy> _mockedFackCheckerStrategyTwo;

    public FactCheckerTests()
    {
        _mockedFackCheckerStrategyOne = new Mock<IFactCheckerStrategy>();
        _mockedFackCheckerStrategyOne.Setup(f => f.Priority).Returns(1);

        _mockedFackCheckerStrategyTwo = new Mock<IFactCheckerStrategy>();
        _mockedFackCheckerStrategyTwo.Setup(f => f.Priority).Returns(2);

        var mockedFactCheckerStrategies = new List<IFactCheckerStrategy> { _mockedFackCheckerStrategyOne.Object, _mockedFackCheckerStrategyTwo.Object };

        _factChecker = new FactChecker(mockedFactCheckerStrategies);
    }

    [Fact]
    public async Task FactChecker_should_return_empty_list_if_no_input()
    {
        // Arrange
        var facts = new List<Fact>();

        // Act
        var responses = await _factChecker.CheckFacts(facts);

        // Assert
        Assert.NotNull(responses);
        Assert.Empty(responses);
    }

    [Fact]
    public async Task FactChecker_should_not_send_facts_to_strategy2_if_strategy1_works()
    {
        // Arrange
        var facts = new List<Fact>
        {
            new Fact
            {
                Id = "1",
                ClaimSummarized = "Summary",
                ClaimRawText = "raw text",
                Tags = new[] { "tag1", "tag2"}
            }
        };

        var factCheckResponses = new List<FactCheckResponse> {
            new FactCheckResponse
            {
                IsChecked = true,
                Fact = facts[0]
            }
        };

        _mockedFackCheckerStrategyOne
            .Setup(f => f.ExecuteFactCheck(It.IsAny<List<Fact>>()))
            .ReturnsAsync(factCheckResponses);

        _mockedFackCheckerStrategyTwo
            .Setup(f => f.ExecuteFactCheck(It.IsAny<List<Fact>>()))
            .ThrowsAsync(new Exception("Strategy 2 shouldn't be called when all facts are handled by strategy 1."));

        // Act
        var responses = await _factChecker.CheckFacts(facts);

        // Assert
        Assert.NotNull(responses);
        Assert.Single(responses);
    }
}