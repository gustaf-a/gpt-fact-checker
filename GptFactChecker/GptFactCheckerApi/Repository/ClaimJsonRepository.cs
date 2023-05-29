using GptFactCheckerApi.Model;
using Newtonsoft.Json;

namespace GptFactCheckerApi.Repository;

public class ClaimJsonRepository : IClaimRepository
{
    public const string ClaimsJsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Claims.json";

    public async Task<bool> CreateClaims(List<Claim> claimsToCreate)
    {
        var claims = await GetAllClaims();

        foreach (var claimToCreate in claimsToCreate)
            if (!claims.Any(c => c.Id == claimToCreate.Id))
                claims.Add(claimToCreate);

        await SaveClaims(claims);

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

        var claims = await GetAllClaims();

        var claimsToKeep = claims.Where(c => !claimIds.Contains(c.Id)).ToList();

        await SaveClaims(claimsToKeep);

        return true;
    }

    private static async Task<List<Claim>> GetClaimsById(List<string>? claimIds)
    {
        if (!claimIds.Any())
            return new();

        var claims = await GetAllClaims();

        if(claimIds != null && claimIds.Any())
        {
            claims = claims.Where(c => claimIds.Contains(c.Id)).ToList();
        }

        return claims;
    }

    private static async Task<List<Claim>> GetAllClaims()
    {
        var claimsJson = await File.ReadAllTextAsync(ClaimsJsonFilePath);

        return JsonConvert.DeserializeObject<List<Claim>>(claimsJson);
    }

    private static async Task SaveClaims(List<Claim> claims)
    {
        var claimsJson = JsonConvert.SerializeObject(claims);

        await File.WriteAllTextAsync(ClaimsJsonFilePath, claimsJson);
    }
}
