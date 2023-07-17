using FactCheckingService.Models;
using Shared.Models;
using Shared.Utils;

namespace FactCheckingService.Extensions;

public static class GptResponseExtensions
{
    public static FactCheck ConvertToFactCheck(this GptResponseFunctionCallFactCheck functionCallFactCheck)
    {
        if (functionCallFactCheck is null)
            return null;

        return new FactCheck
        {
            Id = IdGeneration.GenerateStringId(),
            Label = functionCallFactCheck.Label ?? string.Empty,
            FactCheckText = functionCallFactCheck.Explanation
        };
    }

}
