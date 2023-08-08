using Shared.Models;

namespace SourceCollectingService.Transcription.Models;

public class TranscriptionResponse : ResultBase
{
    public string SourceId { get; internal set; }
    public List<TranscriptionResult> Results { get; internal set; } = new();
}
