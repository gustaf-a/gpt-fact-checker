namespace Shared.Configuration;

public class FactCheckerOptions
{
    public const string FactChecker = "FactChecker";

    public int TopicIdentificationCalls { get; set; } = 3;
}
