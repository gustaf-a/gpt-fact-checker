namespace Shared.Configuration;

public class FactExtractionOptions
{
    public const string FactExtraction = "FactExtraction";

    /// <summary>
    /// Character limit for raw text added to fact extraction prompt.
    /// </summary>
    public int RawTextSizeLimit { get; set; } = 1600;

    /// <summary>
    /// Character limit for context text added to fact extraction prompt.
    /// </summary>
    public int ContextTextSizeLimit { get; set; } = 500;

    /// <summary>
    /// Saves extracted facts directly after extraction without giving user time to see or interact with them.
    /// </summary>
    public bool SaveExtractedFactsDirectlyAfterExtraction { get; set; } = false;

    /// <summary>
    /// Uses more expensive services if available
    /// </summary>
    public bool UseExpensiveServices { get; set; } = false;
}
