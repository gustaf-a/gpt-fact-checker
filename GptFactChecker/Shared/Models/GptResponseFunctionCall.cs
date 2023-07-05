﻿using Newtonsoft.Json;

namespace Shared.Models;

public class GptResponseFunctionCall
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "arguments")]
    public string Arguments { get; set; }
}
