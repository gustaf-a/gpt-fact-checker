namespace GptFactCheckerApi.Model;

public class ClaimCheck
{
    public string Id { get; set; }
    public string CreatorId { get; set; }
    public string ClaimId { get; set; }
    public string Label { get; set; }
    public string ClaimCheckText { get; set; }
}
