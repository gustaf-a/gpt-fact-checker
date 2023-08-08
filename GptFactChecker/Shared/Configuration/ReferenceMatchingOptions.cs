namespace Shared.Configuration;

public class ReferenceMatchingOptions
{
    public const string ReferenceMatching = "ReferenceMatching";
    public string Version { get; set; } = "1.0";

    public int MaxRefCount { get; set; } = 80;
    public int MaxFactsCount { get; set; } = 20;

    public int ReferenceTextMaxLengthInPrompt { get; set; } = 200;

    public string Delimiter { get; set; } = "####";
    public string ReferencesTagText { get; set; } = "references";

    public string SystemPromptWithoutReferences { get; set; } =
"""
Your task is to identify which references are related to different claims and send them for fact checking. Reference IDs with keywords are presented below.
""";

    public string Functions { get; set; } =
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
}
