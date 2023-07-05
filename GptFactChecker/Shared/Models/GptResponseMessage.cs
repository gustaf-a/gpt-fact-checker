using Newtonsoft.Json;

namespace Shared.Models;

public class GptResponseMessage
{
    [JsonProperty(PropertyName = "role")]
    public string Role { get; set; }

    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }

    [JsonProperty(PropertyName = "function_call")]
    public GptResponseFunctionCall? FunctionCall { get; set; }
}
