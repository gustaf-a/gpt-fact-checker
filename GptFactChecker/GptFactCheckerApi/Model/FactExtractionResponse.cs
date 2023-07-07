namespace GptFactCheckerApi.Model;

public class FactExtractionResponse
{
    public string SourceId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsStored { get; set; } = false;
    public List<ClaimDto> ExtractedClaims { get; set; } = new();
}
