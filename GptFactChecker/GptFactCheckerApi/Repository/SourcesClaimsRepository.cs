using GptFactCheckerApi.Model;
using Newtonsoft.Json;
using System.Security.Claims;
using Claim = GptFactCheckerApi.Model.Claim;

namespace GptFactCheckerApi.Repository;

public class SourcesClaimsRepository : ISourcesClaimsRepository
{
    private const string SourceClaimsJsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Sources_Claims.json";

    public async Task<bool> AddClaimsForSource(string sourceId, List<Claim> claims)
    {
        var sourceClaims = await GetAllSourceClaims();

        var existingSourceClaims = sourceClaims.Find(sc => sourceId.Equals(sc.SourceId));

        if (existingSourceClaims is null)
        {
            existingSourceClaims = new SourceClaims
            {
                SourceId = sourceId,
                ClaimIds = new()
            };

            sourceClaims.Add(existingSourceClaims);
        }

        foreach (var claim in claims)
            if (!existingSourceClaims.ClaimIds.Contains(claim.Id))
                existingSourceClaims.ClaimIds.Add(claim.Id);

        await SaveSourceClaims(sourceClaims);

        return true;
    }


    public async Task<List<string>> GetClaimsForSource(string sourceId)
    {
        var sourceClaims = await GetAllSourceClaims();

        var source = sourceClaims.Find(sc => sc.SourceId.Equals(sourceId));

        return source?.ClaimIds ?? new List<string>();
    }

    public async Task<bool> RemoveClaimsForSource(string sourceId, List<string> claimIds)
    {
        var sourceClaims = await GetAllSourceClaims();

        var source = sourceClaims.Find(sc => sc.SourceId.Equals(sourceId));
        if (source is null)
            return false;

        source.ClaimIds.RemoveAll(claimIds.Contains);
        
        await SaveSourceClaims(sourceClaims);
        
        return true;
    }

    public async Task<bool> RemoveSource(string sourceId)
    {
        var sourceClaims = await GetAllSourceClaims();

        var source = sourceClaims.Find(sc => sc.SourceId.Equals(sourceId));
        if (source is null)
            return true;
        
        sourceClaims.Remove(source);
        
        await SaveSourceClaims(sourceClaims);
        
        return true;
    }

    private static async Task<List<SourceClaims>> GetAllSourceClaims()
    {
        var sourceClaimsJson = await File.ReadAllTextAsync(SourceClaimsJsonFilePath);

        if (string.IsNullOrWhiteSpace(sourceClaimsJson))
            return new List<SourceClaims>();

        return JsonConvert.DeserializeObject<List<SourceClaims>>(sourceClaimsJson);
    }

    private static async Task SaveSourceClaims(List<SourceClaims> sourceClaims)
    {
        var sourceClaimsJson = JsonConvert.SerializeObject(sourceClaims);

        await File.WriteAllTextAsync(SourceClaimsJsonFilePath, sourceClaimsJson);
    }
}
