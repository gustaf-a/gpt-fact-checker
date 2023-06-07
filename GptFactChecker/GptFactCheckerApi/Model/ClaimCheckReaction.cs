namespace GptFactCheckerApi.Model;

public class ClaimCheckReaction
{
    public string Id { get; set; }
    public string UserId { get; set; }

    /// <summary>
    /// -1, 0, +1
    /// </summary>
    public int Reaction { get; set; }
}
