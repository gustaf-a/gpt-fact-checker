namespace FactCheckingServiceTests.FactCheckers.ClimateStrategy.TopicIdentification;

public class TopicIdentifierTests
{
    private const string _topicResponse =
"""
{
  "id": "chatcmpl-7YAVcDWyYjNbj5Kz6H8HjKmeiPfF0",
  "object": "chat.completion",
  "created": 1688377904,
  "model": "gpt-3.5-turbo-0613",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "{\"claims_with_references\": [\n  {\n    \"claim_id\": \"1\",\n    \"reference_ids\": [\"419924\"]\n  },\n  {\n    \"claim_id\": \"3\",\n    \"reference_ids\": [\"654985\"]\n  }\n]}",
        "function_call": {
          "name": "fact_check_claims",
          "arguments": "{\n  \"claims_with_references\": [\n    {\n      \"claim_id\": \"1\",\n      \"reference_ids\": [\"419924\"]\n    },\n    {\n      \"claim_id\": \"3\",\n      \"reference_ids\": [\"654985\"]\n    }\n  ]\n}"
        }
      },
      "finish_reason": "function_call"
    }
  ],
  "usage": {
    "prompt_tokens": 210,
    "completion_tokens": 112,
    "total_tokens": 322
  }
}

""";
}
