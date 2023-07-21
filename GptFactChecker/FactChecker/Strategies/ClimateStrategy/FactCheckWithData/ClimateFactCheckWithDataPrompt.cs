using Shared.Models;
using Shared.Prompts;
using System.Text;

namespace FactCheckingService.Strategies.ClimateStrategy.FactCheckWithData;

public class ClimateFactCheckWithDataPrompt : PromptWithReferencesBase, IClimateFactCheckWithDataPrompt
{
    private const string FunctionsFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\FactCheckWithData\climateFactCheckWithDataFunctions.json";

    public async Task<Prompt> GetPrompt(Fact fact, List<ArgumentData> relevantArguments)
    {
        Prompt factCheckPrompt = await base.GetPrompt(new List<Fact>{ fact }, relevantArguments, FunctionsFilePath);

        return factCheckPrompt;
    }

    private const string SystemPromptStart =
$"""
Act as an expert fact checker specialized in climate change, environmental issues and sustainability. Use the references provided to fact check the presented claim. When a reference is used the ID should be written like this in the explanation: (ref:123)
""";

    protected override string BuildSystemPrompt(List<ArgumentData> relevantArguments)
    {
        var sb = new StringBuilder();

        sb.AppendLine(SystemPromptStart);

        sb.AppendLine($"<{ReferenceTagText}>");

        foreach (var argument in relevantArguments)
            AddReference(sb, argument);

        sb.AppendLine($"</{ReferenceTagText}>");

        return sb.ToString();
    }

    private static void AddReference(StringBuilder sb, ArgumentData argument)
    {
        if (argument.Id == 0)
        {
            Console.WriteLine($"Invalid argument found: ID '{argument.Id}', CounterArgumentSummary:'{argument.CounterArgumentSummary}'.");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(argument.CounterArgumentSummary))
        {
            Console.WriteLine($"Invalid argument found: ID '{argument.Id}', CounterArgumentSummary is empty.");
            return;
        }

        sb.Append($"<{argument.Id}>");
        sb.Append(argument.CounterArgumentSummary);
        sb.AppendLine($"</{argument.Id}>");
    }

    protected override string BuildUserPrompt(List<Fact> facts)
    {
        var fact = facts[0];

        var sb = new StringBuilder();

        sb.Append(Delimiter);
        sb.Append(fact.ClaimRawText);
        sb.Append(Delimiter);

        return sb.ToString();
    }
}
