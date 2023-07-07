using Newtonsoft.Json;

namespace FactCheckingService.Models;

internal class GeneralFactCheckFunctionCallArguments
{
    [JsonProperty(PropertyName = "fact_check")]
    public GptResponseFunctionCallFactCheck FactCheck { get; set; }
}
