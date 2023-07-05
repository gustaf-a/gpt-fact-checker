using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactCheckingService.FactCheckers.GeneralStrategy.FactCheckPrompt;

public class GeneralFactCheckPrompt : PromptBase, IGeneralFactCheckPrompt
{
    private const string FunctionsFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\FactCheckWithData\climateFactCheckWithDataFunctions.json";

    private const string Delimiter = "####";

    private const string SystemPrompt =
$"""
Act as a knowledgable expert fact checker.
""";

    public async Task<Prompt> GetPrompt(Fact fact)
    {
        var systemPrompt = SystemPrompt;

        string userPrompt = CreateUserPrompt(fact);

        return await GetPrompt(systemPrompt, userPrompt, FunctionsFilePath);
    }

    private static string CreateUserPrompt(Fact fact)
    {
        var sb = new StringBuilder();

        sb.Append($"Claim ID: {fact.Id} ");
        sb.Append(Delimiter);
        sb.Append(fact.ClaimRawText);
        sb.Append(Delimiter);

        return sb.ToString();
    }
}
