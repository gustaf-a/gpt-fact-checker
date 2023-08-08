using GptFactCheckerApi.Model;
using Shared.Models;

namespace GptFactCheckerApiController.Model;

public class SourceCollectingResponseDto : ResultBase
{
    public string SourceId { get; set; }
    public SourceDto CollectedSource { get; set; }
}
