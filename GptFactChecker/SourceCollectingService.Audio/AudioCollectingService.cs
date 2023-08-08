using Shared.Models;
using SourceCollectingService.Audio.Models;
using SourceCollectingService.Audio.Strategies;

namespace SourceCollectingService.Audio;

public class AudioCollectingService : IAudioCollectingService
{
    private readonly IEnumerable<IAudioCollectingStrategy> _strategies;

    public AudioCollectingService(IEnumerable<IAudioCollectingStrategy> strategies)
    {
        _strategies = strategies;
    }

    public async Task<AudioCollectingResponse> GetMetaData(string sourceUrl)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
            throw new ArgumentNullException(nameof(sourceUrl));

        var response = new AudioCollectingResponse();

        foreach (var strategy in _strategies)
        {
            if (!strategy.IsCompatible(sourceUrl))
                continue;

            var sourceCollectingResult = await TryCollectMetaDataWithStrategy(strategy, response, sourceUrl);
            if (sourceCollectingResult is null)
            {
                response.Messages.Add($"Failed to collect meta data from {sourceUrl}.");
                continue;
            }

            response.Messages.AddRange(sourceCollectingResult.Messages);

            if (!sourceCollectingResult.IsSuccess)
            {
                response.Messages.Add($"Failed to collect meta data with {nameof(strategy)} from {sourceUrl}.");
                continue;
            }

            response.Results.Add(sourceCollectingResult);
            response.IsSuccess = true;

            break;
        }

        return response;
    }

    private static async Task<AudioCollectingResult> TryCollectMetaDataWithStrategy(IAudioCollectingStrategy strategy, AudioCollectingResponse response, string sourceUrl)
    {
        try
        {
            var audioCollectingResult = await strategy.CollectMetaData(sourceUrl);

            return audioCollectingResult;
        }
        catch (Exception ex)
        {
            response.Messages.Add($"Failed to collect data from {sourceUrl}. Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<AudioCollectingResponse> GetAudio(Source source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (string.IsNullOrWhiteSpace(source.SourceUrl))
            throw new ArgumentNullException(nameof(source.SourceUrl));

        var response = new AudioCollectingResponse
        {
            SourceId = source.Id
        };

        foreach (var strategy in _strategies)
        {
            if (!strategy.IsCompatible(source))
                continue;

            var sourceCollectingResult = await TryCollectAudioWithStrategy(strategy, response, source);
            if (sourceCollectingResult is null)
            {
                response.Messages.Add($"Failed to collect data from source: {source.Id}.");
                continue;
            }

            response.Messages.AddRange(sourceCollectingResult.Messages);

            if (!sourceCollectingResult.IsSuccess)
            {
                response.Messages.Add($"Failed to collect audio with {nameof(strategy)} from source: {source.Id}.");
                continue;
            }
            
            response.Results.Add(sourceCollectingResult);
            response.IsSuccess = true;

            break;
        }

        return response;
    }

    private static async Task<AudioCollectingResult> TryCollectAudioWithStrategy(IAudioCollectingStrategy strategy, AudioCollectingResponse response, Source source)
    {
        try
        {
            var audioCollectingResult = await strategy.CollectAudio(source);

            return audioCollectingResult;
        }
        catch (Exception ex)
        {
            response.Messages.Add($"Failed to collect data from source: {source.Id}. Exception: {ex.Message}");
            return null;
        }
    }
}
