using Shared.Models;
using Shared.Extensions;
using Shared.Prompts;
using System.Text;
using JsonClient;

namespace FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;

public class TopicIdentificationPrompt : PromptWithReferencesBase, ITopicIdentificationPrompt
{
    private const string Functions =
"""
[
  {
    "name": "fact_check_claims",
    "description": "Fact check claims with or without references",
    "parameters": {
      "type": "object",
      "properties": {
        "claims_with_references": {
          "type": "array",
          "description": "The claims with any found relevant references",
          "items": {
            "type": "object",
            "properties": {
              "claim_id": {
                "type": "string",
                "description": "The id of the claim"
              },
              "reference_ids": {
                "type": "array",
                "description": "The ids of the relevant references if any found",
                "items": {
                  "type": "string"
                }
              }
            }
          }
        }
      },
      "required": [ "claims_with_references", "claim_id" ]
    }
  }
]
""";
    
    private const string SystemPromptWithoutReferences =
        $"""
Your task is to identify which references are related to different claims and send them for fact checking. Reference IDs with keywords are presented below.
""";

    public Prompt GetPrompt(List<Fact> factClaims, List<ArgumentData> argumentData)
    {
        var functions = JsonHelper.Deserialize<List<PromptFunction>>(Functions);

        return GetPrompt(factClaims, argumentData, functions);
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
