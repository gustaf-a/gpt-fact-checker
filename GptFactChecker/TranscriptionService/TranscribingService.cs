using SourceCollectingService.Transcription.Models;
using SourceCollectingService.Transcription.Strategies;

namespace SourceCollectingService.Transcription;

public class TranscribingService : ITranscribingService
{
    private readonly IEnumerable<ITranscriptionStrategy> _transcriptionStrategies;

    public TranscribingService(IEnumerable<ITranscriptionStrategy> transcriptionStrategies )
    {
        _transcriptionStrategies = transcriptionStrategies;
    }

    public async Task<TranscriptionResponse> GetTranscription(TranscriptionRequest transcriptionRequest)
    {
        var response = new TranscriptionResponse();

        if(transcriptionRequest is null || transcriptionRequest.AudioFiles is null)
        {
            response.Messages.Add($"Invalid transcription request received.");
            return response;
        }

        if(transcriptionRequest.AudioFiles.Count == 0)
        {
            response.Messages.Add($"No files to transcribe found in request for source: {transcriptionRequest.Source?.Id}");
            return response;
        }

        bool compatibleStrategyFound = false;

        foreach (var strategy in _transcriptionStrategies)
        {
            if(!strategy.IsCompatible(transcriptionRequest))
                continue;

            compatibleStrategyFound = true;

            await ExecuteCompatibleStrategy(strategy, transcriptionRequest, response);

            if (response.IsSuccess)
                break;
        }

        if(!compatibleStrategyFound)
            response.Messages.Add($"Failed to find compatible transcription strategy for source: {transcriptionRequest.Source?.Id}");

        return response;
    }

    private static async Task ExecuteCompatibleStrategy(ITranscriptionStrategy strategy, TranscriptionRequest transcriptionRequest, TranscriptionResponse response)
    {
        for (int audioFilePathIndex = 0; audioFilePathIndex < transcriptionRequest.AudioFiles.Count; audioFilePathIndex++)
        {
            var audioFilePath = transcriptionRequest.AudioFiles[audioFilePathIndex];

            if (string.IsNullOrWhiteSpace(audioFilePath))
            {
                response.Messages.Add($"Invalid filepath found for audio file.");
                return;
            }

            if (!File.Exists(audioFilePath))
            {
                response.Messages.Add($"Failed to find audio file: {audioFilePath}");
                return;
            }

            var result = await strategy.Execute(transcriptionRequest, audioFilePathIndex);

            if(result.IsSuccess)
                response.IsSuccess = true;

            response.Results.Add(result);
        }
    }
}
