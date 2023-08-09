using FactCheckingService.Models;
using FactExtractionService.FactExtractors.FunctionCallingStrategy.FactExtractionPrompt;
using FactExtractionService.Models;
using FactExtractionService.Utils;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using GptHandler.GptClient;
using Shared.Models;

namespace FactExtractionService.FactExtractors.FunctionCallingStrategy;

public class FactExtractionWithFunctionCallingStrategy : IFactExtractionStrategy
{
    private readonly FactExtractionOptions _factExtractionOptions;
    private readonly OpenAiOptions _openAiOptions;

    private readonly IFactExtractionFunctionCallingPrompt _factExtractionFunctionCallingPrompt;
    private readonly IGptClient _gptClient;
    private readonly IGptResponseParser _gptResponseParser;

    private readonly ISourceSplitter _sourceSplitter;

    public FactExtractionWithFunctionCallingStrategy(IOptions<FactExtractionOptions> factExtractionOptions, IOptions<OpenAiOptions> openAiOptions, IFactExtractionFunctionCallingPrompt factExtractionFunctionCallingPrompt, IGptClient gptClient, IGptResponseParser gptResponseParser, ISourceSplitter sourceSplitter)
    {
        _factExtractionOptions = factExtractionOptions.Value;
        _openAiOptions = openAiOptions.Value;

        _factExtractionFunctionCallingPrompt = factExtractionFunctionCallingPrompt;
        _gptClient = gptClient;
        _gptResponseParser = gptResponseParser;
        _sourceSplitter = sourceSplitter;
    }

    private const string VersionNumber = "1.0";

    public async Task<List<ExtractedClaims>> ExtractFacts(Source source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (IsInvalid(source))
        {
            Console.WriteLine($"Fact extraction failed for {source.Id}: Invalid source.");
            return null;
        }

        var contextText = GetSourceContext(source);

        var splitRawTexts = GetRawSourceTexts(source);

        var results = new List<ExtractedClaims>();

        var modelToUse = _factExtractionOptions.UseExpensiveServices ? _openAiOptions.ApiModelExpensive : _openAiOptions.ApiModelBase;

        foreach (var rawText in splitRawTexts)
        {
            var factExtractionPrompt = _factExtractionFunctionCallingPrompt.GetPrompt(rawText, contextText, modelToUse);
            if (factExtractionPrompt is null)
            {
                Console.WriteLine($"Fact extraction failed for {source.Id}: Failed to create prompt.");
                return null;
            }

            var factExtractionResponse = await _gptClient.GetCompletion(factExtractionPrompt);

            var gptResponseFunctionCallFactCheck = _gptResponseParser.ParseGptResponseFunctionCall<GptResponseFunctionCallFactExtraction>(factExtractionResponse, nameof(FactExtractionWithFunctionCallingStrategy));

            if (gptResponseFunctionCallFactCheck is null || gptResponseFunctionCallFactCheck.ExtractedClaims.IsNullOrEmpty())
            {
                Console.WriteLine($"Fact extraction failed for {source.Id}: Failed to parse response: {factExtractionResponse}");
                return null;
            }

            results.AddRange(gptResponseFunctionCallFactCheck.ExtractedClaims);
        }

        return results;
    }

    private List<string> GetRawSourceTexts(Source source)
    {
        var maxCharacters = _factExtractionOptions.RawTextSizeLimit;

        var rawTextSource = source.RawText;

        return _sourceSplitter.SplitSourceText(rawTextSource, maxCharacters);
    }

    private string GetSourceContext(Source source)
    {
        var context = source.Description;

        if (context.Length < _factExtractionOptions.ContextTextSizeLimit)
            return context;

        return context[.._factExtractionOptions.ContextTextSizeLimit];
    }

    private static bool IsInvalid(Source source)
        => string.IsNullOrWhiteSpace(source.RawText);
}