using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactExtractionService.FactExtractors.FunctionCallingStrategy.FactExtractionPrompt;

public class FactExtractionFunctionCallingPrompt : PromptBase, IFactExtractionFunctionCallingPrompt
{
    private const string FunctionsFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\DataExtractionService\FactExtractors\FunctionCallingStrategy\FactExtractionPrompt\factExtractionFunctions.json";

    private const string RawTextTag = "raw_text";

    private const string SystemPrompt =
$"""
Act as an expert text analyzer processing text for later fact checking. You will be provided a longer text within tags <{RawTextTag}>. Extract all significant claims made in the text and present them as facts.
""";

    public async Task<Prompt> GetPrompt(string rawText)
    {
        var systemPrompt = SystemPrompt;

        var userPrompt = CreateUserPrompt(rawText);

        return await GetPrompt(systemPrompt, userPrompt, FunctionsFilePath);
    }

    private static string CreateUserPrompt(string rawText)
    {
        var sb = new StringBuilder();

        sb.Append($"Raw text:");
        sb.Append($"<{RawTextTag}>");
        sb.Append(rawText);
        sb.Append($"</{RawTextTag}>");

        return sb.ToString();
    }
}
