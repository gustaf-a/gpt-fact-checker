namespace Shared.GptClient;

public interface IGptResponseParser
{
    /// <summary>
    /// Parses content response from GPT into a type of T
    /// </summary>
    /// <param name="gptResponseString">Content response string from GptClient call</param>
    /// <param name="callerName">Name of the caller, used for logging.</param>
    public T ParseGptResponse<T>(string gptResponseString, string callerName);

    /// <summary>
    /// Parses function call response from GPT into a type of T. 
    /// Returns null if not function call is used in the GPT response.
    /// </summary>
    /// <param name="gptResponseString">Content response string from GptClient call</param>
    /// <param name="callerName">Name of the caller, used for logging.</param>
    public T ParseGptResponseFunctionCall<T>(string gptResponseString, string callerName);
}
