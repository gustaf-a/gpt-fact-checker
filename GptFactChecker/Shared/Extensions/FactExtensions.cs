using Shared.Models;

namespace Shared.Extensions;

public static class FactExtensions
{
    public static List<PromptClaim> ToPromptClaims(this List<Fact> facts)
    {
        return facts.Select(f => f.ToPromptClaim()).ToList();
    }

    public static PromptClaim ToPromptClaim(this Fact fact)
    {
        if (string.IsNullOrWhiteSpace(fact.Id))
            throw new ArgumentNullException(nameof(fact.Id));

        var claimText = GetClaimText(fact);
        if (string.IsNullOrWhiteSpace(claimText))
            throw new ArgumentNullException(nameof(claimText));

        return new PromptClaim
        {
            Id = fact.Id,
            Claim = claimText
        };
    }

    private static string GetClaimText(Fact fact)
    {
        if (!string.IsNullOrWhiteSpace(fact.ClaimSummarized))
            return fact.ClaimSummarized;

        if (!string.IsNullOrWhiteSpace(fact.ClaimRawText))
            return fact.ClaimRawText;

        return string.Empty;
    }
}
