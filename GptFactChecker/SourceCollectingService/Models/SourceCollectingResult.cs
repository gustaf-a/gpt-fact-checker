using Shared.Models;
using SourceCollectingService.Audio.Models;
using SourceCollectingService.Transcription.Models;

namespace SourceCollectingService.Models;

public class SourceCollectingResult : ResultBase
{
    public List<AudioCollectingResult> AudioCollectingResults { get; set; } = new();
    public List<TranscriptionResult> TranscriptionResults { get; set; } = new();
}