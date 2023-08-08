using Shared.Models;

namespace SourceCollectingService.Transcription.Models;

public class TranscriptionRequest
{
    public Source Source { get; set; }
    public List<string> AudioFiles { get; set; }
}
