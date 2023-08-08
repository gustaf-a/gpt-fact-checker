using Shared.Models;
using Shared.Extensions;

namespace Shared.Prompts;

public abstract class PromptWithReferencesBase : PromptBase
{
    protected const string Delimiter = "####";

    protected const string ReferenceTagText = "references";

    public Prompt GetPrompt(List<Fact> facts, List<ArgumentData> arguments, List<PromptFunction> promptFunctions)
    {
        if (facts.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(facts));

        if (arguments.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(arguments));

        return GetPrompt(BuildSystemPrompt(arguments), BuildUserPrompt(facts), promptFunctions);
    }

    protected abstract string BuildSystemPrompt(List<ArgumentData> arguments);

    protected abstract string BuildUserPrompt(List<Fact> facts);
}
