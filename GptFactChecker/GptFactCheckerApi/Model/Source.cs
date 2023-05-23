namespace GptFactCheckerApi.Model;

public class Source
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SourceType { get; set; }
    public string ImageUrl { get; set; }

    public string Description { get; set; }
    
    public string Url { get; set; } 
    public string RawText { get; set; }
    
    public List<string> Authors { get; set; }
    public string Category { get; set; }

    public List<Source> ChildSources { get; set; }
}
