using Newtonsoft.Json;

namespace FactCheckingService.Models;

public class GptResponseFunctionCallTopicIdentification
{
    [JsonProperty(PropertyName = "claims_with_references")]
    public List<ClaimWithReferences> ClaimsWithReferences { get; set; }
}
