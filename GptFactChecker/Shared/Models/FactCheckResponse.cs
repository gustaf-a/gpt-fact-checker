namespace Shared.Models;

public class FactCheckResponse
{
    public Fact Fact { get; set; }
    public FactCheck? FactCheck { get; set; }
    public bool IsChecked { get; set; }
    public string Message { get; set; } = string.Empty;
}
