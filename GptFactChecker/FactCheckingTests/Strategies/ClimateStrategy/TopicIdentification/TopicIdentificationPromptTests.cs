using FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;
using JsonClient;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy.TopicIdentification;

public class TopicIdentificationPromptTests
{
    [Fact]
    public async Task GetPrompt_throws_if_invalid_factClaims()
    {
        // Arrange
        var topicIdentificationPrompt = new TopicIdentificationPrompt();

        var factClaims = new List<Fact>();

        var argumentData = new List<ArgumentData> {
            new ArgumentData()
        };

        // Act
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => topicIdentificationPrompt.GetPrompt(factClaims, argumentData));
    }

    [Fact]
    public async Task GetPrompt_throws_if_invalid_argumentData()
    {
        // Arrange
        var topicIdentificationPrompt = new TopicIdentificationPrompt();

        var factClaims = new List<Fact>
        {
            new Fact()
        };

        var argumentData = new List<ArgumentData>();

        // Act
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => topicIdentificationPrompt.GetPrompt(factClaims, argumentData));
    }

    [Fact]
    public async Task GetPrompt_returns_prompt_with_references_and_facts()
    {
        // Arrange
        var topicIdentificationPrompt = new TopicIdentificationPrompt();

        var factClaims = new List<Fact>
        {
            new Fact
            {
                Id = "1",
                ClaimSummarized = "Earth has always warmed and cooled."
            },
            new Fact
            {
                Id = "2",
                ClaimSummarized = "We're going towards a new ice age."
            }
        };

        var argumentData = new List<ArgumentData> {
            new ArgumentData
            {
                Id = 122334,
                ArgumentTextKeyWords = new(){"Holistic Management", "reverse climate change", "grazing strategy", "desertification", "carbon sequestration", "Allan Savory"}
            },
            new ArgumentData
            {
                Id = 123456,
                ArgumentText = "It's only natural cycles."
            },
            new ArgumentData
            {
                Id = 555544,
                ArgumentText = "The sun is heating up."
            }
        };

        // Act
        var result = await topicIdentificationPrompt.GetPrompt(factClaims, argumentData);

        // Assert
        Assert.NotNull(result);

        Assert.NotEmpty(result.Messages);
        Assert.NotEmpty(result.Functions);

        var serialized = JsonHelper.Serialize(result, false);

        Assert.NotNull(serialized);
    }
}
