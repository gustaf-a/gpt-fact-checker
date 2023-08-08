using JsonClient;
using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactExtractionService.FactExtractors.FunctionCallingStrategy.FactExtractionPrompt;

public class FactExtractionFunctionCallingPrompt : PromptBase, IFactExtractionFunctionCallingPrompt
{
    private const string ContextTextTag = "context_text";
    private const string RawTextTag = "raw_text";

    private const string SystemPrompt =
$"""
Act as an expert text analyzer processing text for later fact checking. You will be provided a text within tags <{RawTextTag}>. Extract all significant claims made in the text and present them as facts. Use the context within tags <{ContextTextTag}> to make it easy to understand the claims.
""";

    private const string Functions =
"""
[
  {
    "name": "store_claims",
    "description": "Stores extracted claims",
    "parameters": {
      "type": "object",
      "properties": {
        "extracted_claims": {
          "type": "array",
          "description": "The extracted claims",
          "items": {
            "type": "object",
            "properties": {
              "claim_raw_text": {
                "type": "string",
                "description": "The raw text of the claim made exactly as is in the text with enough context for it to be understandable"
              },
              "claim_summarized": {
                "type": "string",
                "description": "A short summary of the claim made, stated as a fact"
              },
              "tags": {
                "type": "array",
                "description": "A few topics that relate to the claim made",
                "items": {
                  "type": "string"
                }
              }
            }
          }
        }
      },
      "required": [ "extracted_claims", "claim_raw_text", "claim_summarized" ]
    }
  }
]
""";

    public Prompt GetPrompt(string rawText, string contextText, string modeltoUse = null)
    {
        var systemPrompt = SystemPrompt;

        var userPrompt = CreateUserPrompt(rawText, contextText);

        var functions = JsonHelper.Deserialize<List<PromptFunction>>(Functions);

        return GetPrompt(systemPrompt, userPrompt, functions, modeltoUse);
    }

    private static string CreateUserPrompt(string rawText, string contextText)
    {
        var sb = new StringBuilder();

        sb.Append($"<{ContextTextTag}>");
        sb.Append(contextText);
        sb.AppendLine($"</{ContextTextTag}>");        
        
        sb.Append($"Raw text to extract claims from:");
        sb.Append($"<{RawTextTag}>");
        sb.Append(rawText);
        sb.Append($"</{RawTextTag}>");

        return sb.ToString();
    }
}
