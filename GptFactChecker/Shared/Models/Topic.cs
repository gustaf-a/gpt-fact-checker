namespace Shared.Models;

public class Topic : IIdentifiable, IComparable<Topic>
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; }
    public List<string> Tags { get; set; } = new();
    public int Priority { get; set; } = 50;

    public List<Reference>? References { get; set; } = null;

    public int CompareTo(Topic? other)
    {
        if(other is null) 
            return -1; 

        return Priority.CompareTo(other.Priority);
    }
}
