using GptFactCheckerApiController.Model;
using Shared.Extensions;
using Shared.Models;
using SourceCollectingService.Models;

namespace GptFactCheckerApi.Model;

public static class Extensions
{
    public static Source ToSource(this SourceDto sourceDto)
    {
        return new Source
        {
            Id = sourceDto.Id,
            Name = sourceDto.Name,
            Language = sourceDto.Language,
            Description = sourceDto.Description,
            SourceUrl = sourceDto.SourceUrl,

            SourceType = sourceDto.SourceType,
            SourcePerson = sourceDto.SourcePerson,
            SourceContext = sourceDto.SourceContext,
            SourceCreatedDate = sourceDto.SourceCreatedDate,
            SourceImportedDate = sourceDto.SourceImportedDate,

            ClaimsFirstExtractedDate = sourceDto.ClaimsFirstExtractedDate,
            ClaimsUpdatedDate = sourceDto.ClaimsUpdatedDate,
            CoverImageUrl = sourceDto.CoverImageUrl,

            Tags = sourceDto.Tags,

            RawText = sourceDto.SourceRawText,
        };
    }

    public static List<SourceDto> ToDtos(this List<Source> sources)
    {
        return sources.Select(s => s.ToDto()).ToList();
    }

    public static SourceDto ToDto(this Source source)
    {
        if (source is null)
            return null;

        return new SourceDto
        {
            Id = source.Id,
            Name = source.Name,
            Language = source.Language,
            Description = source.Description,
            SourceUrl = source.SourceUrl,

            SourceType = source.SourceType,
            SourcePerson = source.SourcePerson,
            SourceContext = source.SourceContext,
            SourceCreatedDate = source.SourceCreatedDate,
            SourceImportedDate = source.SourceImportedDate,

            ClaimsFirstExtractedDate = source.ClaimsFirstExtractedDate,
            ClaimsUpdatedDate = source.ClaimsUpdatedDate,
            CoverImageUrl = source.CoverImageUrl,

            Tags = source.Tags,

            SourceRawText = source.RawText,
        };
    }

    public static List<Fact> ToClaims(this IEnumerable<ClaimDto> claimDtos)
    {
        List<Fact> claims = new();

        if (claimDtos is null)
            return claims;

        foreach (ClaimDto claimDto in claimDtos)
        {
            var claim = claimDto.ToClaim();

            if(claim is null) 
                continue;

            claims.Add(claim);
        }

        return claims;
    }

    public static Fact ToClaim(this ClaimDto claimDto)
    {
        if (claimDto is null)
            return null;

        return new()
        {
            Id = claimDto.Id,
            ClaimSummarized = claimDto.ClaimSummarized,
            ClaimRawText = claimDto.ClaimRawText,
            Tags = claimDto.Tags
        };
    }

    public static List<ClaimDto> ToDtos(this List<Fact> facts)
    {
        List<ClaimDto> claimDtos = new();

        foreach (Fact fact in facts)
        {
            ClaimDto claimDto = fact.ToDto();

            if (claimDto is not null)
                claimDtos.Add(claimDto);
        }

        return claimDtos;
    }

    public static ClaimDto ToDto(this Fact fact)
    {
        if (fact is null)
            return null;

        return new()
        {
            Id = fact.Id,
            ClaimSummarized = fact.ClaimSummarized,
            ClaimRawText = fact.ClaimRawText,
            Tags = fact.Tags
        };
    }

    public static List<ClaimCheck> ToClaimChecks(this IEnumerable<ClaimCheckDto> claimCheckDtos)
    {
        List<ClaimCheck> claimChecks = new();

        if (claimCheckDtos is null)
            return claimChecks;

        foreach (ClaimCheckDto claimCheckDto in claimCheckDtos)
        {
            ClaimCheck claimCheck = new()
            {
                Id = claimCheckDto.Id,
                UserId = claimCheckDto.UserId,
                Label = claimCheckDto.Label,
                ClaimCheckText = claimCheckDto.ClaimCheckText,
                DateCreated = claimCheckDto.DateCreated,
                References = claimCheckDto.References ?? new()
            };

            claimChecks.Add(claimCheck);
        }

        return claimChecks;
    }

    public static List<ClaimCheckDto> ToDtos(this IEnumerable<ClaimCheck> claimChecks)
    {
        List<ClaimCheckDto> claimCheckDtos = new();

        foreach (ClaimCheck claimCheck in claimChecks)
        {
            ClaimCheckDto claimCheckDto = new()
            {
                Id = claimCheck.Id,
                UserId = claimCheck.UserId,
                Label = claimCheck.Label,
                ClaimCheckText = claimCheck.ClaimCheckText,
                DateCreated = claimCheck.DateCreated,
                References = claimCheck.References ?? new()
            };

            claimCheckDtos.Add(claimCheckDto);
        }

        return claimCheckDtos;
    }

    public static List<ClaimCheckResultsDto> ToDtos(this IEnumerable<FactCheckResult> factCheckResults)
    {
        List<ClaimCheckResultsDto> claimCheckResultDtos = new();

        if (factCheckResults is null)
            return claimCheckResultDtos;

        foreach (FactCheckResult factCheckResult in factCheckResults)
        {
            var userId = factCheckResult.Author?.Id ?? string.Empty;

            ClaimCheckResultsDto claimCheckResultsDto = new()
            {
                Claim = factCheckResult.Fact.ToDto(),
                ClaimCheck = factCheckResult.FactCheck.ToClaimCheckDto(userId),
                IsFactChecked = factCheckResult.IsFactChecked,
                IsStored = factCheckResult.IsStored,
                AuthorUserId = userId,
                Messages = factCheckResult.Messages
            };

            claimCheckResultDtos.Add(claimCheckResultsDto);
        }

        return claimCheckResultDtos;
    }

    public static ClaimCheckDto ToClaimCheckDto(this FactCheck factCheck, string userId = "")
    {
        if (factCheck is null)
            return null;

        return new ClaimCheckDto
        {
            Id = factCheck.Id,
            UserId = userId,
            Label = factCheck.Label,
            ClaimCheckText = factCheck.FactCheckText,
            DateCreated = factCheck.DateCreated.ToIsoString(),
            References = factCheck.References ?? new()
        };
    }

    public static SourceCollectingResponseDto ToDto(this SourceCollectingResponse sourceCollectingResponse)
    {
        if (sourceCollectingResponse is null)
            return null;

        return new SourceCollectingResponseDto
        {
            SourceId = sourceCollectingResponse.SourceId,
            CollectedSource = sourceCollectingResponse.CollectedSource.ToDto()
        };
    }
}
