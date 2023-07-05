namespace FactCheckingService.FactCheckers.ClimateStrategy.Models;

public class ClimateFactCheck
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string Explanation { get; set; }
    public List<string>? ReferencesUsed { get; set; }
}
