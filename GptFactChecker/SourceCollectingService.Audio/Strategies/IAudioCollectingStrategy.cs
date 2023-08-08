using Shared.Models;
using SourceCollectingService.Audio.Models;

namespace SourceCollectingService.Audio.Strategies;

public interface IAudioCollectingStrategy
{
    public Task<AudioCollectingResult> CollectAudio(Source source);
    public Task<AudioCollectingResult> CollectMetaData(string sourceUrl);
    public bool IsCompatible(string sourceUrl);
    public bool IsCompatible(Source source);
}
