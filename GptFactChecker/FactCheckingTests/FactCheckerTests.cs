using FactCheckingService;
using FactCheckingService.FactCheckers;
using Moq;
using Shared.Models;

namespace FactCheckingTests;

public class FactCheckerTests
{
    private Mock<IFactCheckerStrategy> _mockedFackCheckerStrategyOne;
    private Mock<IFactCheckerStrategy> _mockedFackCheckerStrategyTwo;
    private IFactChecker _factChecker;

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
    public async Task CheckFacts_ReturnsEmptyList_WhenNoInputProvided()
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
    public async Task CheckFacts_DoesNotCallSecondStrategy_WhenFirstStrategySucceeds()
    {
        // Arrange
        var facts = GetTestFacts();

        var factCheckResponses = new List<FactCheckResult> {
            new FactCheckResult
            {
                IsFactChecked = true,
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
        _mockedFackCheckerStrategyTwo.Verify(f => f.ExecuteFactCheck(It.IsAny<List<Fact>>()), Times.Never);
    }

    [Fact]
    public async Task CheckFacts_CallsSecondStrategy_WhenFirstStrategyFails()
    {
        // Arrange
        var facts = GetTestFacts();

        _mockedFackCheckerStrategyOne
            .Setup(f => f.ExecuteFactCheck(It.IsAny<List<Fact>>()))
            .ReturnsAsync(new List<FactCheckResult>());

        _mockedFackCheckerStrategyTwo
            .Setup(f => f.ExecuteFactCheck(It.IsAny<List<Fact>>()))
            .ReturnsAsync(new List<FactCheckResult>() { new FactCheckResult { IsFactChecked = true, Fact = facts[0] } });

        // Act
        var responses = await _factChecker.CheckFacts(facts);

        // Assert
        Assert.NotNull(responses);
        Assert.Single(responses);
        _mockedFackCheckerStrategyTwo.Verify(f => f.ExecuteFactCheck(It.IsAny<List<Fact>>()), Times.Once);
    }

    private List<Fact> GetTestFacts()
    {
        return new List<Fact>
        {
            new Fact
            {
                Id = "1",
                ClaimSummarized = "Summary",
                ClaimRawText = "raw text",
                Tags = new[] { "tag1", "tag2"}
            }
        };
    }
}