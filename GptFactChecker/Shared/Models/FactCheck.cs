namespace Shared.Models;

public class FactCheck
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string FactCheckText { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
    public List<string> References { get; set; } = new();
}
