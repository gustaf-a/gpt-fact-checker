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
        };
    }

    public static List<SourceDto> ToDtos(this List<Source> sources)
    {
        return sources.Select(s => s.ToDto()).ToList();
    }

    public static SourceDto ToDto(this Source source)
    {
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
        };
    }

    public static List<Claim> ToClaims(this IEnumerable<ClaimDto> claimDtos)
    {
        List<Claim> claims = new();

        foreach (ClaimDto claimDto in claimDtos)
        {
            Claim claim = new()
            {
                Id = claimDto.Id,
                ClaimSummarized = claimDto.ClaimSummarized,
                ClaimRawText = claimDto.ClaimRawText,
                Tags = claimDto.Tags
            };

            claims.Add(claim);
        }

        return claims;
    }

    public static List<ClaimDto> ToDtos(this IEnumerable<Claim> claims)
    {
        List<ClaimDto> claimDtos = new();

        foreach (Claim claim in claims)
        {
            ClaimDto claimDto = new()
            {
                Id = claim.Id,
                ClaimSummarized = claim.ClaimSummarized,
                ClaimRawText = claim.ClaimRawText,
                Tags = claim.Tags
            };

            claimDtos.Add(claimDto);
        }

        return claimDtos;
    }

}
