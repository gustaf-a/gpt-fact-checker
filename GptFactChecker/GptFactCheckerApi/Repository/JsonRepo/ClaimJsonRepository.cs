﻿using Shared.Models;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class ClaimJsonRepository : IClaimRepository
{
    public const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Claims.json";

    public async Task<bool> CreateClaims(List<Fact> claimsToCreate)
    {
        var claims = await JsonHelper.GetObjectFromJson<List<Fact>>(JsonFilePath);

        foreach (var claimToCreate in claimsToCreate)
            if (!claims.Any(c => c.Id == claimToCreate.Id))
                claims.Add(claimToCreate);

        await JsonHelper.SaveToJson(claims, JsonFilePath);

        return true;
    }

    public async Task<List<Fact>> GetClaims(List<string>? claimIds = null)
    {
        return await GetClaimsById(claimIds);
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

    private static async Task<List<Fact>> GetClaimsById(List<string>? claimIds)
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
}
