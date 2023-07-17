using GptFactCheckerApi.Model;
using GptFactCheckerApi.Repository;
using GptFactCheckerApi.Repository.JsonRepo;
using Shared.Extensions;

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

    public async Task<BackendResponse<bool>> AddClaimCheckResults(List<ClaimCheckResultsDto> claimCheckResultsDtos)
    {
        var response = new BackendResponse<bool>();

        if (claimCheckResultsDtos.IsNullOrEmpty())
        {
            response.Messages.Add("List of ClaimCheckResults received was null or empty.");
            return response;
        }

        response.IsSuccess = true;

        foreach (var claimCheckResultDto in claimCheckResultsDtos)
        {
            var claimId = claimCheckResultDto.Claim?.Id;

            var claimCheckDto = claimCheckResultDto.ClaimCheck;
            if (claimCheckDto is null || string.IsNullOrWhiteSpace(claimId))
            {
                response.Messages.Add($"Invalid ClaimCheckResul received: {JsonHelper.Serialize(claimCheckResultDto)}");
                response.IsSuccess = false;
            }

            var partialResult = await AddClaimChecks(new List<ClaimCheckDto>() { claimCheckDto }, claimCheckResultDto.Claim.Id);

            if (!partialResult)
            {
                response.Messages.Add($"Failed to add ClaimCheckResult: {JsonHelper.Serialize(claimCheckResultDto)}");
                response.IsSuccess = false;
            }
        }

        return response;
    }

    public async Task<bool> AddClaimChecks(List<ClaimCheckDto> claimCheckDtos, string claimId)
    {
        var claimChecks = claimCheckDtos.ToClaimChecks();

        try
        {
            if (!await _claimCheckRepository.CreateClaimChecks(claimChecks))
                return false;

            await _claimsClaimChecksRepository.AddClaimChecksForClaim(claimId, claimChecks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save claim checks to claim with ID: {claimId}. ClaimChecks: {JsonHelper.Serialize(claimCheckDtos)}", ex);
            return false;
        }

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

            claimCheckDtos = claimCheckDtos
                        .OrderByDescending(dto => dto.ClaimCheckReactions.IsNullOrEmpty() ? 0 : dto.ClaimCheckReactions.Sum(reaction => reaction.Reaction))
                        .ThenByDescending(dto => dto.DateCreated)
                        .ToList();
        }

        return claimCheckDtos;
    }

    public async Task<BackendResponse<bool>> DeleteClaimChecks(List<string> claimCheckIds)
    {
        var response = new BackendResponse<bool>();

        foreach (var claimCheckId in claimCheckIds)
            await DeleteClaimCheck(claimCheckId, response);

        return response;
    }

    private async Task DeleteClaimCheck(string claimCheckId, BackendResponse<bool> response)
    {
        if (string.IsNullOrWhiteSpace(claimCheckId))
        {
            response.Messages.Add("Invalid ClaimCheckId received.");
            return;
        }

        if (!await _claimCheckReactionService.DeleteClaimCheckReactionsByClaimCheck(claimCheckId))
            response.Messages.Add($"Failed to delete claim check reactions for claim check: {claimCheckId}");

        if (!await _claimsClaimChecksRepository.RemoveClaimChecks(new List<string>() { claimCheckId }))
            response.Messages.Add($"Failed to remove claims claimcheck relationships for ClaimCheck ID: {claimCheckId}");

        if (!await _claimCheckRepository.DeleteClaimChecks(new List<string>() { claimCheckId }))
            response.Messages.Add($"Failed to delete claim check: {claimCheckId}");

        return;
    }
}
