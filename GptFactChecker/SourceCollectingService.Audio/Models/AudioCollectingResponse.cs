using Shared.Models;

namespace SourceCollectingService.Audio.Models;

public class AudioCollectingResponse : ResultBase
{
    public string SourceId { get; internal set; }
    public List<AudioCollectingResult> Results { get; set; } = new();
}
