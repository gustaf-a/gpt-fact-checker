using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;
using SourceCollectingService.Models;
using SourceCollectingService.Strategies;
using SourceCollectingService.Transcription.Models;

namespace SourceCollectingService;

public class SourceCollectorService : ISourceCollectorService
{
    private readonly SourceCollectingOptions _sourceCollectingOptions;
    private readonly ICollectorStrategyFactory _collectingStrategyFactory;

    public SourceCollectorService(IOptions<SourceCollectingOptions> options, ICollectorStrategyFactory collectingStrategyFactory)
    {
        _sourceCollectingOptions = options.Value;
        _collectingStrategyFactory = collectingStrategyFactory;
    }

    public async Task<SourceCollectingResponse> CreateSource(string sourceUrl)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
            throw new ArgumentNullException(nameof(sourceUrl));

        var response = new SourceCollectingResponse();

        foreach (var sourceType in SourceTypes.AllTypes)
        {
            try
            {
                var collectingStrategy = _collectingStrategyFactory.CreateStrategy(sourceType);

                var sourceCollectingResult = await collectingStrategy.CollectUrlMetaData(sourceUrl);

                if (sourceCollectingResult is null || sourceCollectingResult.IsSuccess == false)
                    continue;

                response.Messages.AddRange(sourceCollectingResult.Messages);

                response.CollectedSource = CreateCollectedSource(sourceCollectingResult);

                response.IsSuccess = sourceCollectingResult.IsSuccess;

                return response;
            }
            catch (Exception)
            {
                response.Messages.Add($"Exception encountered when trying to collect metadata from {sourceUrl} using sourceType {sourceType}.");
            }
        }

        response.Messages.Add($"Failed to collect meta data from {sourceUrl}");

        return response;
    }

    public async Task<SourceCollectingResponse> CollectSource(SourceCollectingRequest sourceCollectingRequest)
    {
        if (sourceCollectingRequest is null)
            throw new ArgumentNullException(nameof(sourceCollectingRequest));

        var source = sourceCollectingRequest.Source;

        if (source is null)
            throw new ArgumentNullException(nameof(source));

        var response = new SourceCollectingResponse
        {
            SourceId = source.Id
        };

        try
        {
            var collectingStrategy = _collectingStrategyFactory.CreateStrategy(source.SourceType);

            var sourceCollectingResult = await collectingStrategy.CollectSource(source);

            if (sourceCollectingResult is null)
            {
                response.Messages.Add($"Failed to collect data from source: {source.Id}.");
                return response;
            }

            response.Messages.AddRange(sourceCollectingResult.Messages);

            response.CollectedSource = CreateCollectedSource(sourceCollectingResult);

            response.IsSuccess = sourceCollectingResult.IsSuccess;
        }
        catch (Exception ex)
        {
            response.Messages.Add($"Failed to collect data from source: {source.Id}. Exception: {ex.Message}");
        }

        return response;
    }

    private Source CreateCollectedSource(SourceCollectingResult sourceCollectingResult)
    {
        var source = new Source
        {
            Tags = new()
        };

        if (sourceCollectingResult.AudioCollectingResults is not null)
        {
            var audioCollectingResults = sourceCollectingResult.AudioCollectingResults;

            if (audioCollectingResults.Any())
            {
                var firstResult = audioCollectingResults.First();

                source.SourceType = firstResult.SourceType;

                source.Id = firstResult.SourceId;
                source.SourceUrl = firstResult.SourceUrl;
                source.Name = firstResult.Title;
                source.Description = firstResult.Description;
                source.SourcePerson = firstResult.Author;
                source.SourceContext = firstResult.Context;

                source.SourceCreatedDate = firstResult.CreatedDate.ToIsoString(includeTime: false);
                source.SourceImportedDate = DateTimeOffset.UtcNow.ToIsoString(includeTime: false);

                if (!firstResult.Thumbnails.IsNullOrEmpty())
                    source.CoverImageUrl = firstResult.Thumbnails[0].Url;

                foreach (var audioCollectingResult in audioCollectingResults)
                    source.Tags.AddRange(audioCollectingResult.Keywords);
            }
        }

        if (sourceCollectingResult.TranscriptionResults is not null
            && sourceCollectingResult.TranscriptionResults.Any())
        {
            if (string.IsNullOrEmpty(source.Id))
                source.Id = sourceCollectingResult.TranscriptionResults.First().SourceId;

            source.RawText = GetRawText(sourceCollectingResult.TranscriptionResults);
        }

        return source;
    }

    private string GetRawText(List<TranscriptionResult> transcriptionResults)
    {
        return string.Join(_sourceCollectingOptions.TranscriptionResultsConcatenatingCharacter, transcriptionResults.Select(tr => tr.RawTranscription));
    }
}
