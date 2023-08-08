using FactCheckingService.Strategies.GeneralStrategy.FactCheckPrompt;
using FactCheckingService.Strategies.GeneralStrategy;
using Moq;
using Shared.GptClient;
using Shared.Models;
using FactCheckingService.Models;
using Shared.Configuration;
using Microsoft.Extensions.Options;

namespace FactCheckingServiceTests.FactCheckers.GeneralStrategy;

public class FactCheckerStrategyGeneralTests
{
    private readonly FactCheckerOptions _factCheckerOptions;
    private readonly Mock<IGeneralFactCheckPrompt> _mockGeneralFactCheckPrompt;
    private readonly Mock<IGptClient> _mockGptClient;
    private readonly Mock<IGptResponseParser> _mockGptResponseParser;
    private readonly FactCheckerStrategyGeneral _factCheckerStrategy;

    public FactCheckerStrategyGeneralTests()
    {
        _factCheckerOptions = new FactCheckerOptions { AllowGeneralFactCheck = true };

        _mockGeneralFactCheckPrompt = new Mock<IGeneralFactCheckPrompt>();
        _mockGptClient = new Mock<IGptClient>();
        _mockGptResponseParser = new Mock<IGptResponseParser>();
        _factCheckerStrategy = new FactCheckerStrategyGeneral(Options.Create(_factCheckerOptions), _mockGeneralFactCheckPrompt.Object, _mockGptClient.Object, _mockGptResponseParser.Object);
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
        var gptResponseFunctionCallFactCheck = new FactCheckResponse
        {
            Label = "correct",
            Explanation = "It's true"
        };

        _mockGeneralFactCheckPrompt.Setup(x => x.GetPrompt(It.IsAny<Fact>())).Returns(factCheckPrompt);

        _mockGptClient.Setup(x => x.GetCompletion(It.IsAny<Prompt>(), It.IsAny<double>())).ReturnsAsync(factCheckGptResponse);

        _mockGptResponseParser.Setup(x => x.ParseGptResponseFunctionCall<FactCheckResponse>(factCheckGptResponse, nameof(FactCheckerStrategyGeneral)))
                              .Returns(gptResponseFunctionCallFactCheck);

        var result = await _factCheckerStrategy.ExecuteFactCheck(new List<Fact> { new Fact { /* initialization code here */ } });

        Assert.Single(result);
        Assert.True(result.First().IsFactChecked);
    }
}

