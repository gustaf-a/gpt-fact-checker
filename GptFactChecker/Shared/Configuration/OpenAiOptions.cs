namespace Shared.Configuration;

public class OpenAiOptions
{
    public const string OpenAi = "OpenAi";

    public string ApiKey { get; set; } = string.Empty;
    public string ApiModelBase { get; set; } = "gpt-3.5-turbo";
    public string ApiModelExpensive { get; set; } = "gpt-4";
    public int MaxTokens { get; set; } = 4000;

    public bool LogPrompts { get; set; } = true;
    public bool LogResponses { get; set; } = true;

    public string CompletionsUrl { get; set; } = @"https://api.openai.com/v1/chat/completions";

    public string WhisperApiKey { get; set; } = string.Empty;
    public string WhisperApiModel { get; set; } = "whisper-1";
    public string WhisperApiEndPoint { get; set; } = "https://api.openai.com/v1/audio/transcriptions";
    public List<string> WhisperSupportedLanguages { get; set; } = new List<string>
    {
        "Afrikaans", "Arabic", "Armenian", "Azerbaijani", "Belarusian", "Bosnian", "Bulgarian", "Catalan",
        "Chinese", "Croatian", "Czech", "Danish", "Dutch", "English", "Estonian", "Finnish", "French",
        "Galician", "German", "Greek", "Hebrew", "Hindi", "Hungarian", "Icelandic", "Indonesian", "Italian",
        "Japanese", "Kannada", "Kazakh", "Korean", "Latvian", "Lithuanian", "Macedonian", "Malay", "Marathi",
        "Maori", "Nepali", "Norwegian", "Persian", "Polish", "Portuguese", "Romanian", "Russian", "Serbian",
        "Slovak", "Slovenian", "Spanish", "Swahili", "Swedish", "Tagalog", "Tamil", "Thai", "Turkish",
        "Ukrainian", "Urdu", "Vietnamese", "Welsh"
    };
}
