using JsonClient;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;

public class ReferenceMatcherPromptDirector : IReferenceMatcherPromptDirector
{
    private readonly IPromptBuilder _promptBuilder;
    private readonly ReferenceMatchingOptions _referenceMatchingOptions;

    public ReferenceMatcherPromptDirector(IPromptBuilder promptBuilder, IOptions<ReferenceMatchingOptions> options)
    {
        _promptBuilder = promptBuilder;

        _referenceMatchingOptions = options.Value;
    }

    public Prompt BuildPrompt(List<Fact> factsPart, List<Reference> referencesPart)
    {
        _promptBuilder.Reset();

        _promptBuilder.AddFunctionCall(_referenceMatchingOptions.Functions);

        _promptBuilder.AddSystemMessage(GetSystemMessage(referencesPart));
        _promptBuilder.AddUserMessage(GetUserMessage(factsPart));

        return _promptBuilder.GetPrompt();
    }

    private string GetSystemMessage(List<Reference> referencesPart)
    {
        var sb = new StringBuilder();

        sb.Append(_referenceMatchingOptions.SystemPromptWithoutReferences);

        sb.AppendLine($"<{_referenceMatchingOptions.ReferencesTagText}>");

        foreach (var reference in referencesPart)
            AddArgumentReference(sb, reference);

        sb.AppendLine($"</{_referenceMatchingOptions.ReferencesTagText}>");

        return sb.ToString();
    }

    private void AddArgumentReference(StringBuilder sb, Reference reference)
    {
        if (string.IsNullOrWhiteSpace(reference.Id))
        {
            Console.WriteLine($"Invalid references encountered. No ID:'{JsonHelper.Serialize(reference)}'.");
            return;
        }

        if (!reference.TextKeyWords.IsNullOrEmpty())
        {
            sb.AppendLine($"<{reference.Id}>{string.Join(",", reference.TextKeyWords)}</{reference.Id}>");
            return;
        }

        if (!string.IsNullOrWhiteSpace(reference.TextSummary))
        {
            sb.AppendLine($"<{reference.Id}>{reference.TextSummary}</{reference.Id}>");
            return;
        }

        if (!string.IsNullOrWhiteSpace(reference.Text))
        {
            if (reference.Text.Length < _referenceMatchingOptions.ReferenceTextMaxLengthInPrompt)
            {
                sb.AppendLine($"<{reference.Id}>{reference.Text}</{reference.Id}>");
                return;
            }

            Console.WriteLine("Reference text too long for ReferenceMatching.");
        }

        Console.WriteLine($"Invalid argument found. No valid keywords or summary text found. Argument: '{JsonHelper.Serialize(reference)}'.");
        return;
    }

    private string GetUserMessage(List<Fact> factsPart)
    {
        var promptClaims = factsPart.ToPromptClaims();
        var promptClaimsJson = JsonHelper.Serialize(promptClaims);

        var sb = new StringBuilder();

        sb.AppendLine(_referenceMatchingOptions.Delimiter);
        sb.AppendLine(promptClaimsJson);
        sb.AppendLine(_referenceMatchingOptions.Delimiter);

        return sb.ToString().Trim();
    }

    public string GetVersionInfo()
    {
        return $"{_referenceMatchingOptions.Version} using {_promptBuilder.GetModel()}";
    }
}
