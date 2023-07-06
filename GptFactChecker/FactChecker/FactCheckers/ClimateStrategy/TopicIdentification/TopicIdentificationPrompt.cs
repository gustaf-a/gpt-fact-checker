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
Your task is to identify which references are related to different claims and send them for fact checking. Reference IDs with keywords are presented below.
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
        if (argument.Id == 0)
        {
            Console.WriteLine($"Invalid argument found. ID 0 is not allowed. Argument:'{JsonHelper.Serialize(argument)}'.");
            return;
        }

        if (!argument.ArgumentTextKeyWords.IsNullOrEmpty())
        {
            sb.AppendLine($"<{argument.Id}>{string.Join(",", argument.ArgumentTextKeyWords)}</{argument.Id}>");
            return;
        }

        if (!string.IsNullOrWhiteSpace(argument.ArgumentText))
        {
            sb.AppendLine($"<{argument.Id}>{argument.ArgumentText}</{argument.Id}>");
            return;
        }

        Console.WriteLine($"Invalid argument found. No valid keywords or argument text found. Argument: '{JsonHelper.Serialize(argument)}'.");
        return;
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
