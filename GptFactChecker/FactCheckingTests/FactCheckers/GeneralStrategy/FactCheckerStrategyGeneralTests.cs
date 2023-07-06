using FactCheckingService.FactCheckers.GeneralStrategy.FactCheckPrompt;
using FactCheckingService.FactCheckers.GeneralStrategy;
using Moq;
using Shared.GptClient;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.GeneralStrategy;

public class FactCheckerStrategyGeneralTests
{
    private readonly Mock<IGeneralFactCheckPrompt> _mockGeneralFactCheckPrompt;
    private readonly Mock<IGptClient> _mockGptClient;
    private readonly Mock<IGptResponseParser> _mockGptResponseParser;
    private readonly FactCheckerStrategyGeneral _factCheckerStrategy;

    public FactCheckerStrategyGeneralTests()
    {
        _mockGeneralFactCheckPrompt = new Mock<IGeneralFactCheckPrompt>();
        _mockGptClient = new Mock<IGptClient>();
        _mockGptResponseParser = new Mock<IGptResponseParser>();
        _factCheckerStrategy = new FactCheckerStrategyGeneral(_mockGeneralFactCheckPrompt.Object, _mockGptClient.Object, _mockGptResponseParser.Object);
    }

    [Fact]
    public async Task ExecuteFactCheck_ReturnsEmptyList_WhenNoFactsProvided()
    {
        var result = await _factCheckerStrategy.ExecuteFactCheck(null);

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteFactCheck_ReturnsFactCheckResponse_WhenFactsProvided()
    {
        var factCheckPrompt = new Prompt();
        var factCheckGptResponse = "Test Response";
        var gptResponseFunctionCallFactCheck = new GptResponseFunctionCallFactCheck
        {
            Id = "1234",
            Label = "correct",
            Explanation = "It's true",
            ReferencesUsed = new List<string> { "Myself (2023)" }
        };

        _mockGeneralFactCheckPrompt.Setup(x => x.GetPrompt(It.IsAny<Fact>())).ReturnsAsync(factCheckPrompt);

        _mockGptClient.Setup(x => x.GetCompletion(It.IsAny<Prompt>(), It.IsAny<double>())).ReturnsAsync(factCheckGptResponse);

        _mockGptResponseParser.Setup(x => x.ParseGptResponseFunctionCall<GptResponseFunctionCallFactCheck>(factCheckGptResponse, nameof(FactCheckerStrategyGeneral)))
                              .Returns(gptResponseFunctionCallFactCheck);

        var result = await _factCheckerStrategy.ExecuteFactCheck(new List<Fact> { new Fact { /* initialization code here */ } });

        Assert.Single(result);
        Assert.True(result.First().IsChecked);
    }

    [Fact]
    public void IsCompatible_ReturnsTrue_Always()
    {
        var result = _factCheckerStrategy.IsCompatible(new Fact { /* initialization code here */ });

        Assert.True(result);
    }
}

