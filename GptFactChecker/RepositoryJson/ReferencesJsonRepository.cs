using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;

namespace RepositoryJson;

public class ReferencesRepositoryJson : RepositoryBaseJson<Reference>
{
    private const string FileName = "Topics.json";

    public ReferencesRepositoryJson(IOptions<RepositoryJsonOptions> options)
        : base(options, FileName) { }
}
