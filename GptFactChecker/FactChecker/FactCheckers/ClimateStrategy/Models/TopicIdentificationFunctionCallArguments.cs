using Newtonsoft.Json;

namespace FactCheckingService.FactCheckers.ClimateStrategy.Models;

public class TopicIdentificationFunctionCallArguments
{
    [JsonProperty(PropertyName = "claims_with_references")]
    public List<ClaimWithReferences> ClaimsWithReferences { get; set; }
}
