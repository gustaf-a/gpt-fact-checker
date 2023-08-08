using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ReferencesRepositoryJson : RepositoryBaseJson<Reference>, IReferenceRepository
{
    private const string FileName = "References.json";

    public ReferencesRepositoryJson(IOptions<RepositoryJsonOptions> options)
        : base(options, FileName) { }
}
