﻿namespace GptFactCheckerApi.Model;

public class ClaimCheckReactionDto
{
    public string Id { get; set; }
    public string ClaimCheckId { get; set; }
    public string UserId { get; set; }
    public int Reaction { get; set; } //-1, 0, +1
}
