using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Extensions;
using Shared.Models;

namespace Shared.Prompts;

public abstract class PromptBase : IPrompt
{
    private const string DefaultModel = "gpt-3.5-turbo";

    public async Task<Prompt> GetPrompt(string systemMessage, string userMessage, string functionsJsonFilePath, string model = DefaultModel)
    {
        var functions = await GetFunctionsFromJsonFile(functionsJsonFilePath);

        return GetPrompt(systemMessage, userMessage, functions);
    }

    public Prompt GetPrompt(string systemPrompt, string userPrompt, List<PromptFunction> functions, string model = DefaultModel)
    {
        if (string.IsNullOrWhiteSpace(systemPrompt) && string.IsNullOrWhiteSpace(userPrompt))
            throw new ArgumentNullException("Prompt", $"Invalid prompt: systemPrompt and userPrompt both empty");

        var prompt = new Prompt
        {
            Model = model,
            Messages = GetPromptMessages(systemPrompt, userPrompt),
            Functions = functions,
            FunctionCall = GetFunctionCall(functions)
        };

        return prompt;
    }

    private List<PromptMessage> GetPromptMessages(string systemPrompt, string userPrompt)
    {
        var messages = new List<PromptMessage>
        {
            GetSystemPromptMessage(systemPrompt),

            GetUserPromptMessage(userPrompt)
        };

        return messages;
    }

    private PromptMessage GetSystemPromptMessage(string systemPrompt)
    {
        return new PromptMessage
        {
            Role = PromptRoles.System,
            Content = systemPrompt
        };
    }

    private PromptMessage GetUserPromptMessage(string userPrompt)
    {
        return new PromptMessage
        {
            Role = PromptRoles.User,
            Content = userPrompt
        };
    }

    protected virtual PromptFunctionCall GetFunctionCall(List<PromptFunction> functions)
    {
        if (functions.IsNullOrEmpty())
            return null;

        return new PromptFunctionCall
        {
            Name = functions[0].Name
        };
    }

    private static async Task<List<PromptFunction>> GetFunctionsFromJsonFile(string promptFunctionsFilePath)
    {
        var functions = await JsonHelper.GetObjectFromJson<List<PromptFunction>>(promptFunctionsFilePath);

        if (functions.IsNullOrEmpty())
            return null;

        return functions;
    }
}
