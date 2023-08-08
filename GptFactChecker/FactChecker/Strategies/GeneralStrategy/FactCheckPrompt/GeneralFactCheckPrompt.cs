using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactCheckingService.Strategies.GeneralStrategy.FactCheckPrompt;

public class GeneralFactCheckPrompt : PromptBase, IGeneralFactCheckPrompt
{
    private const string Delimiter = "####";

    private const string SystemPrompt =
$"""
Act as a knowledgable expert fact checker.
""";

    private const string Functions =
"""
[
  {
    "name": "store_fact_check",
    "description": "Stores a fact checked claim",
    "parameters": {
      "type": "object",
      "properties": {
        "id": {
          "type": "string",
          "description": "ID of the fact checked claim"
        },
        "label": {
          "type": "string",
          "enum": [ "correct", "false", "misleading", "exaggerated", "unsupported" ]
        },
        "explanation": {
          "type": "string",
          "description": "Short text explaining why the label is proper"
        }
      },
      "required": [ "id", "label", "explanation" ]
    }
  }
]
""";

    public Prompt GetPrompt(Fact fact)
    {
        var systemPrompt = SystemPrompt;

        var userPrompt = CreateUserPrompt(fact);

        var functions = JsonClient.JsonHelper.Deserialize<List<PromptFunction>>(Functions);

        return GetPrompt(systemPrompt, userPrompt, functions);
    }

    private static string CreateUserPrompt(Fact fact)
    {
        var sb = new StringBuilder();

        sb.Append($"Claim ID: {fact.Id} ");
        sb.Append(Delimiter);
        sb.Append(fact.ClaimRawText);
        sb.Append(Delimiter);

        return sb.ToString();
    }
}
