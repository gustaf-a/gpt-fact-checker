namespace Shared.Configuration;

public class ReferenceFactCheckingOptions
{
    public const string ReferenceFactChecking = "ReferenceFactChecking";

    public string Delimiter { get; set; } = "####";
    public string ReferencesTagText { get; set; } = "references";

    public string SystemPromptWithoutReferences { get; set; } =
"""
Act as an expert fact checker specialized in climate change, environmental issues and sustainability. Use the references provided to fact check the presented claim. When a reference is used the ID should be written like this in the explanation: (ref:123)
""";

    public string Functions { get; set; } =
"""
[
  {
    "name": "store_fact_check",
    "description": "Stores a fact checked claim",
    "parameters": {
      "type": "object",
      "properties": {
        "label": {
          "type": "string",
          "enum": [ "True", "False", "Misleading", "Exaggerated", "Understated", "Unsupported", "Out of Context", "Fact Check Failed" ]
        },
        "explanation": {
          "type": "string",
          "description": "Short text explaining why the label is correct with references used like this (ref:123)"
        }
      },
      "required": [ "label", "explanation" ]
    }
  }
]
""";
}
