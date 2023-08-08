using GptFactCheckerApi.Model;
using GptFactCheckerApiController.Model;
using SourceCollectingService;
using SourceCollectingService.Models;

namespace GptFactCheckerApi.Services;

public class SourceExtractorService : ISourceExtractorService
{
    private readonly ISourceCollectorService _sourceCollectorService;
    private readonly ISourceService _sourceService;

    public SourceExtractorService(ISourceService sourceService, ISourceCollectorService sourceCollectorService)
    {
        _sourceService = sourceService;
        _sourceCollectorService = sourceCollectorService;
    }

    public async Task<BackendResponse<SourceCollectingResponseDto>> CreateSourceFromUrl(string sourceUrl)
    {
        var response = new BackendResponse<SourceCollectingResponseDto>();

        var collectorServiceResponse = await _sourceCollectorService.CreateSource(sourceUrl);

        response.Data = collectorServiceResponse.ToDto();
        response.IsSuccess = collectorServiceResponse.IsSuccess;

        return response;
    }

    public async Task<BackendResponse<SourceCollectingResponseDto>> ExtractSource(string id)
    {
        var response = new BackendResponse<SourceCollectingResponseDto>();

        var sourceDto = await _sourceService.GetSourceById(id, false);
        if (sourceDto is null)
        {
            LogError(response, $"Failed to retrieve source with id {id}");
            return response;
        }

        var sourceCollectingRequest = new SourceCollectingRequest
        {
            Source = sourceDto.ToSource()
        };


        var collectorServiceResponse = await _sourceCollectorService.CollectSource(sourceCollectingRequest);

        response.Data = collectorServiceResponse.ToDto();
        response.IsSuccess = collectorServiceResponse.IsSuccess;

        return response;
    }

    private static void LogError<T>(BackendResponse<T> response, string message)
    {
        Console.WriteLine(message);

        response.Messages.Add(message);
    }
}
