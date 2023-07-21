namespace Shared.Models;

public class Reference : IIdentifiable
{
    public string Id { get; set; } = string.Empty;
    public string? Text { get; set; }
    public string? TextSummary { get; set; }
    public List<string> TextKeyWords { get; set; } = new();
    public string Url { get; set; } = string.Empty;
    public DateTimeOffset DateRegistered { get; set; }
}
