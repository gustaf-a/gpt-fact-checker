using Shared.Models;

namespace FactExtractionService.FactExtractors.FunctionCallingStrategy.FactExtractionPrompt;

public interface IFactExtractionFunctionCallingPrompt
{
    public Prompt GetPrompt(string rawText, string contextText, string modelToUse);
}
