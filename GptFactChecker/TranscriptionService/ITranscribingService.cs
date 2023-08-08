using SourceCollectingService.Transcription.Models;

namespace SourceCollectingService.Transcription;

public interface ITranscribingService
{
    public Task<TranscriptionResponse> GetTranscription(TranscriptionRequest transcriptionRequest);
}
