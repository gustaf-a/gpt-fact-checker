using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Models;
using Shared.Extensions;
using Shared.Prompts;
using System.Text;

namespace FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;

public class TopicIdentificationPrompt : PromptWithReferencesBase, ITopicIdentificationPrompt
{
    private const string FunctionsFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\TopicIdentification\TopicIdentificationFunctions.json";

    private const string SystemPromptWithoutReferences =
        $"""
Include all references relevant to a claim. Only use references from within the <{ReferenceTagText}> tags. Use function fact_check_with_reference_data if matching references found, otherwise use fact_check_without_reference_data.
""";

    public async Task<Prompt> GetPrompt(List<Fact> factClaims, List<ArgumentData> argumentData)
    {
        return await GetPrompt(factClaims, argumentData, FunctionsFilePath);
    }

    protected override string BuildSystemPrompt(List<ArgumentData> arguments)
    {
        var sb = new StringBuilder();

        sb.Append(SystemPromptWithoutReferences);

        sb.AppendLine($"<{ReferenceTagText}>");

        foreach (var argument in arguments)
            AddArgumentReference(sb, argument);

        sb.AppendLine($"</{ReferenceTagText}>");

        return sb.ToString();
    }

    private static void AddArgumentReference(StringBuilder sb, ArgumentData argument)
    {
        if (argument.Id == 0 || string.IsNullOrWhiteSpace(argument.ArgumentText))
        {
            Console.WriteLine($"Invalid argument found: ID '{argument.Id == 0}', ArgumentText:'{argument.ArgumentText}'.");
            return;
        }

        sb.AppendLine($"{argument.Id}: {argument.ArgumentText}");
    }

    protected override string BuildUserPrompt(List<Fact> facts)
    {
        var promptClaims = facts.ToPromptClaims();
        var promptClaimsJson = JsonHelper.Serialize(promptClaims);

        var sb = new StringBuilder();

        sb.AppendLine(Delimiter);
        sb.AppendLine(promptClaimsJson);
        sb.AppendLine(Delimiter);

        return sb.ToString().Trim();
    }
}
