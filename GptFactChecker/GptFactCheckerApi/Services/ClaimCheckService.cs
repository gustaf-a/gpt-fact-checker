using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;

namespace GptFactCheckerApi.Services;

public class ClaimCheckService : IClaimCheckService
{
    private readonly IClaimCheckRepository _claimCheckRepository;
    private readonly IClaimsClaimChecksRepository _claimsClaimChecksRepository;

    private readonly IClaimCheckReactionService _claimCheckReactionService;

    public ClaimCheckService(IClaimCheckRepository claimCheckRepository, IClaimsClaimChecksRepository claimsClaimChecksRepository, IClaimCheckReactionService claimCheckReactionService)
    {
        _claimCheckRepository = claimCheckRepository;
        _claimsClaimChecksRepository = claimsClaimChecksRepository;
        _claimCheckReactionService = claimCheckReactionService;
    }

    public async Task<bool> AddClaimChecks(List<ClaimCheckDto> claimCheckDtos, string claimId)
    {
        var claimChecks = claimCheckDtos.ToClaimChecks();

        if (!await _claimCheckRepository.CreateClaimChecks(claimChecks))
            return false;

        await _claimsClaimChecksRepository.AddClaimChecksForClaim(claimId, claimChecks);

        return true;
    }

    public async Task<List<ClaimCheckDto>> GetAllClaimChecks(bool includeClaimCheckReactions = false)
    {
        List<ClaimCheckDto> claimCheckDtos
            = (await _claimCheckRepository.GetClaimChecks(new List<string>(), includeClaimCheckReactions)).ToDtos();

        if (includeClaimCheckReactions)
            foreach (var claimCheckDto in claimCheckDtos)
                claimCheckDto.ClaimCheckReactions = await _claimCheckReactionService.GetClaimCheckReactions(claimCheckDto.Id);

        return claimCheckDtos;
    }

    public async Task<List<ClaimCheckDto>> GetClaimChecks(string claimId, bool includeClaimCheckReactions = false)
    {
        var claimCheckIds = await _claimsClaimChecksRepository.GetClaimChecksForClaim(claimId);

        var claimCheckDtos = (await _claimCheckRepository.GetClaimChecks(claimCheckIds)).ToDtos();

        if (includeClaimCheckReactions)
        {
            foreach (var claimCheckDto in claimCheckDtos)
                claimCheckDto.ClaimCheckReactions = await _claimCheckReactionService.GetClaimCheckReactions(claimCheckDto.Id);

            //TODO sort claimCheckDtos based on sum of their ClaimCheckReactions. Secondary sorting is newest first, that is by the ISOstring DateCreated
        }

        return claimCheckDtos;
    }

    public async Task<bool> DeleteClaimChecks(List<string> claimCheckIds)
    {
        return await _claimCheckRepository.DeleteClaimChecks(claimCheckIds);
    }
}
