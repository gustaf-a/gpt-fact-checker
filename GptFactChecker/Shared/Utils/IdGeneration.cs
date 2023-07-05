namespace Shared.Utils;

public static class IdGeneration
{
    private static readonly Random _random = new();

    private const int IdGenerationLowerBound = 100000;
    private const int IdGenerationUpperBound = 999999;

    public static int GenerateIntegerId()
    {
        var randomNumber = _random.Next(IdGenerationLowerBound, IdGenerationUpperBound);

        return randomNumber;
    }
}
