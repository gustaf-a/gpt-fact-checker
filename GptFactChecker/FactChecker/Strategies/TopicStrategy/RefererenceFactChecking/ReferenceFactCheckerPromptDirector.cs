using JsonClient;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactCheckingService.Strategies.TopicStrategy.RefererenceFactChecking;

public class ReferenceFactCheckerPromptDirector : IReferenceFactCheckerPromptDirector
{
    private readonly IPromptBuilder _promptBuilder;
    private readonly ReferenceFactCheckingOptions _referenceFactCheckingOptions;

    public ReferenceFactCheckerPromptDirector(IPromptBuilder promptBuilder, IOptions<ReferenceFactCheckingOptions> options)
    {
        _promptBuilder = promptBuilder;
        _referenceFactCheckingOptions = options.Value;
    }

    public Prompt BuildPrompt(Fact fact, List<Reference> relevantReferences)
    {
        _promptBuilder.Reset();

        _promptBuilder.AddFunctionCall(_referenceFactCheckingOptions.Functions);

        _promptBuilder.AddSystemMessage(GetSystemMessage(relevantReferences));
        _promptBuilder.AddUserMessage(GetUserMessage(fact));

        return _promptBuilder.GetPrompt();
    }

    private string GetSystemMessage(List<Reference> references)
    {
        var sb = new StringBuilder();

        sb.AppendLine(_referenceFactCheckingOptions.SystemPromptWithoutReferences);

        sb.AppendLine($"<{_referenceFactCheckingOptions.ReferencesTagText}>");

        foreach (var reference in references)
            AddReference(sb, reference);

        sb.AppendLine($"</{_referenceFactCheckingOptions.ReferencesTagText}>");

        return sb.ToString();
    }

    private static void AddReference(StringBuilder sb, Reference reference)
    {
        if (string.IsNullOrWhiteSpace(reference.Id))
        {
            Console.WriteLine($"Invalid argument found: '{JsonHelper.Serialize(reference)}'.");
            return;
        }

        var referenceText = string.IsNullOrWhiteSpace(reference.TextSummary) ? reference.Text : reference.TextSummary;

        if (string.IsNullOrWhiteSpace(referenceText))
        {
            Console.WriteLine($"Invalid argument found: ID '{reference.Id}', reference text is empty.");
            return;
        }

        sb.Append($"<{reference.Id}>");
        sb.Append(referenceText);
        sb.AppendLine($"</{reference.Id}>");
    }

    private string GetUserMessage(Fact fact)
    {
        var sb = new StringBuilder();

        sb.Append(_referenceFactCheckingOptions.Delimiter);
        sb.Append(fact.ClaimRawText);
        sb.Append(_referenceFactCheckingOptions.Delimiter);

        return sb.ToString();
    }

    public string GetVersionInfo()
    {
        return $"{_referenceFactCheckingOptions.Version} using {_promptBuilder.GetModel()}";
    }
}
