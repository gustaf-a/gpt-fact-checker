using Microsoft.Extensions.Options;
using Shared.Configuration;
using SourceCollectingService.Transcription.Models;

namespace SourceCollectingService.Transcription.Strategies.OpenAIWhisper;

public class OpenAiWhisperStrategy : ITranscriptionStrategy
{
    private readonly OpenAiOptions _openAiOptions;

    private readonly IHttpClientFactory _httpClientFactory;

    public OpenAiWhisperStrategy(IOptions<OpenAiOptions> options, IHttpClientFactory httpClientFactory)
    {
        _openAiOptions = options.Value;

        if (string.IsNullOrWhiteSpace(_openAiOptions.WhisperApiKey))
            throw new ArgumentNullException(nameof(_openAiOptions.WhisperApiKey));

        _httpClientFactory = httpClientFactory;
    }

    public bool IsCompatible(TranscriptionRequest transcriptionRequest)
    {
        if (transcriptionRequest.Source is null)
            throw new ArgumentNullException(nameof(transcriptionRequest.Source));

        if (!_openAiOptions.WhisperSupportedLanguages.Contains(transcriptionRequest.Source.Language))
            return false;

        return true;
    }

    public async Task<TranscriptionResult> Execute(TranscriptionRequest transcriptionRequest, int audioFilePathIndex)
    {
        if (!IsCompatible(transcriptionRequest))
            return null;

        var filePath = transcriptionRequest.AudioFiles[audioFilePathIndex];

        var transcriptionResult = new TranscriptionResult
        {
            SourceId = transcriptionRequest.Source.Id
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _openAiOptions.WhisperApiKey);

            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(File.OpenRead(filePath));

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            content.Add(fileContent, "file", Path.GetFileName(filePath));
            content.Add(new StringContent(_openAiOptions.WhisperApiModel), "model");

            var response = await client.PostAsync(_openAiOptions.WhisperApiEndPoint, content);

            if (!response.IsSuccessStatusCode)
            {
                transcriptionResult.Messages.Add("Error: " + response.StatusCode);
                return transcriptionResult;
            }

            string result = await response.Content.ReadAsStringAsync();

            transcriptionResult.RawTranscription = result;
            transcriptionResult.IsSuccess = true;
        }
        catch (Exception e)
        {
            transcriptionResult.Messages.Add("Transcription using OpenAI Whisper failed. Exception: " + e.Message);
        }

        return transcriptionResult;
    }
}