using Shared.Models;
using SourceCollectingService.Transcription.Models;

namespace SourceCollectingService.Transcription.Strategies;

public interface ITranscriptionStrategy
{
    public bool IsCompatible(TranscriptionRequest transcriptionRequest);
    public Task<TranscriptionResult> Execute(TranscriptionRequest transcriptionRequest, int audioFilePathIndex);
}
