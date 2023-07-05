﻿using GptFactCheckerApi.Repository.JsonRepo;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Shared.GptClient;

public class GptClient : IGptClient
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _openAiOptions;

    public GptClient(IHttpClientFactory clientFactory, IOptions<OpenAiOptions> openAiOptions)
    {
        _httpClient = clientFactory.CreateClient();
        _openAiOptions = openAiOptions.Value;

        if (string.IsNullOrWhiteSpace(_openAiOptions.ApiKey))
            throw new ArgumentNullException(nameof(_openAiOptions.ApiKey));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _openAiOptions.ApiKey);
    }

    /// <summary>
    /// Serializes a Prompt-object and sends it to GPT
    /// </summary>
    public async Task<string> GetCompletion(Prompt prompt, double temperature = 0)
    {
        var promptString = JsonHelper.Serialize(prompt, false);

        return await GetCompletion(promptString, temperature);
    }

    /// <summary>
    /// Sends a string containing a JSON formatted prompt to GPT
    /// </summary>
    public async Task<string> GetCompletion(string prompt, double temperature = 0)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_openAiOptions.CompletionsUrl),
            Content = new StringContent(prompt, Encoding.UTF8, "application/json")
        };

        return await SendPostRequest(request);
    }

    private async Task<string> SendPostRequest(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Request failed with status code {response.StatusCode}");
            return string.Empty;
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Request success");
        Console.WriteLine(responseContent);

        return responseContent;
    }
}