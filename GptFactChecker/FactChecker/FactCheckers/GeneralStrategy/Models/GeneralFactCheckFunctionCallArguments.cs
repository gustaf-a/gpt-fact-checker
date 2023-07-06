using Newtonsoft.Json;
using Shared.Models;

namespace FactCheckingService.FactCheckers.GeneralStrategy.Models;

internal class GeneralFactCheckFunctionCallArguments
{
    [JsonProperty(PropertyName = "fact_check")]
    public GptResponseFunctionCallFactCheck FactCheck { get; set; }
}
