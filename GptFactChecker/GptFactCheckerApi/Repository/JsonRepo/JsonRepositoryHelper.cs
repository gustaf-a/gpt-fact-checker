using Newtonsoft.Json;

namespace GptFactCheckerApi.Repository.JsonRepo;

internal static class JsonRepositoryHelper
{
    internal static async Task<T> GetObjectFromJson<T>(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        try
        {
            string fileAsString = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(fileAsString))
                return default;

            var obj = JsonConvert.DeserializeObject<T>(fileAsString);

            return obj ?? default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return default;
        }
    }

    internal static async Task SaveToJson(object objectToSave, string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        var claimClaimChecksJson = JsonConvert.SerializeObject(objectToSave);

        await File.WriteAllTextAsync(filePath, claimClaimChecksJson);
    }
}
