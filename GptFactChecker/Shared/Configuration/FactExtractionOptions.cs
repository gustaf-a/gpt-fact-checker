namespace Shared.Configuration;

public class FactExtractionOptions
{
    public const string FactExtraction = "FactExtraction";

    /// <summary>
    /// Character limit for raw text added to fact extraction prompt.
    /// </summary>
    public int RawTextSizeLimit { get; set; } = 2000;
}
