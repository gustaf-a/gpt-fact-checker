using JsonClient;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimCheckJsonRepository : IClaimCheckRepository
{
    private const string FileName = "ClaimChecks.json";

    private readonly string JsonFilePath;

    public ClaimCheckJsonRepository(IOptions<RepositoryJsonOptions> options)
    {
        JsonFilePath = options.Value.DataFolder + FileName;
    }

    public async Task<bool> CreateClaimChecks(List<ClaimCheck> claimChecksToCreate)
    {
        var claimChecks = await JsonHelper.GetObjectFromJson<List<ClaimCheck>>(JsonFilePath);

        foreach (var claimCheckToCreate in claimChecksToCreate)
            if (!claimChecks.Any(cc => cc.Id == claimCheckToCreate.Id))
                claimChecks.Add(claimCheckToCreate);

        await JsonHelper.SaveToJson(claimChecks, JsonFilePath);

        return true;
    }

    public async Task<List<ClaimCheck>> GetClaimChecks(List<string>? claimCheckIds, bool includeClaimCheckReactions = false)
    {
        if (claimCheckIds == null || !claimCheckIds.Any())
            return new List<ClaimCheck>();

        var claimChecks = await JsonHelper.GetObjectFromJson<List<ClaimCheck>>(JsonFilePath);

        if (claimChecks is not null && claimChecks.Any())
            claimChecks = claimChecks.Where(cc => claimCheckIds.Contains(cc.Id)).ToList();

        return claimChecks ?? new List<ClaimCheck>();
    }

    public async Task<bool> DeleteClaimChecks(List<string> claimCheckIds)
    {
        if (claimCheckIds is null || !claimCheckIds.Any())
            return true;

        var claimChecks = await JsonHelper.GetObjectFromJson<List<ClaimCheck>>(JsonFilePath);

        var claimChecksToKeep = claimChecks.Where(cc => !claimCheckIds.Contains(cc.Id)).ToList();

        await JsonHelper.SaveToJson(claimChecksToKeep, JsonFilePath);

        return true;
    }
}
