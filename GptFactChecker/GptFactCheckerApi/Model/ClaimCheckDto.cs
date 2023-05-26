namespace GptFactCheckerApi.Model;

public class ClaimCheckDto
{
    public string Id { get; set; }
    public string CreatorId { get; set; }
    public string ClaimId { get; set; }
    public string Label { get; set; }
    public string ClaimCheckText { get; set; }

    public List<ClaimCheckReactionDto> ClaimCheckReactions { get; set; }
}
