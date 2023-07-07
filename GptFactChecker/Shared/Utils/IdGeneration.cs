namespace Shared.Utils;

public static class IdGeneration
{
    private static readonly Random _random = new();

    private const int IdGenerationLowerBound = 1000000;
    private const int IdGenerationUpperBound = 9999999;

    public static int GenerateIntegerId()
    {
        var randomNumber = _random.Next(IdGenerationLowerBound, IdGenerationUpperBound);

        return randomNumber;
    }

    public static string GenerateStringId()
    {
        return GenerateIntegerId().ToString();
    }
}
