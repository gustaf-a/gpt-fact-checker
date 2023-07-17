using JsonClient;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimJsonRepository : IClaimRepository
{
    private const string FileName = "Claims.json";

    private readonly string JsonFilePath;

    public ClaimJsonRepository(IOptions<RepositoryJsonOptions> options)
    {
        JsonFilePath = options.Value.DataFolder + FileName;
    }

    public async Task<bool> CreateClaims(List<Fact> claimsToCreate)
    {
        var claims = await JsonHelper.GetObjectFromJson<List<Fact>>(JsonFilePath);

        foreach (var claimToCreate in claimsToCreate)
            if (!claims.Any(c => c.Id == claimToCreate.Id))
                claims.Add(claimToCreate);

        await JsonHelper.SaveToJson(claims, JsonFilePath);

        return true;
    }

    public async Task<List<Fact>> GetAllClaims()
    {
        var claims = await JsonHelper.GetObjectFromJson<List<Fact>>(JsonFilePath);

        if (claims.IsNullOrEmpty())
            return new();

        return claims;
    }

    public async Task<List<Fact>> GetClaims(List<string> claimIds)
    {
        if (!claimIds.Any())
            return new();

        var claims = await JsonHelper.GetObjectFromJson<List<Fact>>(JsonFilePath);

        if (claimIds != null && claimIds.Any())
        {
            claims = claims.Where(c => claimIds.Contains(c.Id)).ToList();
        }

        return claims;
    }

    public async Task<bool> RemoveClaims(List<string> claimIds)
    {
        if (claimIds is null || !claimIds.Any())
            return true;

        var claims = await JsonHelper.GetObjectFromJson<List<Fact>>(JsonFilePath);

        var claimsToKeep = claims.Where(c => !claimIds.Contains(c.Id)).ToList();

        await JsonHelper.SaveToJson(claimsToKeep, JsonFilePath);

        return true;
    }
}
