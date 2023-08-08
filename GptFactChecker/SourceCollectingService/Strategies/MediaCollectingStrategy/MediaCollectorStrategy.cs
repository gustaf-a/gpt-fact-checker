using Shared.Models;
using SourceCollectingService.Audio;
using SourceCollectingService.Audio.Models;
using SourceCollectingService.Models;
using SourceCollectingService.Transcription;
using SourceCollectingService.Transcription.Models;

namespace SourceCollectingService.Strategies.MediaCollectingStrategy;

public class MediaCollectorStrategy : IMediaCollectorStrategy
{
    private readonly IAudioCollectingService _audioCollectingService;
    private readonly ITranscribingService _transcribingService;

    public MediaCollectorStrategy(IAudioCollectingService audioCollectingService, ITranscribingService transcribingService)
    {
        _audioCollectingService = audioCollectingService;
        _transcribingService = transcribingService;
    }

    public async Task<SourceCollectingResult> CollectUrlMetaData(string sourceUrl)
    {
        var result = new SourceCollectingResult();

        if (!IsValidUrl(sourceUrl))
        {
            result.Messages.Add($"Invalid source URL encountered: {sourceUrl}");
            return result;
        }

        if (!await TryGetMetaData(result, sourceUrl))
        {
            result.Messages.Add($"Meta data collection failed for URL: {sourceUrl}.");
            return result;
        }

        result.IsSuccess = true;
        return result;
    }

    private async Task<bool> TryGetMetaData(SourceCollectingResult result, string sourceUrl)
    {
        AudioCollectingResponse audioCollectingResponse = null;

        try
        {
            audioCollectingResponse = await _audioCollectingService.GetMetaData(sourceUrl);
        }
        catch (Exception ex)
        {
            result.Messages.Add($"Failed to collect meta data from {sourceUrl}. Exception: {ex.Message}");
        }

        if (audioCollectingResponse is null)
        {
            result.Messages.Add($"Failed to get valid response from meta data collecting service.");
            return false;
        }

        if (!audioCollectingResponse.IsSuccess)
        {
            result.Messages.AddRange(audioCollectingResponse.Messages);
            return false;
        }

        result.AudioCollectingResults = audioCollectingResponse.Results;
        return true;
    }

    public async Task<SourceCollectingResult> CollectSource(Source source)
    {
        var result = new SourceCollectingResult();

        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (!IsValidUrl(source.SourceUrl))
        {
            result.Messages.Add($"Invalid source URL encountered: {source.SourceUrl}");
            return result;
        }

        if (!await TryGetAudio(result, source))
        {
            result.Messages.Add($"Audio collection failed for source: {source.Id}.");
            return result;
        }

        if (!await TryGetTranscription(result, source))
        {
            result.Messages.Add($"Transcription failed for source: {source.Id}.");
            return result;
        }

        result.IsSuccess = true;
        return result;
    }

    private async Task<bool> TryGetAudio(SourceCollectingResult result, Source source)
    {
        AudioCollectingResponse audioCollectingResponse = null;

        try
        {
            audioCollectingResponse = await _audioCollectingService.GetAudio(source);
        }
        catch (Exception ex)
        {
            result.Messages.Add($"Failed to collect audio from source: {source.SourceUrl}. Exception: {ex.Message}");
        }

        if (audioCollectingResponse is null)
        {
            result.Messages.Add($"Failed to get valid response from audio collecting service.");
            return false;
        }

        if (!audioCollectingResponse.IsSuccess)
        {
            result.Messages.AddRange(audioCollectingResponse.Messages);
            return false;
        }

        result.AudioCollectingResults = audioCollectingResponse.Results;
        return true;
    }

    private async Task<bool> TryGetTranscription(SourceCollectingResult result, Source source)
    {
        var transcriptionRequest = new TranscriptionRequest
        {
            Source = source,
            AudioFiles = result.AudioCollectingResults.Select(a => a.AudioFilePath).ToList(),
        };

        TranscriptionResponse transcriptionResponse = null;

        try
        {
            transcriptionResponse = await _transcribingService.GetTranscription(transcriptionRequest);
        }
        catch (Exception ex)
        {
            result.Messages.Add($"Failed to transcribe audio from source: {source.SourceUrl}. Exception: {ex.Message}");
        }

        if (transcriptionResponse is null)
        {
            result.Messages.Add($"Failed to get valid response from audio collecting service.");
            return false;
        }

        result.TranscriptionResults = transcriptionResponse.Results;

        if (!transcriptionResponse.IsSuccess)
        {
            result.Messages.AddRange(transcriptionResponse.Messages);
            return false;
        }
        
        return true;
    }

    private static bool IsValidUrl(string sourceUrl)
    {
        if(string.IsNullOrWhiteSpace(sourceUrl))
            return false;

        bool result = Uri.TryCreate(sourceUrl, UriKind.Absolute, out var uriResult)
            && Uri.IsWellFormedUriString(sourceUrl, UriKind.Absolute);

        return result;
    }
}
