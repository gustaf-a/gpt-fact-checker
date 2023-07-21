namespace Shared.Models;

public class Topic : IIdentifiable
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; }
    public List<string> Tags { get; set; } = new();

    public List<Reference>? References { get; set; } = null;
}
