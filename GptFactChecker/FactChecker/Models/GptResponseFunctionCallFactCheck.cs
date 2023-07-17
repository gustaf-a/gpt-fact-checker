namespace FactCheckingService.Models;

public class GptResponseFunctionCallFactCheck
{
    public string? Label { get; set; }
    public string Explanation { get; set; }
}
