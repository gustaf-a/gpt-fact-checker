﻿using Newtonsoft.Json;

namespace FactCheckingService.FactCheckers.ClimateStrategy.Models;

public class ClaimWithReferences
{
    [JsonProperty(PropertyName = "claim_id")]
    public string ClaimId { get; set; }

    [JsonProperty(PropertyName = "reference_ids")]
    public List<string> ReferenceIds { get; set; }
}