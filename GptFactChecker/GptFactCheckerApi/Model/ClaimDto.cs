namespace GptFactCheckerApi.Model;

public class ClaimDto
{
    public string Id { get; set; }
    public string ClaimSummarized { get; set; }
    public string? ClaimRawText { get; set; }
    public string[]? Tags { get; set; }
    public string? DateCreated { get; set; }

    public List<ClaimCheckDto>? ClaimChecks { get; set; }
}
