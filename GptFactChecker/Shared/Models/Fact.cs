namespace Shared.Models;

public class Fact
{
    public string Id { get; set; }
    public string ClaimSummarized { get; set; }
    public string ClaimRawText { get; set; }
    public string[] Tags { get; set; }
}
