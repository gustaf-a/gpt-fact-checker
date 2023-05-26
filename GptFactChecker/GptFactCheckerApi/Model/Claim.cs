using System.Security.Cryptography.X509Certificates;

namespace GptFactCheckerApi.Model;

public class Claim
{
    public string Id { get; set; }
    public string ClaimSummarized { get; set; }
    public string ClaimRawText { get; set; }
    public string[] Tags { get; set; }
}
