using Shared.Models;

namespace Shared.Prompts;

public interface IPrompt
{
    public Task<Prompt> GetPrompt(string systemMessage, string userMessage, string functionsJsonFilePath, string model);
    public Prompt GetPrompt(string systemMessage, string userMessage, List<PromptFunction> functions, string model);
}
