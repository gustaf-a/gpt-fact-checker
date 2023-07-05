using GptFactCheckerApi.Repository.JsonRepo;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Configuration;
using Shared.GptClient;
using Shared.Models;
using Shared.Prompts;

namespace PromptTester;

internal class Program
{
    private const string PromptToTestFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\promptToTest.json";
    private const string ApiKeyFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\apikey.json";

    private const string FunctionsPropertiesToTestFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\PromptTest\functionsPropertiesTest.json";

    private const string ResponsesFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\responses.json";
    private const string TestResponseFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\PromptTester\testResponse.json";

    private const bool UseClassPrompt = false;
    private const bool UseTestResponse = false;

    static async Task Main(string[] args)
    {
        var openAiOptions = await GetOpenAiOptions();
        var options = Options.Create(openAiOptions);

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(h => h.CreateClient()).Returns(new HttpClient());

        var gptClient = new GptClient(httpClientFactoryMock.Object, options);

        Console.WriteLine("GPT client initialized");

        var responseContent = UseClassPrompt
                    ? await TestPromptClass(gptClient, openAiOptions)
                    : await TestJsonPrompt(gptClient);

        var gptResponse = JsonHelper.Deserialize<GptResponse>(responseContent);

        Console.WriteLine();
        Console.WriteLine(gptResponse);

        await JsonHelper.SaveToJson(gptResponse, ResponsesFilePath);
    }

    private static async Task<string> TestPromptClass(GptClient gptClient, OpenAiOptions openAiOptions)
    {
        var messages = new List<PromptMessage>
        {
            new PromptMessage
            {
                Role = PromptRoles.User,
                Content = "What is the weather like in Boston?"
            }
        };

        var functions = new List<PromptFunction>
        {
            new PromptFunction
            {
                Name = "get_current_weather",
                Description = "Get the current weather in a given location",
                Parameters = new PromptParameters
                {
                    Properties = await JsonHelper.GetObjectFromJson<object>(FunctionsPropertiesToTestFilePath),
                    Required = new List<string> { "location" }
                }
            }
        };

        PromptFunctionCall? functionCall = new PromptFunctionCall {  Name = functions[0].Name };

        var promptToTest = new Prompt
        {
            Model = openAiOptions.ApiModel,
            Messages = messages,
            Functions = functions,
            FunctionCall = functionCall
        };

        Console.WriteLine("Found prompt. Sending to GPT");

        if (UseTestResponse)
            return GetTestResponse();

        var result = await gptClient.GetCompletion(promptToTest);

        return result;
    }

    private static async Task<string> TestJsonPrompt(GptClient gptClient)
    {
        var promptToTest = await File.ReadAllTextAsync(PromptToTestFilePath);
        if (string.IsNullOrWhiteSpace(promptToTest))
            throw new Exception("Failed to find a valid prompt.");

        promptToTest = promptToTest.ReplaceLineEndings("");

        Console.WriteLine("Found prompt. Sending to GPT");

        if (UseTestResponse)
            return GetTestResponse();

        var result = await gptClient.GetCompletion(promptToTest);
        return result;
    }

    private static string GetTestResponse()
    {
        Console.WriteLine("Using test response.");

        return File.ReadAllText(TestResponseFilePath);
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

        return new OpenAiOptions
        {
            ApiKey = apikey
        };
    }
}