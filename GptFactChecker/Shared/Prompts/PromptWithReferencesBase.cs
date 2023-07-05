using Shared.Models;
using Shared.Extensions;
using GptFactCheckerApi.Repository.JsonRepo;

namespace Shared.Prompts;

public abstract class PromptWithReferencesBase : PromptBase
{
    protected const string Delimiter = "####";
    protected const string ReferenceTagText = "references";

    private const string DefaultModel = "gpt-3.5-turbo";

    public async Task<Prompt> GetPrompt(List<Fact> facts, List<ArgumentData> arguments, string promptFunctionsFilePath, string model = DefaultModel)
    {
        if (facts.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(facts));

        if (arguments.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(arguments));
        
        var functions = await GetPromptFunctions(promptFunctionsFilePath);

        return GetPrompt(BuildSystemPrompt(arguments), BuildUserPrompt(facts), functions, model);
    }

    protected abstract string BuildSystemPrompt(List<ArgumentData> arguments);

    protected abstract string BuildUserPrompt(List<Fact> facts);

    protected static async Task<List<PromptFunction>> GetPromptFunctions(string promptFunctionsFilePath)
    {
        var functions = await JsonHelper.GetObjectFromJson<List<PromptFunction>>(promptFunctionsFilePath);

        if (functions.IsNullOrEmpty())
            return null;

        return functions;
    }
}
