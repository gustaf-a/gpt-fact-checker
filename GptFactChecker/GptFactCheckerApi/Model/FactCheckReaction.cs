namespace GptFactCheckerApi.Model;

public class FactCheckReaction
{
    public string Id { get; set; }
    public string FactCheckId { get; set; }
    public string UserId { get; set; }
    public int Reaction { get; set; } //-1, 0, +1
}
