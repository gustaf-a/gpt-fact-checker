namespace GptFactCheckerApi.Model;

public class Source
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }

    public string SourceType { get; set; }
    public string SourcePerson { get; set; }
    public string SourceContext { get; set; }
    public string SourceUrl { get; set; } 

    public DateOnly SourceImportedDate { get; set; }
    public DateOnly SourceCreatedDate { get; set; }

    public DateOnly ClaimsFirstExtractedDate { get; set; }
    public DateOnly ClaimsUpdatedDate { get; set; }
   
    public string CoverImageUrl { get; set; }
}
