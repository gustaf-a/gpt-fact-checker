using Microsoft.Extensions.Options;
using Moq;
using Shared.Configuration;
using Shared.Models;
using Shared.Prompts;
using System.Reflection;

public class PromptBuilderBaseTests
{
    private Mock<IOptions<OpenAiOptions>> _openAiOptionsMock;

    public PromptBuilderBaseTests()
    {
        _openAiOptionsMock = new Mock<IOptions<OpenAiOptions>>();
        _openAiOptionsMock.Setup(o => o.Value).Returns(new OpenAiOptions { ApiModel = "testModel" });

    }

    [Fact]
    public void ConstructorTest()
    {
        // Arrange
        
        // Act
        var promptBuilder = new PromptBuilderBase(_openAiOptionsMock.Object);

        // Assert
        Assert.NotNull(promptBuilder);
    }

    [Fact]
    public void AddModel_ValidInputTest()
    {
        // Arrange

        var model = "test";
        var promptBuilder = new PromptBuilderBase(_openAiOptionsMock.Object);

        // Act
        promptBuilder.AddModel(model);

        // Assert
        // Due to private access level, _model is not directly accessible for the assertion
        // Instead, use GetPrompt() or any other public method that uses the _model field
        var prompt = promptBuilder.GetPrompt();
        Assert.Equal(model, prompt.Model);
    }

    [Fact]
    public void AddModel_NullOrWhiteSpaceInputTest()
    {
        // Arrange

        var model = "";
        var promptBuilder = new PromptBuilderBase(_openAiOptionsMock.Object);

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => promptBuilder.AddModel(model));
    }

    [Fact]
    public void GetPrompt_NoPromptMessagesTest()
    {
        // Arrange
        var promptBuilder = new PromptBuilderBase(_openAiOptionsMock.Object);

        // Act

        // Assert
        Assert.Throws<ArgumentNullException>(() => promptBuilder.GetPrompt());
    }

    [Fact]
    public void AddFunctionCall_ValidInputTest()
    {
        // Arrange
        var promptFunction = new PromptFunction { Name = "TestFunction" };
        var promptBuilder = new PromptBuilderBase(_openAiOptionsMock.Object);

        // Act
        promptBuilder.AddFunctionCall(promptFunction);
        promptBuilder.AddUserMessage("test prompt");

        // Assert
        var prompt = promptBuilder.GetPrompt();
        Assert.Contains(prompt.Functions, pf => pf.Name == promptFunction.Name);
    }
}
