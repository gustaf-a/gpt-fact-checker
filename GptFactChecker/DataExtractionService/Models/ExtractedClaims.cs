using Newtonsoft.Json;

namespace FactExtractionService.Models;

public class ExtractedClaims
{
    [JsonProperty(PropertyName = "claim_raw_text")]
    public string ClaimRawText { get; set; }

    [JsonProperty(PropertyName = "claim_summarized")]
    public string ClaimSummarized { get; set; }

    [JsonProperty(PropertyName = "tags")]
    public string[] Tags { get; set; } = Array.Empty<string>();
}
