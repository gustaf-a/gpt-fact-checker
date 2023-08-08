using Shared.Models;

namespace SourceCollectingService.Models;

public class SourceCollectingResponse : ResultBase
{
    public string SourceId { get; set; }
    public Source CollectedSource { get; set; }
}