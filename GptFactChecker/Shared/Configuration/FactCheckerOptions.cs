namespace Shared.Configuration;

public class FactCheckerOptions
{
    public const string FactChecker = "FactChecker";

    public int TopicIdentificationCalls { get; set; } = 3;

    /// <summary>
    /// Saves checked facts directly without giving user time to see or interact with them.
    /// </summary>
    public bool SaveFactChecksDirectlyAfterCreation { get; set; } = false;

    public bool ReturnTestData { get; set; } = false;
}
