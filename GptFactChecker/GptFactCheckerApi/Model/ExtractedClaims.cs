namespace GptFactCheckerApi.Model;

/// <summary>
/// Return object from GTP fact extraction
/// </summary>
public class ExtractedClaims
{
    public Claim[] Claims;
    public string Description;
}
