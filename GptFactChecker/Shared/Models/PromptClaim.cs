using Newtonsoft.Json;

namespace Shared.Models;

public class PromptClaim
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "claim")]
    public string Claim { get; set; }
}
