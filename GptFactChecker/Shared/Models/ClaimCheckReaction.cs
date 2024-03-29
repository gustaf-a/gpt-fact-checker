﻿using Newtonsoft.Json;

namespace Shared.Models;

public class ClaimCheckReaction : IIdentifiable
{
    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "userId")]
    public string? UserId { get; set; }

    /// <summary>
    /// -1 | 0 | +1
    /// </summary>
    [JsonProperty(PropertyName = "reaction")]
    public int Reaction { get; set; } = 0;
}
