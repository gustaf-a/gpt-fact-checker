using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class ClaimJsonRepository : IClaimRepository
{
    public const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Claims.json";

    public async Task<bool> CreateClaims(List<Claim> claimsToCreate)
    {
        var claims = await JsonRepositoryHelper.GetObjectFromJson<List<Claim>>(JsonFilePath);

        foreach (var claimToCreate in claimsToCreate)
            if (!claims.Any(c => c.Id == claimToCreate.Id))
                claims.Add(claimToCreate);

        await JsonRepositoryHelper.SaveToJson(claims, JsonFilePath);

        return true;
    }

    public async Task<List<Claim>> GetClaims(List<string>? claimIds = null)
    {
        return await GetClaimsById(claimIds);
    }

    public async Task<bool> RemoveClaims(List<string> claimIds)
    {
        if (claimIds is null || !claimIds.Any())
            return true;

        var claims = await JsonRepositoryHelper.GetObjectFromJson<List<Claim>>(JsonFilePath);

        var claimsToKeep = claims.Where(c => !claimIds.Contains(c.Id)).ToList();

        await JsonRepositoryHelper.SaveToJson(claimsToKeep, JsonFilePath);

        return true;
    }

    private static async Task<List<Claim>> GetClaimsById(List<string>? claimIds)
    {
        if (!claimIds.Any())
            return new();

        var claims = await JsonRepositoryHelper.GetObjectFromJson<List<Claim>>(JsonFilePath);

        if (claimIds != null && claimIds.Any())
        {
            claims = claims.Where(c => claimIds.Contains(c.Id)).ToList();
        }

        return claims;
    }
}
