using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Utils;
using SourceCollectingService.Audio.Models;
using System.Text.RegularExpressions;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace SourceCollectingService.Audio.Strategies.YouTube;

public class YouTubeAudioCollectingStrategy : IAudioCollectingStrategy
{
    private const string YoutubeVideoUrlPattern = @"^(https?://)?(www\.)?(youtube\.com/watch\?v=)[a-zA-Z0-9\-_]+(&.*)?$";
    private const string FileExtension = ".mp4";
    private const string YouTubeContext = "YouTube";

    private readonly YoutubeClient _youtubeClient;

    private readonly SourceCollectingOptions _sourceCollectionOptions;

    public YouTubeAudioCollectingStrategy(IOptions<SourceCollectingOptions> options)
    {
        _youtubeClient = new YoutubeClient();

        _sourceCollectionOptions = options.Value;
    }

    public bool IsCompatible(Source source)
    {
        if (source is null)
            return false;

        return IsCompatible(source.SourceUrl);
    }

    private static bool IsValidSource(Source source)
        => source is not null && !string.IsNullOrWhiteSpace(source.SourceUrl);

    public bool IsCompatible(string sourceUrl)
    {
        if(string.IsNullOrWhiteSpace(sourceUrl)) 
            return false;

        return IsYoutubeVideoUrl(sourceUrl);
    }

    private static bool IsYoutubeVideoUrl(string url)
        => Regex.IsMatch(url, YoutubeVideoUrlPattern);

    public async Task<AudioCollectingResult> CollectMetaData(string sourceUrl)
    {
        var result = new AudioCollectingResult();

        if (!IsCompatible(sourceUrl))
        {
            result.Messages.Add($"SourceUrl incompatible with {nameof(YouTubeAudioCollectingStrategy)}: {sourceUrl}");
            return result;
        }

        var video = await _youtubeClient.Videos.GetAsync(sourceUrl);
        if (video is null)
        {
            result.Messages.Add($"Failed to get video from url: {sourceUrl}");
            return result;
        }

        AddVideoMetaDataToResult(result, video);

        result.IsSuccess = true;

        return result;
    }

    public async Task<AudioCollectingResult> CollectAudio(Source source)
    {
        if (!IsValidSource(source))
            return null;

        var result = new AudioCollectingResult
        {
            SourceId = source.Id
        };

        if (!IsCompatible(source))
        {
            result.Messages.Add($"Source incompatible {source.Id} url: {source.SourceUrl}");
            return result;
        }

        var video = await _youtubeClient.Videos.GetAsync(source.SourceUrl);
        if (video is null)
        {
            result.Messages.Add($"Failed to get video from source {source.Id} url: {source.SourceUrl}");
            return result;
        }

        AddVideoMetaDataToResult(result, video);

        return await CollectAudioInternal(source, result);
    }

    private static void AddVideoMetaDataToResult(AudioCollectingResult result, Video? video)
    {
        result.SourceType = SourceTypes.Video;

        result.SourceUrl = video.Url;
        result.Title = video.Title;
        result.Description = video.Description;
        result.Author = video.Author.ChannelTitle;
        result.Context = YouTubeContext;
        result.CreatedDate = video.UploadDate;
        result.Duration = video.Duration;
        result.Keywords = video.Keywords;
        result.Thumbnails = video.Thumbnails;
    }

    private async Task<AudioCollectingResult> CollectAudioInternal(Source source, AudioCollectingResult result)
    {
        var filePath = CreateFilePath(result.Title);
        
        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(source.SourceUrl);

        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        using (var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo))
        {
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            await stream.CopyToAsync(fileStream);
        };

        result.AudioFilePath = filePath;

        result.IsSuccess = true;

        return result;
    }

    private string CreateFilePath(string videoTitle)
    {
        var cleanedTitle = FileNameGeneration.CreateFileNameFromString(videoTitle);

        var filePath = Path.Combine(_sourceCollectionOptions.AudioFilesFolderPath, cleanedTitle);

        var filePathWithExtension = filePath + FileExtension;

        var finalFilePath = FileNameGeneration.EnsureFilePathIsUnique(filePathWithExtension);

        return finalFilePath;
    }
}
