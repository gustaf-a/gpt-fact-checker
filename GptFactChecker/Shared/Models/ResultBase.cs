namespace Shared.Models;

public abstract class ResultBase
{
    public bool IsSuccess { get; set; }
    public List<string> Messages { get; set; } = new();
}
