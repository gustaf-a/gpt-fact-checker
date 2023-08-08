namespace GptFactCheckerApi.Model;

public class SourceDto
{
    public string Id { get; set; }
    public string Name { get; set; } = "";
    public string? SourceUrl { get; set; } = "";
    public string? Language { get; set; } = "";
    public string? Description { get; set; } = "";
    public List<string>? Tags { get; set; } = new List<string>();

    public string? SourceType { get; set; } = "";
    public string? SourcePerson { get; set; } = "";
    public string? SourceContext { get; set; } = "";
    public string? SourceRawText { get; set; } = "";

    public string? SourceImportedDate { get; set; } = "";
    public string? SourceCreatedDate { get; set; } = "";

    public string? ClaimsFirstExtractedDate { get; set; } = "";
    public string? ClaimsUpdatedDate { get; set; } = "";

    public string? CoverImageUrl { get; set; } = "";

    public List<ClaimDto>? Claims { get; set; }
}
