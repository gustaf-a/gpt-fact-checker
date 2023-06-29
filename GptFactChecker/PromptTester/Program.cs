using GptFactCheckerApi.Repository.JsonRepo;
using Microsoft.Extensions.Configuration;
using Shared.Configuration;
using Shared.GptClient;

namespace PromptTester;

internal class Program
{
    private const string PromptToTestFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\promptToTest.json";
    private const string ApiKeyFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\apikey.json";

    static async Task Main(string[] args)
    {
        var openAiOptions = await GetOpenAiOptions();
        var gptClient = new GptClient(openAiOptions);

        Console.WriteLine("GPT client initialized");

        var promptToTest = await JsonHelper.GetObjectFromJson<string>(PromptToTestFilePath);
        if (string.IsNullOrWhiteSpace(promptToTest))
            throw new Exception("Failed to find a valid prompt.");

        Console.WriteLine("Found prompt. Sending to GPT");

        var result = await gptClient.GetCompletion(promptToTest);

    }

    private static async Task<OpenAiOptions> GetOpenAiOptions()
    {
        var options = await JsonHelper.GetObjectFromJson<Dictionary<string, string>>(ApiKeyFilePath);

        var apikey = options["ApiKey"];

        if (string.IsNullOrWhiteSpace(apikey))
        {
            Console.WriteLine("No valid API key found. Please provide an API key:");

            apikey = Console.ReadLine();
        }

        if (string.IsNullOrWhiteSpace(apikey))
            throw new Exception("No API key provided.");

        return new OpenAiOptions
        {
            ApiKey = apikey
        };
    }
}