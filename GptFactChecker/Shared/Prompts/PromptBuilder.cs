﻿using JsonClient;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;

namespace Shared.Prompts;

public class PromptBuilder : IPromptBuilder
{
    private readonly OpenAiOptions _openAiOptions;
    private readonly List<PromptMessage> _promptMessages = new();
    private readonly List<PromptFunction> _promptFunctions = new();

    private string _model;

    public PromptBuilder(IOptions<OpenAiOptions> options)
    {
        _openAiOptions = options.Value;
    }

    public void Reset()
    {
        _model = string.Empty;
        _promptFunctions.Clear();
        _promptMessages.Clear();  
    }

    public Prompt GetPrompt()
    {
        if (_promptMessages.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(_promptMessages), $"Invalid prompt: systemPrompt and userPrompt both empty");

        var prompt = new Prompt
        {
            Model = GetModelInternal(),
            Messages = _promptMessages,
            Functions = _promptFunctions,
            FunctionCall = GetFunctionCall()
        };

        return prompt;
    }

    private string GetModelInternal()
    {
        if (!string.IsNullOrWhiteSpace(_model))
            return _model;

        if (string.IsNullOrWhiteSpace(_openAiOptions.ApiModelBase))
            throw new ArgumentNullException(nameof(_openAiOptions.ApiModelBase));

        return _openAiOptions.ApiModelBase;
    }

    public string GetModel()
    {
        return GetModelInternal();
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

    public void AddFunctionCall(string promptFunctionJson)
    {
        if(string.IsNullOrWhiteSpace(promptFunctionJson))
            throw new ArgumentNullException(nameof(promptFunctionJson));

        var functionCall = JsonHelper.TryDeserialize<PromptFunction>(promptFunctionJson);
        if (functionCall != null)
        {
            AddFunctionCall(functionCall);
            return;
        }

        var functionCallList = JsonHelper.TryDeserialize<List<PromptFunction>>(promptFunctionJson);
        if (functionCallList.IsNullOrEmpty())
            throw new ArgumentException(nameof(promptFunctionJson), "Failed to deserialize input into PromptFunction or List<PromptFunction>");

        foreach (var call in functionCallList)
            AddFunctionCall(call);

        return;
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
