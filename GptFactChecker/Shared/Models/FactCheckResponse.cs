namespace Shared.Models;

public class FactCheckResponse
{
    public Fact Fact { get; set; }
    public FactCheck? FactCheck { get; set; }
    public Author Author { get; set; }
    public bool IsChecked { get; set; }
    public bool IsStored { get; set; } = false;
    public List<string> Messages { get; set; } = new();
}
