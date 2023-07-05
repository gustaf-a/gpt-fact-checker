namespace Shared.Configuration;

public class OpenAiOptions
{
    public const string OpenAi = "OpenAi";

    public string ApiKey { get; set; } = string.Empty;
    public string ApiModel { get; set; } = "gpt-3.5-turbo";
    public int MaxTokens { get; set; } = 4000;

    public string CompletionsUrl { get; set; } = @"https://api.openai.com/v1/chat/completions";
}
