using SourceCollectingService.Models;

namespace SourceCollectingService;

public interface ISourceCollectorService
{
    public Task<SourceCollectingResponse> CollectSource(SourceCollectingRequest sourceCollectingRequest);
    public Task<SourceCollectingResponse> CreateSource(string sourceUrl);
}
