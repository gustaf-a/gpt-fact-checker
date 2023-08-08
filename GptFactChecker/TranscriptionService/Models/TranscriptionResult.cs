using Shared.Models;

namespace SourceCollectingService.Transcription.Models;

public class TranscriptionResult : ResultBase
{
    public string SourceId { get; set; }
    public string RawTranscription { get; set; }
}
