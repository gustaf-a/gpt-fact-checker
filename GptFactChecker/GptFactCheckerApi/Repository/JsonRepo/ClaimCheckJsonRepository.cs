using GptFactCheckerApi.Model;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class ClaimCheckJsonRepository : IClaimCheckRepository
{
    public const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\ClaimChecks.json";

    public async Task<bool> CreateClaimChecks(List<ClaimCheck> claimChecksToCreate)
    {
        var claimChecks = await JsonRepositoryHelper.GetObjectFromJson<List<ClaimCheck>>(JsonFilePath);

        foreach (var claimCheckToCreate in claimChecksToCreate)
            if (!claimChecks.Any(cc => cc.Id == claimCheckToCreate.Id))
                claimChecks.Add(claimCheckToCreate);

        await JsonRepositoryHelper.SaveToJson(claimChecks, JsonFilePath);

        return true;
    }

    public async Task<List<ClaimCheck>> GetClaimChecks(List<string>? claimCheckIds, bool includeClaimCheckReactions = false)
    {
        if (claimCheckIds == null || !claimCheckIds.Any())
            return new List<ClaimCheck>();

        var claimChecks = await JsonRepositoryHelper.GetObjectFromJson<List<ClaimCheck>>(JsonFilePath);

        if (claimChecks is not null && claimChecks.Any())
            claimChecks = claimChecks.Where(cc => claimCheckIds.Contains(cc.Id)).ToList();

        return claimChecks ?? new List<ClaimCheck>();
    }

    public async Task<bool> DeleteClaimChecks(List<string> claimCheckIds)
    {
        if (claimCheckIds is null || !claimCheckIds.Any())
            return true;

        var claimChecks = await JsonRepositoryHelper.GetObjectFromJson<List<ClaimCheck>>(JsonFilePath);

        var claimChecksToKeep = claimChecks.Where(cc => !claimCheckIds.Contains(cc.Id)).ToList();

        await JsonRepositoryHelper.SaveToJson(claimChecksToKeep, JsonFilePath);

        return true;
    }
}
