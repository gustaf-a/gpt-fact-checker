using Newtonsoft.Json;

namespace FactCheckingService.Models;

internal class GeneralFactCheckFunctionCallArguments
{
    [JsonProperty(PropertyName = "fact_check")]
    public FactCheckResponse FactCheck { get; set; }
}
