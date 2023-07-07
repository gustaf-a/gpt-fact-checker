using FactExtractionService.Models;
using Newtonsoft.Json;

namespace FactCheckingService.Models;

internal class GptResponseFunctionCallFactExtraction
{
    [JsonProperty(PropertyName = "extracted_claims")]
    public List<ExtractedClaims> ExtractedClaims { get; set; }
}
