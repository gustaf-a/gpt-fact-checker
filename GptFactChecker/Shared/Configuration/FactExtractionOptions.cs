namespace Shared.Configuration;

public class FactExtractionOptions
{
    public const string FactExtraction = "FactExtraction";

    /// <summary>
    /// Character limit for raw text added to fact extraction prompt.
    /// </summary>
    public int RawTextSizeLimit { get; set; } = 2000;

    /// <summary>
    /// Saves extracted facts directly after extraction without giving user time to see or interact with them.
    /// </summary>
    public bool SaveExtractedFactsDirectlyAfterExtraction { get; set; } = false;
}
