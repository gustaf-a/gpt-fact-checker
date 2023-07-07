using FactCheckingService.Models;
using Shared.Extensions;
using Shared.Models;

namespace FactCheckingService.Extensions;

public static class GptResponseExtensions
{
    public static FactCheck ConvertToFactCheck(this GptResponseFunctionCallFactCheck functionCallFactCheck)
    {
        if (functionCallFactCheck is null)
            return null;

        return new FactCheck
        {
            Id = functionCallFactCheck.Id,
            Label = functionCallFactCheck.Label,
            FactCheckText = functionCallFactCheck.Explanation,
            References = functionCallFactCheck.ReferencesUsed.IsNullOrEmpty() ? new() : functionCallFactCheck.ReferencesUsed
        };
    }

}
