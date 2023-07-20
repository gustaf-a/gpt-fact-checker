using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;

namespace Shared.Prompts;

public class PromptBuilderBase : IPromptBuilder
{
    private readonly OpenAiOptions _openAiOptions;
    private readonly List<PromptMessage> _promptMessages = new();
    private readonly List<PromptFunction> _promptFunctions = new();

    private string _model;

    public PromptBuilderBase(IOptions<OpenAiOptions> options)
    {
        _openAiOptions = options.Value;
    }

    public Prompt GetPrompt()
    {
        if (_promptMessages.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(_promptMessages), $"Invalid prompt: systemPrompt and userPrompt both empty");

        var prompt = new Prompt
        {
            Model = GetModel(),
            Messages = _promptMessages,
            Functions = _promptFunctions,
            FunctionCall = GetFunctionCall()
        };

        return prompt;
    }

    private string GetModel()
    {
        if (!string.IsNullOrWhiteSpace(_model))
            return _model;

        if (string.IsNullOrWhiteSpace(_openAiOptions.ApiModel))
            throw new ArgumentNullException(nameof(_openAiOptions.ApiModel));

        return _openAiOptions.ApiModel;
    }

    public void AddModel(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentNullException(nameof(model));

        _model = model;
    }

    public void AddFunctionCalls(List<PromptFunction> promptFunctions)
    {
        if (promptFunctions.IsNullOrEmpty())
            return;

        foreach (var promptFunction in promptFunctions)
            AddFunctionCall(promptFunction);
    }

    public void AddFunctionCall(PromptFunction promptFunction)
    {
        if (promptFunction is null)
            return;

        _promptFunctions.Add(promptFunction);
    }

    public void AddSystemMessage(string systemPrompt)
    {
        AddPromptMessage(systemPrompt, PromptRoles.System);
    }

    public void AddUserMessage(string userPrompt)
    {
        AddPromptMessage(userPrompt, PromptRoles.User);
    }

    private void AddPromptMessage(string prompt, string role)
    {
        _promptMessages.Add(new PromptMessage
        {
            Role = role,
            Content = prompt
        });
    }

    private PromptFunctionCall GetFunctionCall()
    {
        var promptFunctionCall = "none";

        if (!_promptFunctions.IsNullOrEmpty())
            promptFunctionCall = _promptFunctions.Count == 1 ? _promptFunctions[0].Name : "auto";

        return new PromptFunctionCall
        {
            Name = promptFunctionCall
        };
    }
}
