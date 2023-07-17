namespace Shared.Models;

public class FactCheckResult
{
    public Fact Fact { get; set; }
    public FactCheck? FactCheck { get; set; }
    public Author Author { get; set; }
    public bool IsFactChecked { get; set; } = false;
    public bool IsStored { get; set; } = false;
    public List<string> Messages { get; set; } = new();
}
