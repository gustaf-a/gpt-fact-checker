using GptHandler.GptClient;
using Newtonsoft.Json;

namespace GptHandlerTests.GptClient;

public class GptResponseParserTests
{
    private const string TestResponse_FunctionCall_Stop =
"""
{
  "id": "chatcmpl-7ZJBMFEzDNQVrlNSkJIaldspMsBUL",
  "object": "chat.completion",
  "created": 1688649572,
  "model": "gpt-3.5-turbo-0613",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": null,
        "function_call": {
          "name": "fact_check_claims",
          "arguments": "{\n  \"test_arguments\": [\n    {\n      \"claim_id\": \"test-id\",\n      \"reference_ids\": [\n        \"343291\"\n      ]\n    }\n  ]\n}"
        }
      },
      "finish_reason": "stop"
    }
  ],
  "usage": {
    "prompt_tokens": 2221,
    "completion_tokens": 37,
    "total_tokens": 2258
  }
}
""";

    private const string TestResponse_FunctionCall_FunctionCall =
"""
{
  "id": "chatcmpl-7ZJBMFEzDNQVrlNSkJIaldspMsBUL",
  "object": "chat.completion",
  "created": 1688649572,
  "model": "gpt-3.5-turbo-0613",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": null,
        "function_call": {
          "name": "fact_check_claims",
          "arguments": "{\n  \"test_arguments\": [\n    {\n      \"claim_id\": \"test-id\",\n      \"reference_ids\": [\n        \"343291\"\n      ]\n    }\n  ]\n}"
        }
      },
      "finish_reason": "function_call"
    }
  ],
  "usage": {
    "prompt_tokens": 2221,
    "completion_tokens": 37,
    "total_tokens": 2258
  }
}
""";

    private IGptResponseParser _gptResponseParser;

    public GptResponseParserTests()
    {
        _gptResponseParser = new GptResponseParser();
    }

    [Fact]
    public void ParseGptResponseFunctionCall_ShouldReturnDefaultWhenGptResponseStringIsInvalid()
    {
        var result = _gptResponseParser.ParseGptResponseFunctionCall<TestFunctionCallArgs>("bad test string", "caller");

        Assert.Null(result);
    }

    [Fact]
    public void ParseGptResponseFunctionCall_ShouldReturnDeserializedTypeWhenAllConditionsMet_FinishReasonIsStop()
    {
        var result = _gptResponseParser.ParseGptResponseFunctionCall<TestFunctionCallArgs>(TestResponse_FunctionCall_Stop, "caller");

        Assert.NotNull(result);

        //Assert.Equal("test-id", result.TestArguments.First().ClaimId);
    }

    [Fact]
    public void ParseGptResponseFunctionCall_ShouldReturnDeserializedTypeWhenAllConditionsMet_FinishReasonIsFunctionCall()
    {
        var result = _gptResponseParser.ParseGptResponseFunctionCall<TestFunctionCallArgs>(TestResponse_FunctionCall_FunctionCall, "caller");

        Assert.NotNull(result);

        //Assert.Equal("test-id", result.TestArguments.First().ClaimId);
    }

    //[Fact]
    //public void ParseGptResponse_ShouldReturnDefaultWhenGptResponseStringIsNull()
    //{
    //    var result = _gptResponseParser.ParseGptResponse<Content>(null, "caller");
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void ParseGptResponse_ShouldReturnDefaultWhenContentIsNull()
    //{
    //    var result = _gptResponseParser.ParseGptResponse<Content>(TestResponse_FunctionCall_FunctionCall, "caller");
    //    Assert.Null(result);
    //}

    [Fact]
    public void ParseGptResponse_ShouldReturnDeserializedContentWhenAllConditionsMet()
    {
        // Test with appropriate TestResponse_Content
    }
}

internal class TestFunctionCallArgs
{
    [JsonProperty(PropertyName = "test_arguments")]
    public List<TestArguments> TestArguments { get; set; }
}

internal class TestArguments
{
    [JsonProperty(PropertyName = "claim_id")]
    public string ClaimId { get; set; }

    [JsonProperty(PropertyName = "reference_ids")]
    public List<string> ReferenceIds { get; set; }
}
