﻿namespace Shared.Models;

public class ClaimCheck : IIdentifiable
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Label { get; set; }
    public string ClaimCheckText { get; set; }
    public string DateCreated { get; set; }
    public List<string> References { get; set; } = new();
}
