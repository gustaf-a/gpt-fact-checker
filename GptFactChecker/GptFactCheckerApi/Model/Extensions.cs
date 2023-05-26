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
}
