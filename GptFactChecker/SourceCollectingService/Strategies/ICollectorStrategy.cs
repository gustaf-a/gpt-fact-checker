using Shared.Models;
using SourceCollectingService.Models;

namespace SourceCollectingService.Strategies;

public interface ICollectorStrategy
{
    public Task<SourceCollectingResult> CollectSource(Source source);
    public Task<SourceCollectingResult> CollectUrlMetaData(string sourceUrl);
}
