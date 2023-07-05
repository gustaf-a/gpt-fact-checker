using Shared.Models;
using Shared.Extensions;

namespace Shared.Prompts;

public abstract class PromptWithReferencesBase : PromptBase
{
    protected const string Delimiter = "####";

    protected const string ReferenceTagText = "references";

    public async Task<Prompt> GetPrompt(List<Fact> facts, List<ArgumentData> arguments, string promptFunctionsFilePath)
    {
        if (facts.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(facts));

        if (arguments.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(arguments));

        return await base.GetPrompt(BuildSystemPrompt(arguments), BuildUserPrompt(facts), promptFunctionsFilePath);
    }

    protected abstract string BuildSystemPrompt(List<ArgumentData> arguments);

    protected abstract string BuildUserPrompt(List<Fact> facts);
}
