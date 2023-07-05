using FactCheckingService.FactCheckers.GeneralStrategy.FactCheckPrompt;
using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.GeneralStrategy;

public class FactCheckerStrategyGeneralTests
{

    [Fact]
    public async Task GetPrompt_should_return_correct_prompt()
    {
        // Arrange
        var generalFactCheckPrompt = new GeneralFactCheckPrompt();

        var fact = new Fact
        {
            Id = "50",
            ClaimRawText = "High LDL doesn't cause heart disease.",
            Tags = new[] { "health" }
        };

        // Act

        var result = await generalFactCheckPrompt.GetPrompt(fact);

        // Assert
        Assert.NotNull(result);

        var serialized = JsonHelper.Serialize(result, includeNullValues: false);

        Assert.NotNull(serialized);
    }
}
