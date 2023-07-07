using Shared.Models;

namespace FactExtractionService.FactExtractors.FunctionCallingStrategy.FactExtractionPrompt;

public interface IFactExtractionFunctionCallingPrompt
{
    public Task<Prompt> GetPrompt(string rawText);
}
