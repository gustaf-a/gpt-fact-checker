namespace Shared.Models;

public class ArgumentData
{
    public int Id { get; set; }
    public string? ArgumentText { get; set; }
    public string? ArgumentTextKeyWords { get; set; }
    public string? CounterArgumentSummary { get; set; }
    public string CounterArgumentText { get; set; } = string.Empty;
    public string ReferenceUrl { get; set; } = string.Empty;
}
