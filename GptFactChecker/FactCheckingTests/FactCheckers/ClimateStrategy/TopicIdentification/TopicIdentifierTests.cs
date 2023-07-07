using FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;
using FactCheckingService.Models;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Configuration;
using Shared.GptClient;
using Shared.Models;

namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy.TopicIdentification;

public class TopicIdentifierTests
{
    private const string _topicResponse =
"""
{
  "id": "chatcmpl-7YAVcDWyYjNbj5Kz6H8HjKmeiPfF0",
  "object": "chat.completion",
  "created": 1688377904,
  "model": "gpt-3.5-turbo-0613",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "{\"claims_with_references\": [\n  {\n    \"claim_id\": \"1\",\n    \"reference_ids\": [\"419924\"]\n  },\n  {\n    \"claim_id\": \"3\",\n    \"reference_ids\": [\"654985\"]\n  }\n]}",
        "function_call": {
          "name": "fact_check_claims",
          "arguments": "{\n  \"claims_with_references\": [\n    {\n      \"claim_id\": \"1\",\n      \"reference_ids\": [\"419924\"]\n    },\n    {\n      \"claim_id\": \"3\",\n      \"reference_ids\": [\"654985\"]\n    }\n  ]\n}"
        }
      },
      "finish_reason": "function_call"
    }
  ],
  "usage": {
    "prompt_tokens": 210,
    "completion_tokens": 112,
    "total_tokens": 322
  }
}
""";

    private Mock<IGptClient> _mockGptClient;
    private Mock<IGptResponseParser> _mockGptResponseParser;
    private Mock<ITopicIdentificationPrompt> _mockTopicIdentificationPrompt;
    private FactCheckerOptions _factCheckerOptions;

    public TopicIdentifierTests()
    {
        _mockGptClient = new Mock<IGptClient>();
        _mockGptResponseParser = new Mock<IGptResponseParser>();
        _mockTopicIdentificationPrompt = new Mock<ITopicIdentificationPrompt>();
        _factCheckerOptions = new FactCheckerOptions();
    }

    [Fact]
    public async Task Should_Return_Claims_When_Correct_ArgumentDataList_And_CompatibleFacts_Are_Passed()
    {
        // Arrange
        var topicIdentifier = new TopicIdentifier(Options.Create(_factCheckerOptions), _mockGptClient.Object, _mockGptResponseParser.Object, _mockTopicIdentificationPrompt.Object);
        
        var facts = new List<Fact> { new Fact { Id = "1", ClaimSummarized = "Fact 1" } };
        
        var arguments = new List<ArgumentData> {
            new ArgumentData { Id = 1, ArgumentText = "Argument 1" },
            new ArgumentData { Id = 2, ArgumentText = "Argument 2" },
            new ArgumentData { Id = 3, ArgumentText = "Argument 3" },
        };

        var expectedClaims = new List<ClaimWithReferences>
        {
            new ClaimWithReferences { ClaimId = "1", ReferenceIds = new List<string> { "Ref1", "Ref2" } }
        };

        _mockGptClient.Setup(client => client.GetCompletion(It.IsAny<string>(), It.IsAny<double>())).ReturnsAsync(_topicResponse);

        _mockGptResponseParser.Setup(parser => parser.ParseGptResponseFunctionCall<GptResponseFunctionCallTopicIdentification>(
                It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new GptResponseFunctionCallTopicIdentification { ClaimsWithReferences = expectedClaims });

        // Act
        var claimsWithReferences = await topicIdentifier.GetClaimsWithReferences(facts, arguments);

        // Assert
        Assert.Equal(expectedClaims, claimsWithReferences);
    }

    [Fact]
    public async Task Should_Return_Claims_And_Combine_References_When_Same_ClaimId_Exists()
    {
        // Arrange
        var topicIdentifier = new TopicIdentifier(Options.Create(_factCheckerOptions), _mockGptClient.Object, _mockGptResponseParser.Object, _mockTopicIdentificationPrompt.Object);
        
        var facts = new List<Fact> { new Fact { Id = "1", ClaimSummarized = "Fact 1" } };
        
        var arguments = new List<ArgumentData> {
            new ArgumentData { Id = 1, ArgumentText = "Argument 1" },
            new ArgumentData { Id = 2, ArgumentText = "Argument 2" },
            new ArgumentData { Id = 3, ArgumentText = "Argument 3" },
        };

        var expectedClaims = new List<ClaimWithReferences>
        {
            new ClaimWithReferences { ClaimId = "1", ReferenceIds = new List<string> { "Ref1", "Ref2", "Ref3", "Ref4", "Ref5", "Ref6" } }
        };

        _mockGptClient.Setup(client => client.GetCompletion(It.IsAny<string>(), It.IsAny<double>())).ReturnsAsync(_topicResponse);

        _mockGptResponseParser.SetupSequence(parser => parser.ParseGptResponseFunctionCall<GptResponseFunctionCallTopicIdentification>(
                It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new GptResponseFunctionCallTopicIdentification { ClaimsWithReferences = new List<ClaimWithReferences> { new ClaimWithReferences { ClaimId = "1", ReferenceIds = new List<string> { "Ref1", "Ref2" } } } })
            .Returns(new GptResponseFunctionCallTopicIdentification { ClaimsWithReferences = new List<ClaimWithReferences> { new ClaimWithReferences { ClaimId = "1", ReferenceIds = new List<string> { "Ref3", "Ref4" } } } })
            .Returns(new GptResponseFunctionCallTopicIdentification { ClaimsWithReferences = new List<ClaimWithReferences> { new ClaimWithReferences { ClaimId = "1", ReferenceIds = new List<string> { "Ref5", "Ref6" } } } });

        // Act
        var claimsWithReferences = await topicIdentifier.GetClaimsWithReferences(facts, arguments);

        // Assert
        Assert.Equal(expectedClaims[0].ClaimId, claimsWithReferences[0].ClaimId);
        Assert.Equal(expectedClaims[0].ReferenceIds.Count, claimsWithReferences[0].ReferenceIds.Count);
        Assert.True(expectedClaims[0].ReferenceIds.All(claim => claimsWithReferences[0].ReferenceIds.Contains(claim)));
    }
}

