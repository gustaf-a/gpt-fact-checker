using Shared.Models;
using SourceCollectingService.Audio.Models;

namespace SourceCollectingService.Audio;

public interface IAudioCollectingService
{
    public Task<AudioCollectingResponse> GetAudio(Source source);
    public Task<AudioCollectingResponse> GetMetaData(string sourceUrl);
}
